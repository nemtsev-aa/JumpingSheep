using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryToFileStorageService : IStorageService {
    private string _path;

    public void Init(string path) {
        _path = path;
    }
    
    public void Save(string key, object data, Action<bool> callback = null) {
        string savePath = BuildPath(key);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate)) {
            formatter.Serialize(fs, data); // Cериализуем весь массив
        }

#if UNITY_EDITOR
        Debug.Log($"Data saved to Binary successfully: {savePath}");
        callback?.Invoke(true);
#endif

    }
    public void Serialize<T>(T obj) {
        BinaryFormatter formatter = new BinaryFormatter();
        using (Stream stream = new MemoryStream()) {
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            T deserializedObj = (T)formatter.Deserialize(stream);
        }
    }

    public void Load<T>(string key, Action<T> callback) {
        string savePath = BuildPath(key);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fs = new FileStream(savePath, FileMode.Open)) {
            if (fs.Length > 0) {
                T data = (T)formatter.Deserialize(fs);
                callback.Invoke(data);
            }
        }
    }

    private string BuildPath(string key) {
        return Path.Combine(_path, key + ".save");
    }
}
    
