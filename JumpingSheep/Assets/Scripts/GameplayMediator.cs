using System;
using UnityEngine;

public class GameplayMediator : MonoBehaviour, IPause, IDisposable {
    private PauseHandler _pauseHandler;
    private SheepSpawner _spawner;
    private SheepQuantityCounter _sheepCounter;
    private UIManager _uIManager;
    private DialogSwitcher _dialogSwitcher;
    private EnvironmentSoundManager _environmentSound;

    private LevelConfig _currentLevelConfig;
    private QTESystem _qTESystem;
    private Sheep _currentSheep;
    private bool _sheepOver;
    private bool _isPaused;

    private MainMenuDialog MainMenuDialog => _uIManager.MainMenuDialog;
    private LevelSelectionDialog LevelSelectionDialog => _uIManager.LevelSelectionDialog;
    private GameDialog GameDialog => _uIManager.GameDialog;
    private SettingsDialog SettingsDialog => _uIManager.SettingsDialog;
    private AboutDialog AboutDialog => _uIManager.AboutDialog;
    
    public void Init(PauseHandler pauseHandler, SheepSpawner spawner, QTESystem qTESystem, UIManager uIManager, EnvironmentSoundManager environmentSound) {
        _pauseHandler = pauseHandler;
        _spawner = spawner;
        _qTESystem = qTESystem;

        _uIManager = uIManager;
        _environmentSound = environmentSound;

        _sheepCounter = new SheepQuantityCounter();
        _dialogSwitcher = uIManager.DialogSwitcher;

        AddListener();
        ShowMainMenuDialog();
    }

    public void SetPause(bool isPaused) {
        _pauseHandler.SetPause(isPaused);

        if (_currentSheep != null)
            _currentSheep.SetPause(isPaused);
    }

    #region Switching Dialogs

    private void ShowMainMenuDialog() {
        _environmentSound.PlaySound(MusicType.UI);
        _dialogSwitcher.ShowDialog(DialogTypes.MainMenu);
    }

    private void ShowGameplayDialog() {
        GameDialog.GetPanelByType<LearningPanel>().Show(false);

        _environmentSound.PlaySound(MusicType.Gameplay);
        _dialogSwitcher.ShowDialog(DialogTypes.Game);
    }

    private void ShowSettings() => _dialogSwitcher.ShowDialog(DialogTypes.Settings);

    private void ShowAboutDialog() => _dialogSwitcher.ShowDialog(DialogTypes.About);

    private void ShowLevelSelectionDialog() => _dialogSwitcher.ShowDialog(DialogTypes.LevelSelection);

    #endregion

    #region Dialogs Events
    private void AddListener() {
        SettingsDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;
        AboutDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed += ShowSettings;
        MainMenuDialog.LevelSelectDialogShowed += ShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed += ShowAboutDialog;

        LevelSelectionDialog.LevelStarted += OnLevelStarted;

        GameDialog.PlayClicked += StartGameplay;
        GameDialog.ResetClicked += OnResetClicked;
        GameDialog.MainMenuClicked += OnMainMenuClicked;
        GameDialog.PauseClicked += OnPauseClicked;
        GameDialog.LearningClicked += OnLearningClicked;
        GameDialog.SettingsClicked -= OnSettingsClicked;

        _spawner.SheepCreated += OnSheepCreated;
        _sheepCounter.SheepIsOver += OnSheepIsOver;
    }

    private void RemoveLisener() {
        SettingsDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;
        AboutDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed -= ShowSettings;
        MainMenuDialog.LevelSelectDialogShowed -= ShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed -= ShowAboutDialog;

        LevelSelectionDialog.LevelStarted -= OnLevelStarted;

        GameDialog.PlayClicked -= StartGameplay;
        GameDialog.ResetClicked -= OnResetClicked;
        GameDialog.MainMenuClicked -= OnMainMenuClicked;
        GameDialog.PauseClicked -= OnPauseClicked;
        GameDialog.LearningClicked -= OnLearningClicked;
        GameDialog.SettingsClicked -= OnSettingsClicked;

        _spawner.SheepCreated -= OnSheepCreated;
        _sheepCounter.SheepIsOver -= OnSheepIsOver;

    }

    #endregion

    #region Sheeps Events

    private void OnSheepCreated(Sheep sheep) {
        _currentSheep = sheep;
        _currentSheep.Init(_qTESystem);

        _currentSheep.EventsHandler.Striked += OnSheepStriked;
        _currentSheep.EventsHandler.Jumped += OnSheepJumped;
    }

    private void OnSheepStriked() {
        _sheepCounter.AddStrike();

        DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep(_currentLevelConfig.Color);
    }

    private void OnSheepJumped() {
        _sheepCounter.AddJump();
        DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep(_currentLevelConfig.Color);
    }

    private void DestroyCurrentSheep() {
        Destroy(_currentSheep.gameObject);
        _currentSheep = null;
    }

    private void OnSheepIsOver() {
        _sheepOver = true;
        _environmentSound.PlaySound(MusicType.GameOver);
    }

    #endregion
    
    private void OnLevelStarted(LevelConfig config) {
        if (_currentLevelConfig != config)
            _currentLevelConfig = config;

        _sheepCounter.SetMaxCount(_currentLevelConfig.SheepCount);
        _qTESystem.SetLevelConfig(_currentLevelConfig);

        GameDialog.SetServices(_sheepCounter, _qTESystem);
        ShowGameplayDialog();
    }

    private void OnPauseClicked(bool value) {
        SetPause(value);
    }

    private void OnLearningClicked() {
        GameDialog.GetPanelByType<LearningPanel>().Show(true);
    }

    private void OnSettingsClicked() {
        ShowSettings();
    }

    private void OnResetClicked() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;

        OnLevelStarted(_currentLevelConfig);
    }

    private void StartGameplay() {
        _sheepCounter.Reset();
        _environmentSound.PlaySound(MusicType.Gameplay);
        _spawner.CreateSheep(_currentLevelConfig.Color);

        ShowGameplayDialog();
    }

    private void OnMainMenuClicked() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;


        ShowMainMenuDialog();
    }

    public void Dispose() {
        RemoveLisener();
    }
}
