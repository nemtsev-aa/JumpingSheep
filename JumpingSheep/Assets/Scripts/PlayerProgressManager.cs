using GamePush;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerProgressManager {
    private const string PlayerData = "PlayerData";
    private const string DefaultProgress = "DefaultPlayerProgress";

    private Logger _logger;
    private PlayerProgressLoader _progressLoader;
    private PlayerProgressData _currentProgress;

    public PlayerProgressManager(Logger logger, SavesManager savesManager) {
        _logger = logger;
        _progressLoader = new PlayerProgressLoader(logger, savesManager);
    }

    public IReadOnlyList<LevelProgressData> LevelProgress => _currentProgress.LevelProgressDatas;

    public async Task LoadProgress() {
        _logger.Log("PlayerProgress Loading");

        _currentProgress = await _progressLoader.LoadPlayerProgress();

        if (_currentProgress != null) 
            _logger.Log("PlayerProgress updated success");
        else
            _logger.Log("PlayerProgress updated fialed");  
    }

    public void UpdateProgressByLevel(LevelProgressData levelProgressData) {
        LevelProgressData data = _currentProgress.LevelProgressDatas.First(data => data.Index == levelProgressData.Index);

        data.SetStatus(levelProgressData.Status);
        data.SetStarsCount(levelProgressData.StarsCount);

        _progressLoader.SavePlayerProgress(_currentProgress);
    }

    public LevelStatusTypes GetLevelProgressByName(int index) {
        LevelProgressData data = _currentProgress.LevelProgressDatas.First(data => data.Index == index);
        return data.Status;
    }

    public int GetReadyLevelIndex() {
        LevelProgressData data = _currentProgress.LevelProgressDatas.First(data => data.Status == LevelStatusTypes.Ready);
        return data.Index;
    }

    public int GetStarsCountByLevelIndex(int levelIndex) {
        LevelProgressData data = _currentProgress.LevelProgressDatas.First(data => data.Index == levelIndex);
        return data.StarsCount;
    }

    public void ResetLocalPlayerProgress() {
        var defaultPlayerProgress = _progressLoader.LoadDefaultProgress().Result;

        ResetProgress(defaultPlayerProgress);

        string defaultPlayerProgressToString = JsonConvert.SerializeObject(defaultPlayerProgress);
        PlayerPrefs.SetString(DefaultProgress, defaultPlayerProgressToString);
    }

    public void ResetCloudPlayerProgress() {
        var defaultPlayerProgress = _progressLoader.LoadDefaultProgress().Result;

        ResetProgress(defaultPlayerProgress);

        string defaultPlayerProgressToString = JsonConvert.SerializeObject(defaultPlayerProgress);
        GP_Player.Set(PlayerData, defaultPlayerProgressToString);
        GP_Player.Sync();
    }

    private void ResetProgress(PlayerProgressData data) {
        _currentProgress = data;

        foreach (var iLevelProgress in _currentProgress.LevelProgressDatas) {
            iLevelProgress.SetStatus(LevelStatusTypes.Locked);
            iLevelProgress.SetStarsCount(0);
        }

        _currentProgress.LevelProgressDatas[0].SetStatus(LevelStatusTypes.Ready);
        _currentProgress.LevelProgressDatas[0].SetStarsCount(0);

        _progressLoader.SavePlayerProgress(_currentProgress);
    }

    private void UpdateProgress() {
        foreach (var iLevelProgress in LevelProgress) {
            LevelProgressData data = LevelProgress.First(data => data.Index == iLevelProgress.Index);

            data.SetStatus(iLevelProgress.Status);
            data.SetStarsCount(iLevelProgress.StarsCount);
        }
    }
}
