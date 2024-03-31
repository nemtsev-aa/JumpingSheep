using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class ProgressLoader {
    private const string Key = "PlayerData";
    private const string DefaultPlayerProgress = "https://s3.eponesh.com/games/files/13071/DefaultPlayerProgress.json";

    private SavesManager _savesManager;
    private Logger _logger;
    private PlayerProgressData _playerData;
    private bool _isLoadComplete;

    public ProgressLoader(Logger logger, SavesManager savesManager) {
        _savesManager = savesManager;
        _logger = logger;
    }

    public PlayerProgressData PlayerProgressData => _playerData;

    private CloudToFileStorageService _saveService => (CloudToFileStorageService)_savesManager.CurrentService;

    public async Task LoadPlayerProgress() {
        _isLoadComplete = false;
        _saveService.Load<PlayerProgressData>(Key, OnLevelProgressLoaded);

        while (_isLoadComplete) {
            await Task.Yield();
        }
    }

    public async Task<string> LoadDefaultProgress() {
        UnityWebRequest www = UnityWebRequest.Get(DefaultPlayerProgress);
        www.SendWebRequest();

        while (!www.isDone) {
            await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success) {
            string defaultProgressData = www.downloadHandler.text;

            _logger.Log($"LoadDefaultProgress complited: {defaultProgressData}");
            return defaultProgressData;
        }
        else {
            _logger.Log($"LoadDefaultProgress falled: {www.error}");
            return null;
        }

    }

    public void SavePlayerProgress(PlayerProgressData playerData) => _savesManager.Save(Key, playerData, OnLevelProgressSaved);

    private void OnLevelProgressSaved(bool status) {
        if (status == true)
            _logger.Log("Save complited");
        else
            _logger.Log("Save falled");
    }

    private async void OnLevelProgressLoaded(PlayerProgressData playerData) {
        _isLoadComplete = true;

        if (playerData != null) {
            _playerData = playerData;
            _logger.Log($"LoadCurrentProgress complited: {_playerData}");
            return;
        }

        _logger.Log($"{Key}.json not found or empty");
        _isLoadComplete = false;

        string defaultProgressData = await LoadDefaultProgress();
        _logger.Log($"DefaultProgressData: {defaultProgressData}");

        if (defaultProgressData != null) {
            _playerData = JsonConvert.DeserializeObject<PlayerProgressData>(defaultProgressData);

            _logger.Log($"LoadDefaultProgress complited: {_playerData.LevelProgressDatas}");
            _isLoadComplete = true;

            return;
        }
    }
}
