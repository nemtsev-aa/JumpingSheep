using System;
using GamePush;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class CloudToFileStorageService : IStorageService {
    private const string Key = "DefaultPlayerProgress";
    private const string DefaultPlayerProgress = "https://s3.eponesh.com/games/files/13071/DefaultPlayerProgress.json";
    private string _stringData;
    private Logger _logger;

    public CloudToFileStorageService(Logger logger) {
        _logger = logger;
    }

    public void Init(string path) {

    }

    public async void Load<T>(string key, Action<T> callback) {
        _logger.Log($"CloudToFileStorageService: PlayerProgress Loading...");

        string currentProgressData = GP_Player.GetString(key);

        if (currentProgressData != null) {
            _logger.Log($"CloudToFileStorageService_CurrentProgress: {currentProgressData}");
            callback?.Invoke(JsonConvert.DeserializeObject<T>(currentProgressData));
            return;
        }

        if (TryLocalProgressDataLoad(out string localData)) {
            _logger.Log($"CloudToFileStorageService_LocalProgress: {localData}");
            callback?.Invoke(JsonConvert.DeserializeObject<T>(localData));
            return;
        }

        var defaultProgress = await TryDefaultProgressDataLoad();
        if (defaultProgress) {
            _logger.Log($"CloudToFileStorageService_DefaultProgress: {defaultProgress}");
            callback?.Invoke(JsonConvert.DeserializeObject<T>(_stringData));
        }         
    }

    public void Save(string key, object data, Action<bool> callback = null) {
        string stringData = JsonConvert.SerializeObject(data);

        SaveInPlayerPrefs(stringData);

        GP_Player.Set(key, stringData);
        GP_Player.Sync();

        callback?.Invoke(true);
    }

    public bool TryLocalProgressDataLoad(out string stringData) {
        try 
        {
            stringData = PlayerPrefs.GetString(Key);
            return stringData != "";
        }
        catch (Exception) {
            stringData = "";
            return false;
        }
    }
    
    public async UniTask<bool> TryDefaultProgressDataLoad() {
        _stringData = await LoadDefaultProgress();

        return _stringData != "";
    }

    public async UniTask<string> LoadDefaultProgress() {
        var req = UnityWebRequest.Get(DefaultPlayerProgress);
        var op = await req.SendWebRequest();

        string defaultProgressToString = op.downloadHandler.text;
        
        if (defaultProgressToString != "")
            return defaultProgressToString;
        else
            return null;
    }

    public void SaveInPlayerPrefs(string stringData) {
        try {
            PlayerPrefs.SetString(Key, stringData);
        }
        catch (Exception) {
            throw;
        }
    }
}
