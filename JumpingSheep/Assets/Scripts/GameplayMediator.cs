using System;
using System.Linq;
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

    public void SetPause(bool isPaused) => _pauseHandler.SetPause(isPaused);

    public void Reset() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;
    }

    public void ResetPlayerProgress() {
        if (_adManager.Platform == GamePush.Platform.YANDEX)
            ResetCloudPlayerProgress();

        ResetLocalPlayerProgress();
    }

    public void LevelPreparation(LevelConfig config) {
        if (_currentLevelConfig != config)
            _currentLevelConfig = config;

        _score.SetLevelConfig(_currentLevelConfig);
        _sheepCounter.SetLevelConfig(_currentLevelConfig);
        _qTESystem.SetLevelConfig(_currentLevelConfig);

        StartGameplay();
    }

    public void FinishGameplay(Switchovers switchover) {
        _switchover = switchover;

        var newStarsCount = _score.StarsCount;

        if (newStarsCount > 0) {
            UnlockLevel();

            var currentLevelIndex = _currentLevelConfig.Index;
            var oldStarsCount = _playerProgressManager.GetStarsCountByLevelIndex(currentLevelIndex);
            var currentLevelProgressData = new LevelProgressData(LevelStatusTypes.Complited, currentLevelIndex, Mathf.Max(newStarsCount, oldStarsCount));

            _playerProgressManager.UpdateProgressByLevel(currentLevelProgressData);
        }

        _playerProgressManager.SavePlayerProgress();

        Reset();
        MakeTransition();
    }

    #region Service Events
    private void AddListener() {
        _spawner.SheepCreated += OnSheepCreated;
        _sheepCounter.SheepIsOver += OnSheepIsOver;
        _qTESystem.EventFinished += OnQTESystemEventFinished;

        _adManager.FullscreenStarted += OnFullscreenStarted;
        _adManager.FullscreenClosed += OnFullscreenClosed;
        _adManager.RewardedClosed += OnRewardedClosed;
        _adManager.RewardedReward += OnRewardedReward;
    }

    private void RemoveLisener() {
        _spawner.SheepCreated -= OnSheepCreated;
        _sheepCounter.SheepIsOver -= OnSheepIsOver;
        _qTESystem.EventFinished -= OnQTESystemEventFinished;

        _adManager.FullscreenStarted -= OnFullscreenStarted;
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

    private void OnFullscreenStarted() => SetPause(true);

    private void OnFullscreenClosed(bool value) => ResumeGameplay();

    private void OnRewardedReward(string key) { }

    private void OnRewardedClosed(bool value) { }

    #endregion

    private void StartGame() => _uIManager.ShowMainMenuDialog();

    private void StartGameplay() {
        _qTESystem.CreateQTESoundManager(_soundsLoader);
        _environmentSound.PlaySound(MusicType.Gameplay);
        _spawner.CreateSheep(_currentLevelConfig.Color);

        ShowFullScreenAds();
    }

    private void ResumeGameplay() => SetPause(false);

    private void ShowFullScreenAds() {

        if (_adManager == null)
            return;

        _adManager.TryShowFullScreen();
    }

    private void UnlockLevel() {
        var configs = _configs.Configs;

        int index = configs.IndexOf(_currentLevelConfig) + 1;

        if (index > configs.Count) {
            StartGame();
            return;
        }

        var levelIndex = _configs.Configs[index].Index;
        LevelStatusTypes currentStatus = _playerProgressManager.GetLevelStatusTypeByIndex(levelIndex);

        if (currentStatus == LevelStatusTypes.Locked) {
            var data = new LevelProgressData(LevelStatusTypes.Ready, levelIndex, 0);
            _playerProgressManager.UpdateProgressByLevel(data);
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
