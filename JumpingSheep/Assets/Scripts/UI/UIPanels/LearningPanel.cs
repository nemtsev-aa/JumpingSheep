using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearningPanel : UIPanel {
    public event Action PlayButtonClicked;
    public event Action MainMenuButtonClicked;

    [SerializeField] private TextMeshProUGUI _learnText;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _mainMenuButton;
    
    public void Init() {
        AddListeners();
    }

    public override void Reset() {
        base.Reset();

        Show(false);
    }

    public override void AddListeners() {
        base.AddListeners();

        _playButton.onClick.AddListener(PlayButtonClick);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _playButton.onClick.RemoveListener(PlayButtonClick);
        _mainMenuButton.onClick.RemoveListener(MainMenuButtonClick);
    }

    private void MainMenuButtonClick() => MainMenuButtonClicked?.Invoke();
    
    private void PlayButtonClick() => PlayButtonClicked?.Invoke();

}
