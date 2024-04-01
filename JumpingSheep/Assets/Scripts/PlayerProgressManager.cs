using GamePush;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerProgressManager {
    private const string PlayerData = "PlayerData";
    private const string DefaultProgress = "DefaultPlayerProgress";

    private Logger _logger;
    private ProgressLoader _progressLoader;
    private PlayerProgressData _currentProgress;

    public PlayerProgressManager(Logger logger, ProgressLoader progressLoader) {
        _logger = logger;
        _progressLoader = progressLoader;
    }

    public IReadOnlyList<LevelProgressData> LevelProgress => _currentProgress.LevelProgressDatas;

    public async Task LoadProgress() {
        _logger.Log("PlayerProgress Loading");

        await _progressLoader.LoadPlayerProgress();

        if (TryPlayerProgressLoad(out PlayerProgressData playerData)) {
            _currentProgress = playerData;
            _logger.Log("PlayerProgress updated success");
            return;
        }

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
        _currentProgress = JsonUtility.FromJson<PlayerProgressData>(stringData);

        foreach (var iLevelProgress in _currentProgress.LevelProgressDatas) {
            iLevelProgress.SetStatus(LevelStatusTypes.Locked);
            iLevelProgress.SetStarsCount(0);
        }

        _currentProgress.LevelProgressDatas[0].SetStatus(LevelStatusTypes.Ready);
        _currentProgress.LevelProgressDatas[0].SetStarsCount(0);

        _progressLoader.SavePlayerProgress(_currentProgress);
    }

    private bool TryPlayerProgressLoad(out PlayerProgressData playerData) {
        if (_progressLoader.PlayerProgressData == null) {
            playerData = null;
            return false;
        }

        playerData = _progressLoader.PlayerProgressData;
        return true;
    }

    private void UpdateProgress() {
        foreach (var iLevelProgress in LevelProgress) {
            LevelProgressData data = LevelProgress.First(data => data.Index == iLevelProgress.Index);

            data.SetStatus(iLevelProgress.Status);
            data.SetStarsCount(iLevelProgress.StarsCount);
        }
    }
}
