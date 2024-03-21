using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class JsonToFileStorageService : IStorageService {
    private string _path;
    private Logger _logger;

    public JsonToFileStorageService(Logger logger) {
        _logger = logger;
    }

    public void SetPath(string path) {
        _path = path;
    }

    public void Save(string key, object data, Action<bool> callback = null) {
        string savePath = BuildPath(key);
        string json = JsonConvert.SerializeObject(data);
        //string json = JsonConvert.SerializeObject(data, Formatting.None,
        //                new JsonSerializerSettings() {
        //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //                });
        _logger.Log(json);

        try {
            if (File.Exists(savePath))
                File.Delete(savePath);

            using (var writer = new StreamWriter(savePath, true)) {
                writer.Write(json);
                writer.Close();
            }

            _logger.Log($"Data saved to Json successfully: {savePath}");
            callback?.Invoke(true);
        }
        catch (Exception e) {
            _logger.Log($"Data saved to Json invalid: {e.Message}");
        }
    }

    public void Load<T>(string key, Action<T> callback) {
        string savePath = BuildPath(key);

        if (File.Exists(savePath) == false)
            return;

        using (var fileStream = new StreamReader(savePath)) {
            var json = fileStream.ReadToEnd();
            var data = JsonConvert.DeserializeObject<T>(json);

            callback.Invoke(data);
        }
    }

    private string BuildPath(string key) {
        return Application.persistentDataPath + $"/{key}.json";
    }
}
