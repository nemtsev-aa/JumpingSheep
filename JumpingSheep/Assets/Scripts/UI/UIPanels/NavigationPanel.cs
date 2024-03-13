using System;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPanel : UIPanel {
    public event Action LearningButtonClicked;
    public event Action PauseButtonClicked;

    [SerializeField] private Button _learningButton;
    [SerializeField] private Button _pauseButton;

    public void Init() {
        AddListeners();
    }

    public override void Reset() {
        base.Reset();

        Show(true);
    }

    public override void AddListeners() {
        base.AddListeners();

        _learningButton.onClick.AddListener(LearningButtonClick);
        _pauseButton.onClick.AddListener(PauseButtonClick);
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _learningButton.onClick.RemoveListener(LearningButtonClick);
        _pauseButton.onClick.RemoveListener(PauseButtonClick);
    }

    private void LearningButtonClick() => LearningButtonClicked?.Invoke();

    private void PauseButtonClick() => PauseButtonClicked?.Invoke();
}
