using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
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

    public async UniTask<PlayerProgressData> LoadPlayerProgress() {
        _isLoadComplete = false;
        _saveService.Load<PlayerProgressData>(Key, OnLevelProgressLoaded);

        while (_isLoadComplete == false) {
            await UniTask.Yield();
        }

        return _playerData;
    }

    public async UniTask<PlayerProgressData> LoadDefaultProgress() {

        string defaultProgressToString = await GetTextAsync(UnityWebRequest.Get(DefaultPlayerProgress));
        
        if (defaultProgressToString != "") {
            _logger.Log($"LoadDefaultProgress succeeded");
            
            return JsonConvert.DeserializeObject<PlayerProgressData>(defaultProgressToString);
        }
        else 
        {
            _logger.Log($"LoadDefaultProgress falled!");
            return null;
        } 
    }

    private async UniTask<string> GetTextAsync(UnityWebRequest req) {
        var op = await req.SendWebRequest();
        return op.downloadHandler.text;
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

        if (playerData.LevelProgressDatas != null) {
            _playerData = playerData;
            int complitedLevelCount = _playerData.LevelProgressDatas.Where(level => level.Status == LevelStatusTypes.Complited).Count();
            _logger.Log($"CurrentPlayerProgress: {complitedLevelCount} levels complited");

            return;
        }

        _logger.Log($"CurrentPlayerProgress is empty!");
        _isLoadComplete = false;

        var defaultProgressData = await LoadDefaultProgress();

        if (defaultProgressData != null) {
            _playerData = defaultProgressData;

            _logger.Log($"DefaultPlayerProgress loading success");
            _isLoadComplete = true;

            return;
        }
    }
}
