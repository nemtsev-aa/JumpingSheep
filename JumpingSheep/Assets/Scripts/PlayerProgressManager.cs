using GamePush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerProgressManager {
    private const string PlayerData = "PlayerData";
    private const string DefaultProgress = "DefaultPlayerProgress";

    private Logger _logger;
    private ProgressLoader _progressLoader;

    /// <summary>
    /// Текущий прогресс игрока
    /// </summary>
    public PlayerProgressData CurrentProgress { get; private set; }

    /// <summary>
    /// Прогресс в текущей игровой сессии
    /// </summary>
    public List<LevelProgressData> LevelProgress { get; private set; }

    /// <summary>
    /// Прогресс в текущем уровне
    /// </summary>
    public LevelProgressData CurrentLevelProgress { get; private set; }

    public PlayerProgressManager(Logger logger, ProgressLoader progressLoader) {
        _logger = logger;
        _progressLoader = progressLoader;
    }

    public async Task LoadProgress() {
        _logger.Log("PlayerProgress Loading");

        await _progressLoader.LoadPlayerProgress();

        if (TryPlayerProgressLoad(out PlayerProgressData playerData)) {
            CurrentProgress = playerData;
            
            await UpdateProgress(CurrentProgress.LevelProgressDatas);

            _logger.Log("PlayerProgress Updated");
        }

        _logger.Log("PlayerProgress Unupdated");

    }

    public void UpdateProgressByLevel(LevelProgressData levelProgressData) {
        LevelProgressData data = CurrentProgress.LevelProgressDatas.First(data => data.Index == levelProgressData.Index);

        data.SetStatus(levelProgressData.Status);
        data.SetStarsCount(levelProgressData.StarsCount);

        _progressLoader.SavePlayerProgress(CurrentProgress);
    }

    public LevelStatusTypes GetLevelProgressByName(int index) {
        LevelProgressData data = CurrentProgress.LevelProgressDatas.First(data => data.Index == index);
        return data.Status;
    }

    public int GetReadyLevelIndex() {
        LevelProgressData data = CurrentProgress.LevelProgressDatas.First(data => data.Status == LevelStatusTypes.Ready);
        return data.Index;
    }

    public void ResetLocalPlayerProgress() {
        string stringData = _progressLoader.LoadDefaultProgress().Result;

        ResetProgress(stringData);

        PlayerPrefs.SetString(DefaultProgress, stringData);
    }

    public void ResetCloudPlayerProgress() {
        string stringData = _progressLoader.LoadDefaultProgress().Result;

        ResetProgress(stringData);

        GP_Player.Set(PlayerData, stringData);
        GP_Player.Sync();
    }

    private void ResetProgress(string stringData) {
        CurrentProgress = JsonUtility.FromJson<PlayerProgressData>(stringData);

        foreach (var iLevelProgress in CurrentProgress.LevelProgressDatas) {
            iLevelProgress.SetStatus(LevelStatusTypes.Locked);
            iLevelProgress.SetStarsCount(0);
        }

        CurrentProgress.LevelProgressDatas[0].SetStatus(LevelStatusTypes.Ready);
        CurrentProgress.LevelProgressDatas[0].SetStarsCount(0);

        _progressLoader.SavePlayerProgress(CurrentProgress);
    }

    private bool TryPlayerProgressLoad(out PlayerProgressData data) {
        if (_progressLoader.PlayerProgressData == null) {
            data = null;
            _logger.Log("PlayerProgress empty");

            return false;
        }
        else {
            data = _progressLoader.PlayerProgressData;
            _logger.Log("PlayerProgress loaded");

            return true;
        }
    }

    private async Task UpdateProgress(List<LevelProgressData> levelProgressDatas) {
        List<LevelProgressData> progressDatas = CurrentProgress.LevelProgressDatas;

        foreach (var iLevelProgress in progressDatas) {
            LevelProgressData data = progressDatas.First(data => data.Index == iLevelProgress.Index);

            data.SetStatus(iLevelProgress.Status);
            data.SetStarsCount(iLevelProgress.StarsCount);

            await Task.Delay(1);
        }
    }
}
