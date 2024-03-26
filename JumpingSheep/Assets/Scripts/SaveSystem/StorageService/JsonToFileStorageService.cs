using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class JsonToFileStorageService : IStorageService {
    private string _path;

    public void Init(string path) {
        _path = path;
    }

    public void Save(string key, object data, Action<bool> callback = null) {
        string savePath = BuildPath(key);
        string json = JsonConvert.SerializeObject(data);
        //string json = JsonConvert.SerializeObject(data, Formatting.None,
        //                new JsonSerializerSettings() {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                });

        if (File.Exists(savePath)) {
            using (var fileStream = new StreamWriter(savePath)) {
                fileStream.Write(json);
            }
        }

#if UNITY_EDITOR
        Debug.Log($"Data saved to Json successfully: {savePath}");
        callback?.Invoke(true);
#endif
    }

    public void Load<T>(string key, Action<T> callback) {
        string savePath = BuildPath(key);

        using (var fileStream = new StreamReader(savePath)) {
            var json = fileStream.ReadToEnd();
            var data = JsonConvert.DeserializeObject<T>(json);

            callback.Invoke(data);
        }
    }

    private string BuildPath(string key) {
        return Path.Combine(_path, key + ".json");
    }
}
