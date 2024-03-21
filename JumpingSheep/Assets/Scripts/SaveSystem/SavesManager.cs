using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum SaveType {
    Binary,
    Json
};

public class SavesManager : IService, IDisposable {
    private static string _savePath = Application.persistentDataPath;

    private SaveType _currentSaveType = SaveType.Binary;
    private Logger _logger;
    private Dictionary<SaveType, IStorageService> _saveServices = new Dictionary<SaveType, IStorageService>();

    public SavesManager(SaveManagerConfig _config, Logger logger) {
        if (_config.SavePath != "")
            _savePath = _config.SavePath;

        _currentSaveType = _config.SaveType;
        _logger = logger;

        InitializationServices();
    }

    public IStorageService CurrentService { get { return _saveServices[_currentSaveType]; } }

    public void Save(string key, object data, Action<bool> callback = null) {
        _saveServices[_currentSaveType].Save(key, data, callback);
    }

    public void Load<T>(string key, Action<T> callback) {
        _saveServices[_currentSaveType].Load(key, callback);
    }

    public void DeleteFile(string key) {
        string path = "";

        switch (_currentSaveType) {
            case SaveType.Binary:
                path = Path.Combine(_savePath, key, ".save");
                break;

            case SaveType.Json:
                path = Path.Combine(_savePath, key, ".json");
                break;

            default:
                break;
        }

        if (File.Exists(path)) {
            File.Delete(path);
            _logger.Log("SavesManager: DeleteFile - Deleted");
        }
        else 
            _logger.Log("SavesManager: DeleteFile - No file");
    }

    private void InitializationServices() {
        _saveServices.Add(SaveType.Binary, new BinaryToFileStorageService(_logger));
        _saveServices.Add(SaveType.Json, new JsonToFileStorageService(_logger));

        foreach (var iService in _saveServices) {
            iService.Value.SetPath(_savePath);
        }
    }

    public void Dispose() {
        
    }
}

