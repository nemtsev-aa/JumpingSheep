using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class PlayerProgressLoader {
    private const string Key = "PlayerData";
    private const string DefaultPlayerProgress = "https://s3.eponesh.com/games/files/13071/DefaultPlayerProgress.json";

    private SavesManager _savesManager;
    private Logger _logger;
    private PlayerProgressData _playerData;

    private bool _isLoadComplete;

    public PlayerProgressLoader(Logger logger, SavesManager savesManager) {
        _logger = logger;
        _savesManager = savesManager;
    }

    private CloudToFileStorageService _saveService => (CloudToFileStorageService)_savesManager.CurrentService;

    public async Task<PlayerProgressData> LoadPlayerProgress() {
        _isLoadComplete = false;
        _saveService.Load<PlayerProgressData>(Key, OnLevelProgressLoaded);

        while (_isLoadComplete == false) {
            await Task.Yield();
        }

        return _playerData;
    }

    public async Task<PlayerProgressData> LoadDefaultProgress() {
        UnityWebRequest www = UnityWebRequest.Get(DefaultPlayerProgress);
        www.SendWebRequest();

        while (!www.isDone) {
            await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success)
            return JsonConvert.DeserializeObject<PlayerProgressData>(www.downloadHandler.text);
               

        _logger.Log($"LoadDefaultProgress falled: {www.error}");
        return null;
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

        var defaultProgressData = await LoadDefaultProgress();

        if (defaultProgressData != null) {
            _playerData = defaultProgressData;

            _logger.Log($"LoadDefaultProgress complited!");
            _isLoadComplete = true;

            return;
        }
    }
}
