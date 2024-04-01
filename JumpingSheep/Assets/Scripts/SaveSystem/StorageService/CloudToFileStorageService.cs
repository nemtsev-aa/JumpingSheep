using GamePush;
using System;
using UnityEngine;

public class CloudToFileStorageService : IStorageService {
    private const string Key = "DefaultPlayerProgress";

    public void Init(string path) {

    }

    public void Load<T>(string key, Action<T> callback) {
        string currentProgressData = GP_Player.GetString(key);

        if (currentProgressData != null)
            callback?.Invoke(JsonUtility.FromJson<T>(currentProgressData));
        else {

            if (TryLocalProgressDataLoad(out string localData))
                callback?.Invoke(JsonUtility.FromJson<T>(localData));

            if (TryDefaultProgressDataLoad(out string defaultData))
                callback?.Invoke(JsonUtility.FromJson<T>(defaultData));
        }
    }

    public void Save(string key, object data, Action<bool> callback = null) {
        string stringData = JsonUtility.ToJson(data);

        SaveInPlayerPrefs(stringData);

        GP_Player.Set(key, stringData);
        GP_Player.Sync();

        callback?.Invoke(true);
    }

    public bool TryDefaultProgressDataLoad(out string stringData) {
        try {
            stringData = GP_Variables.GetString(Key);
            return stringData != "";
        }
        catch (Exception) {
            stringData = "";
            return false;
        }
    }

    public bool TryLocalProgressDataLoad(out string stringData) {
        try {
            stringData = PlayerPrefs.GetString(Key);
            return stringData != "";
        }
        catch (Exception) {
            stringData = "";
            return false;
        }
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
