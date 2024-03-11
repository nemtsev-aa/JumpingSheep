using System;
using UnityEngine;

public class GameplayMediator : IDisposable {
    private readonly SheepSpawner _spawner;
    private SheepQuantityCounter _sheepCounter;
    private readonly UIManager _uIManager;
    private readonly DialogSwitcher _dialogSwitcher;
    private readonly EnvironmentSoundManager _environmentSound;

    private readonly QTESystem _qTESystem;
    private Sheep _currentSheep;
    private bool _sheepOver;

    public GameplayMediator(SheepSpawner spawner, UIManager uIManager, EnvironmentSoundManager environmentSound) {
        _spawner = spawner;

        _sheepCounter = new SheepQuantityCounter();

        _uIManager = uIManager;
        _environmentSound = environmentSound;

        _dialogSwitcher = uIManager.DialogSwitcher;
        _qTESystem = GameDialog.QTESystem;
    }

    private MainMenuDialog MainMenuDialog => _uIManager.MainMenuDialog;
    private LevelSelectionDialog LevelSelectionDialog => _uIManager.LevelSelectionDialog;
    private GameDialog GameDialog => _uIManager.GameDialog;
    private SettingsDialog SettingsDialog => _uIManager.SettingsDialog;
    private AboutDialog AboutDialog => _uIManager.AboutDialog;

    public void Init() {
        AddListener();

        ShowMainMenuDialog();
    }

    public void StartGameplay() {
        _sheepCounter.Reset();
        _environmentSound.PlaySound(MusicType.Gameplay);
        _spawner.CreateSheep();
    }

    #region Switching Dialogs

    public void ShowMainMenuDialog() {
        _environmentSound.PlaySound(MusicType.UI);
        _dialogSwitcher.ShowDialog(DialogTypes.MainMenu);
    }

    public void ShowGameplayDialog(LevelConfig config) {
        _sheepCounter.SetMaxCount(config.SheepCount);
        
        GameDialog.SetSheepCounter(_sheepCounter);

        _environmentSound.PlaySound(MusicType.Gameplay);
        _dialogSwitcher.ShowDialog(DialogTypes.Game);
    }

    public void ShowSettings() => _dialogSwitcher.ShowDialog(DialogTypes.Settings);

    public void ShowAboutDialog() => _dialogSwitcher.ShowDialog(DialogTypes.About);

    public void ShowLevelSelectionDialog() => _dialogSwitcher.ShowDialog(DialogTypes.LevelSelection);

    #endregion

    private void AddListener() {
        SettingsDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;
        AboutDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed += ShowSettings;
        MainMenuDialog.LevelSelectDialogShowed += ShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed += ShowAboutDialog;

        LevelSelectionDialog.LevelStarted += ShowGameplayDialog;

        GameDialog.PlayClicked += StartGameplay;
        GameDialog.ResetClicked += OnResetClicked;
        GameDialog.MainMenuClicked += OnMainMenuClicked;

        _spawner.SheepCreated += OnSheepCreated;
        _sheepCounter.SheepIsOver += OnSheepIsOver;
    }

    private void RemoveLisener() {
        SettingsDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;
        AboutDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed -= ShowSettings;
        MainMenuDialog.LevelSelectDialogShowed -= ShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed -= ShowAboutDialog;

        LevelSelectionDialog.LevelStarted -= ShowGameplayDialog;

        GameDialog.PlayClicked -= StartGameplay;
        GameDialog.ResetClicked -= OnResetClicked;
        GameDialog.MainMenuClicked -= OnMainMenuClicked;

        _spawner.SheepCreated -= OnSheepCreated;
        _sheepCounter.SheepIsOver -= OnSheepIsOver;
    }


    private void OnSheepCreated(Sheep sheep) {
        _currentSheep = sheep;
        _currentSheep.Init(_qTESystem);

        _currentSheep.EventsHandler.Striked += OnSheepStriked;
        _currentSheep.EventsHandler.Jumped += OnSheepJumped;
    }

    private void OnSheepStriked() {
        _sheepCounter.AddStrike();

        _spawner.DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep();
    }

    private void OnSheepJumped() {
        _sheepCounter.AddJump();
        _spawner.DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep();
    }

    private void OnSheepIsOver() {
        _sheepOver = true;
        _environmentSound.PlaySound(MusicType.GameOver);
    }

    private void OnResetClicked() {
        _sheepCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;

        StartGameplay();
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
