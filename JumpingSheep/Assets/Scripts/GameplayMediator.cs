using System;
using UnityEngine;

public class GameplayMediator : IDisposable {
    private readonly SheepSpawner _spawner;
    private readonly SheepQuantityCounter _scoreCounter;
    private readonly UIManager _uIManager;
    private readonly DialogSwitcher _dialogSwitcher;

    private readonly QTESystem _qTESystem;
    private Sheep _currentSheep;
    private bool _sheepOver;

    public GameplayMediator(SheepSpawner spawner, SheepQuantityCounter scoreCounter, UIManager uIManager) {
        _spawner = spawner;
        _scoreCounter = scoreCounter;
        _uIManager = uIManager;

        _dialogSwitcher = uIManager.DialogSwitcher;
        _qTESystem = GameDialog.QTESystem;
    }

    private MainMenuDialog MainMenuDialog => _uIManager.MainMenuDialog;
    private GameDialog GameDialog => _uIManager.GameDialog;
    private SettingsDialog SettingsDialog => _uIManager.SettingsDialog;
    private AboutDialog AboutDialog => _uIManager.AboutDialog;

    public void Init() {
        AddListener();
    }

    public void StartGameplay() {
        _spawner.CreateSheep();
    }

    #region Switching Dialogs

    public void ShowMainMenuDialog() => _dialogSwitcher.ShowDialog(DialogTypes.MainMenu);
    public void ShowGameplayDialog() => _dialogSwitcher.ShowDialog(DialogTypes.Game);
    public void ShowSettings() => _dialogSwitcher.ShowDialog(DialogTypes.Settings);
    public void ShowAboutDialog() => _dialogSwitcher.ShowDialog(DialogTypes.About);

    #endregion

    private void AddListener() {
        SettingsDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;
        AboutDialog.BackClicked += _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed += ShowSettings;
        MainMenuDialog.GameplayDialogShowed += ShowGameplayDialog;
        MainMenuDialog.AboutDialogShowed += ShowAboutDialog;

        GameDialog.PlayClicked += StartGameplay;
        GameDialog.ResetClicked += OnResetClicked;
        GameDialog.MainMenuClicked += OnMainMenuClicked;

        _spawner.SheepCreated += OnSheepCreated;
        _scoreCounter.SheepIsOver += OnSheepIsOver;
    }

    private void RemoveLisener() {
        SettingsDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;
        AboutDialog.BackClicked -= _dialogSwitcher.ShowPreviousDialog;

        MainMenuDialog.SettingsDialogShowed -= ShowSettings;
        MainMenuDialog.GameplayDialogShowed -= ShowGameplayDialog;
        MainMenuDialog.AboutDialogShowed -= ShowAboutDialog;
 
        GameDialog.PlayClicked -= StartGameplay;
        GameDialog.ResetClicked -= OnResetClicked;
        GameDialog.MainMenuClicked -= OnMainMenuClicked;

        _spawner.SheepCreated -= OnSheepCreated;
        _scoreCounter.SheepIsOver -= OnSheepIsOver;
    }

    private void OnSheepCreated(Sheep sheep) {
        _currentSheep = sheep;
        _currentSheep.Init(_qTESystem);

        _currentSheep.EventsHandler.Striked += OnSheepStriked;
        _currentSheep.EventsHandler.Jumped += OnSheepJumped;
    }

    private void OnSheepStriked() {
        _scoreCounter.AddStrike();

        _spawner.DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep();
    }

    private void OnSheepJumped() {
        _scoreCounter.AddJump();
        _spawner.DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep();
    }

    private void OnSheepIsOver() {
        _sheepOver = true;
    }

    private void OnResetClicked() {
        _scoreCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;

        StartGameplay();
    }

    private void OnMainMenuClicked() {
        _scoreCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;


        ShowMainMenuDialog();
    }

    public void Dispose() {
        RemoveLisener();
    }
}
