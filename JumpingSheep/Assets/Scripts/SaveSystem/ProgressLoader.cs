using GamePush;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressLoader {
    private const string Key = "PlayerData";

    private LevelConfigs _configs;
    private SavesManager _savesManager;
    private Logger _logger;
    private PlayerData _playerData;

    public ProgressLoader(LevelConfigs configs, SavesManager savesManager, Logger logger) {
        _configs = configs;
        _savesManager = savesManager;
        _logger = logger;
    }

    public PlayerData PlayerData => _playerData;

    public void LoadPlayerData() {
        _savesManager.Load<PlayerData>(Key, OnLevelProgressLoaded);
    }

    public void SaveLevelProgress(PlayerData playerData) {
        _savesManager.Save(Key, playerData, OnLevelProgressSaved);
    }

    public void ResetLocalPlayerProgress() {
        _configs.ResetProgress();

        PlayerData playerData = new PlayerData(_configs.Progress);
        string stringData = JsonConvert.SerializeObject(playerData);

        PlayerPrefs.SetString(Key, stringData);
    }

    public void ResetCloudPlayerProgress() {
        _configs.ResetProgress();

        PlayerData playerData = new PlayerData(_configs.Progress);
        string stringData = JsonConvert.SerializeObject(playerData);

        GP_Player.Set(Key, stringData);
        GP_Player.Sync();
    }
    
    private void OnLevelProgressSaved(bool status) {
        if (status == true)
            _logger.Log("Save complited");
        else
            _logger.Log("Save falled");
    }

    private void OnLevelProgressLoaded(PlayerData playerData) {
        if (playerData == null) {
            _logger.Log($"{Key}.json not found or empty");
            return;
        }

        if (playerData.LevelProgressDatas.Count() == 0) {
            _logger.Log($"List {Key} in Json-file is empty");
            return;
        }

        _playerData = playerData;
        _logger.Log("Load complited");
    }
}
