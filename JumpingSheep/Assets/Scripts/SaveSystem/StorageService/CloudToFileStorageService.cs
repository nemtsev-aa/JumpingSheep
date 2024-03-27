using GamePush;
using Newtonsoft.Json;
using System;
using UnityEngine;

public class CloudToFileStorageService : IStorageService {
    public void Init(string path) { }
    
    public void Load<T>(string key, Action<T> callback) {
        string stringData = GP_Player.GetString(key);

        if (String.IsNullOrEmpty(stringData) == true) {
            stringData = PlayerPrefs.GetString(key);

            if (String.IsNullOrEmpty(stringData) == false)
                callback?.Invoke(JsonConvert.DeserializeObject<T>(stringData));
        }
        else
            callback?.Invoke(JsonConvert.DeserializeObject<T>(stringData));
    }

    public void Save(string key, object data, Action<bool> callback = null) {
        string stringData = JsonConvert.SerializeObject(data);

        PlayerPrefs.SetString(key, stringData);

        GP_Player.Set(key, stringData);
        GP_Player.Sync();

        callback?.Invoke(true);
    }
}
