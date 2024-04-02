using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
public enum Switchovers {
    MainMenu,
    CurrentLevel,
    NextLevel,
}

public class GameplayMediator : MonoBehaviour, IPause, IDisposable {
    public const float InitDelay = 0.1f;

    private Logger _logger;
    private PauseHandler _pauseHandler;
    private SheepSpawner _spawner;
    private SheepQuantityCounter _sheepCounter;
    private LevelConfigs _configs;
    private PlayerProgressLoader _progressLoader;
    private AdManager _adManager;
    private PlayerProgressManager _playerProgressManager;
    private UIManager _uIManager;
    private SoundsLoader _soundsLoader;
    private EnvironmentSoundManager _environmentSound;
    private SheepSFXManager _sheepSFXManager;
    private LevelConfig _currentLevelConfig;
    private LevelProgressData _currentLevelProgressData;
    private QTESystem _qTESystem;
    private Score _score;

    private Sheep _currentSheep;
    private bool _sheepOver;
    private Switchovers _switchover;

    [Inject]
    public void Construct(Logger logger, PauseHandler pauseHandler, SheepSpawner spawner,
                          QTESystem qTESystem, Score score, SheepQuantityCounter sheepCounter,
                          LevelConfigs configs, PlayerProgressLoader progressLoader, AdManager adManager, 
                          PlayerProgressManager playerProgressManager) {

        _logger = logger;
        _pauseHandler = pauseHandler;
        _spawner = spawner;
        _qTESystem = qTESystem;
        _score = score;
        _sheepCounter = sheepCounter;
        _configs = configs;
        _adManager = adManager;

        _progressLoader = progressLoader;
        _playerProgressManager = playerProgressManager;
    }

    public void Init(UIManager uIManager, SoundsLoader soundsLoader, EnvironmentSoundManager environmentSound, SheepSFXManager sheepSFXManager) {
        _uIManager = uIManager;
        _soundsLoader = soundsLoader;
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

    public void LevelPreparation(LevelConfig config) {
        if (_currentLevelConfig != config)
            _currentLevelConfig = config;

        _score.SetLevelConfig(_currentLevelConfig);
        _sheepCounter.SetLevelConfig(_currentLevelConfig);
        _qTESystem.SetLevelConfig(_currentLevelConfig);

        StartGameplay();
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

    public void ResetPlayerProgress() {
        if (_adManager.Platform == GamePush.Platform.YANDEX)
            ResetCloudPlayerProgress();

        ResetLocalPlayerProgress();
    }

    private void StartGameplay() {
        _qTESystem.CreateQTESoundManager(_soundsLoader);
        _environmentSound.PlaySound(MusicType.Gameplay);
        _spawner.CreateSheep(_currentLevelConfig.Color);
    }
    
    private void ResumeGameplay() {
        if (_pauseHandler.IsPaused) 
            SetPause(false);
    }

    public void FinishGameplay(Switchovers switchover) {
        _switchover = switchover;

        var starsCount = _score.StarsCount;

        if (starsCount > 0) {
            UnlockLevel();

            var playerStarsCount = _playerProgressManager.GetStarsCountByLevelIndex(_currentLevelConfig.Index);
            _currentLevelProgressData = new LevelProgressData(LevelStatusTypes.Complited, _currentLevelConfig.Index, playerStarsCount);
            
            if (starsCount > playerStarsCount) {
                _currentLevelProgressData.SetStarsCount(starsCount);
                
                SetPlayerProgress();
            }
        }

        Reset();

        ShowFullScreenAds();
    }

    private void SetPlayerProgress() =>
        _playerProgressManager.UpdateProgressByLevel(_currentLevelProgressData);
        
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
            LevelStatusTypes currentStatus = _playerProgressManager.GetLevelProgressByName(level.Index);

            if (currentStatus == LevelStatusTypes.Locked) {
                var data = new LevelProgressData(LevelStatusTypes.Ready, level.Index, 0);
                _playerProgressManager.UpdateProgressByLevel(data);
            }
        }
    }

    private void ResetLocalPlayerProgress() => _playerProgressManager.ResetLocalPlayerProgress();

    private void ResetCloudPlayerProgress() => _playerProgressManager.ResetCloudPlayerProgress();
    
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

    private LevelConfig GetLevelConfigByStatus(LevelStatusTypes status) {
        int readyLevelIndex = _playerProgressManager.GetReadyLevelIndex();

        return _configs.Configs.First(config => config.Index == readyLevelIndex);
    }

    private void OnQTESystemEventFinished(bool result) => _score.SetSwipeResult(result);

    public void Dispose() {
        RemoveLisener();
    }
}
