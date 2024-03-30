using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
public enum Switchovers {
    MainMenu,
    CurrentLevel,
    NextLevel,
}

public class GameplayMediator : MonoBehaviour, IPause, IDisposable {
    private PauseHandler _pauseHandler;
    private SheepSpawner _spawner;
    private SheepQuantityCounter _sheepCounter;
    private LevelConfigs _configs;
    private ProgressLoader _progressLoader;
    private AdManager _adManager;
    private UIManager _uIManager;

    private EnvironmentSoundManager _environmentSound;
    private SheepSFXManager _sheepSFXManager;
    private LevelConfig _currentLevelConfig;
    private QTESystem _qTESystem;
    private Score _score;

    private Sheep _currentSheep;
    private bool _sheepOver;
    private Switchovers _switchover;

    [Inject]
    public void Construct(PauseHandler pauseHandler, SheepSpawner spawner,
                          QTESystem qTESystem, Score score, SheepQuantityCounter sheepCounter,
                          LevelConfigs configs, ProgressLoader progressLoader, AdManager adManager) {

        _pauseHandler = pauseHandler;
        _spawner = spawner;
        _qTESystem = qTESystem;
        _score = score;
        _sheepCounter = sheepCounter;
        _configs = configs;
        _progressLoader = progressLoader;
        _adManager = adManager;
    }

    public void Init(UIManager uIManager, EnvironmentSoundManager environmentSound, SheepSFXManager sheepSFXManager) {
        _uIManager = uIManager;
        _environmentSound = environmentSound;
        _sheepSFXManager = sheepSFXManager;

        AddListener();

        StartGame();
    }

    private void StartGame() {
        _environmentSound.PlaySound(MusicType.UI);
        _uIManager.ShowMainMenuDialog();
    }

    public void SetPause(bool isPaused) => _pauseHandler.SetPause(isPaused);

