using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour, IDisposable {
    public event Action ResetClicked;

    [SerializeField] private TextMeshProUGUI _resultLabel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button _resetButton;

    public void Init(string score) {
        _resultLabel.text = "Результат";
        _scoreText.text = score;
        AddListener();
    }

    private void AddListener() {
        _resetButton.onClick.AddListener(ResetButtonClick);
    }

    private void RemoveLisener() {
        _resetButton.onClick.RemoveListener(ResetButtonClick);
    }

    private void ResetButtonClick() {
        ResetClicked?.Invoke();
    }

    public void Dispose() {
        RemoveLisener();
    }
}
