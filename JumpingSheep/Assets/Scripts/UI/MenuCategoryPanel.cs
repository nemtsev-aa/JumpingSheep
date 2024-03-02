using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuCategoryPanel : UIPanel {
    public event Action GameplayDialogSelected;
    public event Action SettingsDialogSelected;
    public event Action AboutDialogSelected;
    public event Action QuitButtonSelected;

    [SerializeField] private Button _gameplayButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _quitButton;

    public void Init() {
        AddListeners();
    }

    public override void AddListeners() {
        base.AddListeners();

        _gameplayButton.onClick.AddListener(GameplayButtonClick);
        _settingsButton.onClick.AddListener(SettingsButtonClick);
        _aboutButton.onClick.AddListener(AboutButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    public override void RemoveListeners() {
        _gameplayButton.onClick.RemoveListener(GameplayButtonClick);
        _settingsButton.onClick.RemoveListener(SettingsButtonClick);
        _aboutButton.onClick.RemoveListener(AboutButtonClick);
        _quitButton.onClick.RemoveListener(QuitButtonClick);
    }

    public override void Show(bool value) {
        base.Show(value);

    }

    private void GameplayButtonClick() => GameplayDialogSelected?.Invoke();

    private void SettingsButtonClick() => SettingsDialogSelected?.Invoke();

    private void AboutButtonClick() => AboutDialogSelected?.Invoke();

    private void QuitButtonClick() => QuitButtonSelected?.Invoke();

}
