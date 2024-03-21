using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BinaryToFileStorageService : IStorageService {
    private string _path;
    private Logger _logger;

    public BinaryToFileStorageService(Logger logger) {
        _logger = logger;
    }

    public void SetPath(string path) {
        _path = path;
    }

    public void Save(string key, object data, Action<bool> callback = null) {
        string savePath = BuildPath(key);
        
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate)) {
            formatter.Serialize(fs, data); // Cериализуем весь массив
        }

        _logger.Log($"Data saved to Binary successfully: {savePath}");
        callback?.Invoke(true);
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

        if (File.Exists(savePath) == false)
            return;

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

