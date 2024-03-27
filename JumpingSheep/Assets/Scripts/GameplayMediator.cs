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
    private DialogSwitcher _dialogSwitcher;
    private EnvironmentSoundManager _environmentSound;

    private LevelConfig _currentLevelConfig;
    private QTESystem _qTESystem;
    private Score _score;

    private Sheep _currentSheep;
    private bool _sheepOver;
    private Switchovers _switchover;

    private MainMenuDialog MainMenuDialog => _uIManager.MainMenuDialog;
    private LevelSelectionDialog LevelSelectionDialog => _uIManager.LevelSelectionDialog;
    private GameDialog GameDialog => _uIManager.GameDialog;
    private SettingsDialog SettingsDialog => _uIManager.SettingsDialog;
    private AboutDialog AboutDialog => _uIManager.AboutDialog;

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

    public void Init(UIManager uIManager, EnvironmentSoundManager environmentSound) {
        _uIManager = uIManager;
        _environmentSound = environmentSound;

        _dialogSwitcher = uIManager.DialogSwitcher;
        AddListener();
        ShowMainMenuDialog();
    }

    public void SetPause(bool isPaused) => _pauseHandler.SetPause(isPaused);

    public void Reset() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;
    }

    #region Switching Dialogs

    private void ShowMainMenuDialog() {
        _environmentSound.PlaySound(MusicType.UI);
        _dialogSwitcher.ShowDialog(DialogTypes.MainMenu);
    }

    private void ShowGameplayDialog() {
        _environmentSound.PlaySound(MusicType.Gameplay);
        _dialogSwitcher.ShowDialog(DialogTypes.Game);
    }

    private void ShowSettings() => _dialogSwitcher.ShowDialog(DialogTypes.Settings);

    private void ShowAboutDialog() => _dialogSwitcher.ShowDialog(DialogTypes.About);

    private void ShowLevelSelectionDialog() => _dialogSwitcher.ShowDialog(DialogTypes.LevelSelection);

    private void MakeTransition() {
        switch (_switchover) {
            case Switchovers.MainMenu:
                ShowMainMenuDialog();
                break;

            case Switchovers.CurrentLevel:
                OnLevelStarted(_currentLevelConfig);
                break;

            case Switchovers.NextLevel:
                _currentLevelConfig = GetLevelConfigByStatus(LevelStatusTypes.Ready);
                OnLevelStarted(_currentLevelConfig);
                break;

            default:
                throw new ArgumentException($"Invalid Switchovers value: {_switchover}");
        }
    }

    #endregion

    #region Dialogs Events
    private void AddListener() {
        SettingsDialog.BackClicked += OnSettingsDialogBackClicked;
        SettingsDialog.ResetClicked += OnSettingsDialogResetClicked;

        AboutDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;
        LevelSelectionDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed += ShowSettings;
        MainMenuDialog.LevelSelectDialogShowed += ShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed += ShowAboutDialog;

        LevelSelectionDialog.LevelStarted += OnLevelStarted;

        GameDialog.PlayClicked += StartGameplay;
        GameDialog.ResetClicked += OnResetClicked;
        GameDialog.MainMenuClicked += OnMainMenuClicked;
        GameDialog.PauseClicked += OnPauseClicked;
        GameDialog.LearningClicked += OnLearningClicked;
        GameDialog.SettingsClicked += OnSettingsClicked;
        GameDialog.NextLevelClicked += OnNextLevelClicked;

        _spawner.SheepCreated += OnSheepCreated;
        _sheepCounter.SheepIsOver += OnSheepIsOver;
        _qTESystem.EventFinished += OnQTESystemEventFinished;

        _adManager.FullscreenClosed += OnFullscreenClosed;
        _adManager.RewardedClosed += OnRewardedClosed;
        _adManager.RewardedReward += OnRewardedReward;

    }

    private void RemoveLisener() {
        SettingsDialog.BackClicked -= OnSettingsDialogBackClicked;
        SettingsDialog.ResetClicked -= OnSettingsDialogResetClicked;

        AboutDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;
        LevelSelectionDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed -= ShowSettings;
        MainMenuDialog.LevelSelectDialogShowed -= ShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed -= ShowAboutDialog;

        LevelSelectionDialog.LevelStarted -= OnLevelStarted;

        GameDialog.PlayClicked -= StartGameplay;
        GameDialog.ResetClicked -= OnResetClicked;
        GameDialog.MainMenuClicked -= OnMainMenuClicked;
        GameDialog.NextLevelClicked -= OnNextLevelClicked;

        GameDialog.PauseClicked -= OnPauseClicked;
        GameDialog.LearningClicked -= OnLearningClicked;
        GameDialog.SettingsClicked -= OnSettingsClicked;

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
        _currentSheep.Init(_qTESystem, _pauseHandler);

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

    private void OnLevelStarted(LevelConfig config) {
        if (_currentLevelConfig != config)
            _currentLevelConfig = config;

        _score.SetLevelConfig(_currentLevelConfig);
        _sheepCounter.SetLevelConfig(_currentLevelConfig);
        _qTESystem.SetLevelConfig(_currentLevelConfig);

        StartGameplay();
    }

    private void OnQTESystemEventFinished(bool result) => _score.SetSwipeResult(result);

    private void OnPauseClicked(bool value) => SetPause(value);

    private void OnLearningClicked() => GameDialog.GetPanelByType<LearningPanel>().Show(true);

    private void OnSettingsClicked() => ShowSettings();

    private void OnSettingsDialogBackClicked() {
        if (_pauseHandler.IsPaused) {
            SetPause(false);
            SettingsDialog.Show(false);

            ShowGameplayDialog();
        }
        else
            _dialogSwitcher.ShowPreviousDialog();
    }

    private void OnMainMenuClicked() => FinishGameplay(Switchovers.MainMenu);

    private void OnResetClicked() => FinishGameplay(Switchovers.CurrentLevel);

    private void OnNextLevelClicked() => FinishGameplay(Switchovers.NextLevel);

    private void OnSettingsDialogResetClicked() {
        if (_adManager.Platform == GamePush.Platform.YANDEX)
            ResetCloudPlayerProgress();

        ResetLocalPlayerProgress();
    }

    private void StartGameplay() {
        ShowGameplayDialog();

        _environmentSound.PlaySound(MusicType.Gameplay);
        _spawner.CreateSheep(_currentLevelConfig.Color);
    }

    private void FinishGameplay(Switchovers switchover) {
        _switchover = switchover;

        if (_score.StarsCount > 0) {
            UnlockLevel();

            _currentLevelConfig.Progress.SetStatus(LevelStatusTypes.Complited);

            var sratsCount = _score.StarsCount;
            if (_score.StarsCount > _currentLevelConfig.Progress.StarsCount) {
                _currentLevelConfig.Progress.SetStarsCount(sratsCount);
                SaveProgress();
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

    private void SaveProgress() {
        List<LevelProgressData> data = new List<LevelProgressData>();

        foreach (var iLevelConfig in _configs.Configs) {
            LevelProgressData progress = iLevelConfig.Progress;
            data.Add(progress);
        }

        PlayerData playerData = new PlayerData(data);
        _progressLoader.SaveLevelProgress(playerData);
    }

    private void UnlockLevel() {
        int index = _configs.Configs.IndexOf(_currentLevelConfig) + 1;

        if (index > _configs.Configs.Count) {
            ShowMainMenuDialog();
        }
        else {
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

    public void Dispose() {
        RemoveLisener();
    }
}
