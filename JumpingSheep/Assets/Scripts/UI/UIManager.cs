using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using System;

public class UIManager : MonoBehaviour, IDisposable {
    private DialogFactory _factory;
    private List<Dialog> _dialogs;

    private GameplayMediator _gameplayMediator;

    public DialogSwitcher DialogSwitcher { get; private set; }

    [Inject]
    public void Constuct(DialogFactory factory) {
        _factory = factory;
        _factory.SetDialogsParent(GetComponent<RectTransform>());
        _dialogs = new List<Dialog>();
    }

    public void Init(GameplayMediator gameplayMediator) {
        _gameplayMediator = gameplayMediator;

        DialogSwitcher = new DialogSwitcher(this);

        AddListener();
    }

    public void ShowMainMenuDialog() => DialogSwitcher.ShowDialog<MainMenuDialog>();

    #region Creating Dialogs

    public T GetDialogByType<T>() where T : Dialog {
        var dialog = GetDialogFromList<T>();

        if (dialog != null)
            return dialog;
        else
            return CreateNewDialog<T>();
    }

    private T GetDialogFromList<T>() where T : Dialog {
        if (_dialogs.Count == 0)
            return null;

        return (T)_dialogs.FirstOrDefault(dialog => dialog is T);
    }

    private T CreateNewDialog<T>() where T : Dialog {
        var dialog = _factory.GetDialog<T>();
        dialog.Init();

        _dialogs.Add(dialog);

        return dialog;
    }

    #endregion

    #region Dialogs Events
    private void AddListener() {
        SettingsDialog.BackClicked += OnSettingsDialogBackClicked;
        SettingsDialog.ResetClicked += OnSettingsDialogResetClicked;

        AboutDialog.BackClicked += OnShowMainMenuDialog;
        LevelSelectionDialog.BackClicked += OnShowMainMenuDialog;

        MainMenuDialog.SettingsDialogShowed += OnShowSettingsDialog;
        MainMenuDialog.LevelSelectDialogShowed += OnShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed += OnShowAboutDialog;

        LevelSelectionDialog.LevelStarted += OnLevelStarted;

        GameDialog.ResetClicked += OnGameDialogResetClicked;
        GameDialog.MainMenuClicked += OnMainMenuClicked;
        GameDialog.PauseClicked += OnPauseClicked;
        GameDialog.LearningClicked += OnLearningClicked;
        GameDialog.NextLevelClicked += OnGameDialogNextLevelClicked;
    }

    private void RemoveLisener() {
        SettingsDialog.BackClicked -= OnSettingsDialogBackClicked;
        SettingsDialog.ResetClicked -= OnSettingsDialogResetClicked;

        AboutDialog.BackClicked -= OnShowMainMenuDialog;
        LevelSelectionDialog.BackClicked -= OnShowMainMenuDialog;

        MainMenuDialog.SettingsDialogShowed -= OnShowSettingsDialog;
        MainMenuDialog.LevelSelectDialogShowed -= OnShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed -= OnShowAboutDialog;

        LevelSelectionDialog.LevelStarted -= OnLevelStarted;

        GameDialog.ResetClicked -= OnGameDialogResetClicked;
        GameDialog.MainMenuClicked -= OnMainMenuClicked;
        GameDialog.NextLevelClicked -= OnGameDialogNextLevelClicked;
        GameDialog.PauseClicked -= OnPauseClicked;
        GameDialog.LearningClicked -= OnLearningClicked;
    }
    
    #endregion

    private void OnShowMainMenuDialog() => DialogSwitcher.ShowDialog<MainMenuDialog>();

    private void OnShowSettingsDialog()  => DialogSwitcher.ShowDialog<SettingsDialog>();

    private void OnShowAboutDialog() => DialogSwitcher.ShowDialog<AboutDialog>();
   
    private void OnShowLevelSelectionDialog() => DialogSwitcher.ShowDialog<LevelSelectionDialog>();
    
    private void OnShowGameplayDialog() => DialogSwitcher.ShowDialog<GameDialog>();
    
    private void OnSettingsDialogBackClicked() => OnShowMainMenuDialog();

    private void OnSettingsDialogResetClicked() => _gameplayMediator.ResetPlayerProgress();

    private void OnLevelStarted(LevelConfig config) {
        _gameplayMediator.LevelPreparation(config);

        OnShowGameplayDialog();
    }

    #region GameDialog Events

    private void OnLearningClicked(bool value) => _gameplayMediator.SetPause(value);
    
    private void OnPauseClicked(bool value) => _gameplayMediator.SetPause(value);

    private void OnMainMenuClicked() {
        OnShowMainMenuDialog();

        _gameplayMediator.FinishGameplay(Switchovers.MainMenu);
    }

    private void OnGameDialogResetClicked() =>
        _gameplayMediator.FinishGameplay(Switchovers.CurrentLevel);


    private void OnGameDialogNextLevelClicked() {
        _gameplayMediator.FinishGameplay(Switchovers.NextLevel);
        OnShowGameplayDialog();
    }
        
    #endregion

 
    public void Dispose() {
        RemoveLisener();
    }

}
