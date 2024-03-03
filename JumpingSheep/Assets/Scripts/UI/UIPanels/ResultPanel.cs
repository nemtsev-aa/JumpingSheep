using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : UIPanel {
    public event Action MainMenuClick;
    public event Action ResetClicked;

    [SerializeField] private TextMeshProUGUI _resultLabel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _mainMenuButton;
    
    private SheepQuantityCounter _counter;

    public void Init(SheepQuantityCounter counter) {
        _counter = counter;

        AddListeners();
    }

    public override void Reset() {
        Show(false);

        _scoreText.text = "";
    }

    public override void AddListeners() {
        base.AddListeners();

        _counter.SheepIsOver += OnSheepIsOver;
        _resetButton.onClick.AddListener(ResetButtonClick);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _counter.SheepIsOver -= OnSheepIsOver;
        _resetButton.onClick.RemoveListener(ResetButtonClick);
        _mainMenuButton.onClick.RemoveListener(MainMenuButtonClick);
    }

    private void OnSheepIsOver() {
        _resultLabel.text = "Ñ÷¸ò";
        _scoreText.text = _counter.Result;
    }

    private void MainMenuButtonClick() => MainMenuClick?.Invoke();

    private void ResetButtonClick() => ResetClicked?.Invoke();

}
