using System;
using UnityEngine;
using Zenject;

public class GameplayMediator : MonoBehaviour, IPause, IDisposable {
    private PauseHandler _pauseHandler;
    private SheepSpawner _spawner;
    private SheepQuantityCounter _sheepCounter;
    private UIManager _uIManager;
    private DialogSwitcher _dialogSwitcher;
    private EnvironmentSoundManager _environmentSound;

    private LevelConfig _currentLevelConfig;
    private QTESystem _qTESystem;
    private Score _score;
    private Sheep _currentSheep;
    private bool _sheepOver;

    private MainMenuDialog MainMenuDialog => _uIManager.MainMenuDialog;
    private LevelSelectionDialog LevelSelectionDialog => _uIManager.LevelSelectionDialog;
    private GameDialog GameDialog => _uIManager.GameDialog;
    private SettingsDialog SettingsDialog => _uIManager.SettingsDialog;
    private AboutDialog AboutDialog => _uIManager.AboutDialog;

    [Inject]
    public void Construct(PauseHandler pauseHandler, SheepSpawner spawner, QTESystem qTESystem, Score score, SheepQuantityCounter sheepCounter) {
        _pauseHandler = pauseHandler;
        _spawner = spawner;
        _qTESystem = qTESystem;
        _score = score;
        _sheepCounter = sheepCounter;
    }

    public void Init(UIManager uIManager, EnvironmentSoundManager environmentSound) {
        _uIManager = uIManager;
        _environmentSound = environmentSound;

        _dialogSwitcher = uIManager.DialogSwitcher;
        AddListener();
        ShowMainMenuDialog();
    }

    public void SetPause(bool isPaused) => _pauseHandler.SetPause(isPaused);


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

    #endregion

    #region Dialogs Events
    private void AddListener() {
        SettingsDialog.BackClicked += OnSettingsDialogBackClicked;
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
        GameDialog.SettingsClicked += OnSettingsClicked;

        _spawner.SheepCreated += OnSheepCreated;
        _sheepCounter.SheepIsOver += OnSheepIsOver;
        _qTESystem.EventFinished += OnQTESystemEventFinished;
    }

    private void RemoveLisener() {
        SettingsDialog.BackClicked -= OnSettingsDialogBackClicked;
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
        _qTESystem.EventFinished -= OnQTESystemEventFinished;
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

    private void OnLevelStarted(LevelConfig config) {
        if (_currentLevelConfig != config)
            _currentLevelConfig = config;

        _score.SetLevelConfig(_currentLevelConfig);
        _sheepCounter.SetLevelConfig(_currentLevelConfig);
        _qTESystem.SetLevelConfig(_currentLevelConfig);

        StartGameplay();
    }

    private void OnQTESystemEventFinished(bool result) {
        _score.SetSwipeResult(result);
    }

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

    private void OnResetClicked() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;

        OnLevelStarted(_currentLevelConfig);
    }

    private void StartGameplay() {
        ShowGameplayDialog();

        _environmentSound.PlaySound(MusicType.Gameplay);
        _spawner.CreateSheep(_currentLevelConfig.Color);
    }

    private void OnMainMenuClicked() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;


        ShowMainMenuDialog();
    }

    private void SetStarsCount() {
        _currentLevelConfig.StarsCount = _score.StarsCount;
        _currentLevelConfig.Status = LevelStatusTypes.Complited;
    }

    public void Dispose() {
        RemoveLisener();
    }
}
