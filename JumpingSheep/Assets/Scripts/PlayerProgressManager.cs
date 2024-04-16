using Cysharp.Threading.Tasks;
using GamePush;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProgressManager {
    private const string PlayerData = "PlayerData";
    private const string DefaultProgress = "DefaultPlayerProgress";

    private readonly Logger _logger;
    private readonly PlayerProgressLoader _progressLoader;
    private PlayerProgressData _currentProgress;

    public PlayerProgressManager(Logger logger, SavesManager savesManager) {
        _logger = logger;
        _progressLoader = new PlayerProgressLoader(logger, savesManager);
    }

    private IReadOnlyList<LevelProgressData> LevelProgress => _currentProgress.LevelProgressDatas;

    public async UniTask LoadProgress() {
        _logger.Log("PlayerProgress Loading...");

        _currentProgress = await _progressLoader.LoadPlayerProgress();

        if (_currentProgress.LevelProgressDatas != null) 
            _logger.Log("PlayerProgress loaded success");
        else
            _logger.Log("PlayerProgress loaded fialed");  
    }

    public void UpdateProgressByLevel(LevelProgressData levelProgressData) {
        LevelProgressData data = _currentProgress.LevelProgressDatas.First(data => data.Index == levelProgressData.Index);
        
        data.SetStatus(levelProgressData.Status);
        data.SetStarsCount(levelProgressData.StarsCount); 
    }

    public void SavePlayerProgress() => _progressLoader.SavePlayerProgress(_currentProgress);

    public LevelStatusTypes GetLevelStatusTypeByIndex(int index) {
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

    public async void ResetLocalPlayerProgress() {
        var defaultPlayerProgress = await _progressLoader.LoadDefaultProgress();

        ResetProgress(defaultPlayerProgress);

        string defaultPlayerProgressToString = JsonConvert.SerializeObject(defaultPlayerProgress);
        PlayerPrefs.SetString(DefaultProgress, defaultPlayerProgressToString);
    }

    public async void ResetCloudPlayerProgress() {
        var defaultPlayerProgress = await _progressLoader.LoadDefaultProgress();

        ResetProgress(defaultPlayerProgress);

        string defaultPlayerProgressToString = JsonConvert.SerializeObject(defaultPlayerProgress);
        GP_Player.Set(PlayerData, defaultPlayerProgressToString);
        GP_Player.Sync();
    }

    public IReadOnlyList<LevelProgressData> GetLevelProgress() {
        return _currentProgress.LevelProgressDatas;
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
