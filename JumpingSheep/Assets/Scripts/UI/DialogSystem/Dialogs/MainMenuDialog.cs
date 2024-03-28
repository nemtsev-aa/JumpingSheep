using System;
using UnityEngine;

public class MainMenuDialog : Dialog {
    public event Action LevelSelectDialogShowed;
    public event Action SettingsDialogShowed;
    public event Action AboutDialogShowed;

    private MenuCategoryPanel _category;

    public override void Init() {
        base.Init();
    }

    public override void InitializationPanels() {
        _category = GetPanelByType<MenuCategoryPanel>();
        _category.Init();
    }

    public override void AddListeners() {
        base.AddListeners();

        _category.GameplayDialogSelected += OnGameplayDialogSelected;
        _category.SettingsDialogSelected += OnSettingsDialogSelected;
        _category.AboutDialogSelected += OnAboutDialogSelected;
        _category.QuitButtonSelected += OnQuitButtonSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _category.GameplayDialogSelected -= OnGameplayDialogSelected;
        _category.SettingsDialogSelected -= OnSettingsDialogSelected;
        _category.AboutDialogSelected -= OnAboutDialogSelected;
        _category.QuitButtonSelected -= OnQuitButtonSelected;
    }

    private void OnGameplayDialogSelected() => LevelSelectDialogShowed?.Invoke();

    private void OnSettingsDialogSelected() => SettingsDialogShowed?.Invoke();

    private void OnAboutDialogSelected() => AboutDialogShowed?.Invoke();

    private void OnQuitButtonSelected() => Application.Quit();

}