    public void Reset() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;
    }

    #region Service Events
    private void AddListener() {
        _spawner.SheepCreated += OnSheepCreated;
        _sheepCounter.SheepIsOver += OnSheepIsOver;
        _qTESystem.EventFinished += OnQTESystemEventFinished;

        _adManager.FullscreenClosed += OnFullscreenClosed;
        _adManager.RewardedClosed += OnRewardedClosed;
        _adManager.RewardedReward += OnRewardedReward;
    }

    private void RemoveLisener() {
        _spawner.SheepCreated -= OnSheepCreated;
        _sheepCounter.SheepIsOver -= OnSheepIsOver;
        _qTESystem.EventFinished -= OnQTESystemEventFinished;

        _adManager.FullscreenClosed -= OnFullscreenClosed;
        _adManager.RewardedClosed -= OnRewardedClosed;
        _adManager.RewardedReward -= OnRewardedReward;
    }

    #endregion

    #region Sheeps Events

    private void OnSheepCreated(Sheep sheep) {
        _currentSheep = sheep;
        _currentSheep.Init(_qTESystem, _pauseHandler, _sheepSFXManager);

        _currentSheep.EventsHandler.Striked += OnSheepStriked;
        _currentSheep.EventsHandler.Jumped += OnSheepJumped;
    }

    private void OnSheepStriked() {
        _score.SetQTEEventResult(false);

        DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep(_currentLevelConfig.Color);
    }

    private void OnSheepJumped() {
        _score.SetQTEEventResult(true);

        DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep(_currentLevelConfig.Color);
    }

    private void DestroyCurrentSheep() {
        _sheepCounter.TakeSheep();
        _qTESystem.Reset();

        Destroy(_currentSheep.gameObject);

        _currentSheep = null;
    }

    private void OnSheepIsOver() {
        _sheepOver = true;
        _environmentSound.PlaySound(MusicType.GameOver);
    }

    #endregion

    #region AD Events
    private void OnFullscreenClosed(bool value) => MakeTransition();

    private void OnRewardedReward(string key) { }

    private void OnRewardedClosed(bool value) { }

    #endregion

    public bool TryProgressLoad() {
        LoadPlayerProgress();
        return true;
    }

    public void LevelPreparation(LevelConfig config) {
        if (_currentLevelConfig != config)
            _currentLevelConfig = config;

        _score.SetLevelConfig(_currentLevelConfig);
        _sheepCounter.SetLevelConfig(_currentLevelConfig);
        _qTESystem.SetLevelConfig(_currentLevelConfig);

        StartGameplay();
    }

    private void OnQTESystemEventFinished(bool result) => _score.SetSwipeResult(result);

    public void LoadPlayerProgress() {
        _progressLoader.LoadPlayerData();
        var playerData = _progressLoader.PlayerData;

        if (playerData == null)
            return;

        _configs.UpdateProgress(playerData.LevelProgressDatas);
    }

    public void ResetPlayerProgress() {
        if (_adManager.Platform == GamePush.Platform.YANDEX)
            ResetCloudPlayerProgress();

        ResetLocalPlayerProgress();
    }
    
    private void SetPlayerProgress() {
        List<LevelProgressData> data = new List<LevelProgressData>();

        foreach (var iLevelConfig in _configs.Configs) {
            LevelProgressData progress = iLevelConfig.Progress;
            data.Add(progress);
        }

        PlayerData playerData = new PlayerData(data);
        _progressLoader.SaveLevelProgress(playerData);
    }

    private void StartGameplay() {
        _qTESystem.CreateQTESoundManager();
        _environmentSound.PlaySound(MusicType.Gameplay);
        _spawner.CreateSheep(_currentLevelConfig.Color);
    }
    
    public void ResumeGameplay() {
        if (_pauseHandler.IsPaused) 
            SetPause(false);
    }

    public void FinishGameplay(Switchovers switchover) {
        _switchover = switchover;

        if (_score.StarsCount > 0) {
            UnlockLevel();

            _currentLevelConfig.Progress.SetStatus(LevelStatusTypes.Complited);

            var sratsCount = _score.StarsCount;
            if (_score.StarsCount > _currentLevelConfig.Progress.StarsCount) {
                _currentLevelConfig.Progress.SetStarsCount(sratsCount);
                
                SetPlayerProgress();
            }
        }

        Reset();

        ShowFullScreenAds();
    }

    private void ShowFullScreenAds() {
        if (_adManager != null && _adManager.Platform == GamePush.Platform.YANDEX)
            _adManager.ShowFullScreen();
        else
            OnFullscreenClosed(true);
    }

    private void UnlockLevel() {
        int index = _configs.Configs.IndexOf(_currentLevelConfig) + 1;

        if (index > _configs.Configs.Count) {
            StartGame();
        }
        else 
        {
            var level = _configs.Configs[index];

            if (level.Progress.Status == LevelStatusTypes.Locked)
                level.Progress.SetStatus(LevelStatusTypes.Ready);
        }
    }

    private LevelConfig GetLevelConfigByStatus(LevelStatusTypes status) {
        return _configs.Configs.First(config => config.Progress.Status == status);
    }

    private void ResetLocalPlayerProgress() => _progressLoader.ResetLocalPlayerProgress();

    private void ResetCloudPlayerProgress() => _progressLoader.ResetCloudPlayerProgress();
    
    private void MakeTransition() {
        switch (_switchover) {
            case Switchovers.MainMenu:
                StartGame();

                break;

            case Switchovers.CurrentLevel:
                LevelPreparation(_currentLevelConfig);

                break;

            case Switchovers.NextLevel:
                _currentLevelConfig = GetLevelConfigByStatus(LevelStatusTypes.Ready);
                LevelPreparation(_currentLevelConfig);

                break;

            default:
                throw new ArgumentException($"Invalid Switchovers value: {_switchover}");
        }
    }

    public void Dispose() {
        RemoveLisener();
    }
}
