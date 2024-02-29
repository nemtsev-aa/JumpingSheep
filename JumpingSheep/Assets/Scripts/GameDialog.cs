using System;
using TMPro;
using UnityEngine;

public class GameDialog : MonoBehaviour, IDisposable {
    public event Action ResetClicked;

    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _strikeCount;
    [SerializeField] private TextMeshProUGUI _jumpCount;
    [SerializeField] private ResultPanel _resultPanel;
    [SerializeField] private QTESystem _qTESystem;

    private GameScoreCounter _counter;
    public QTESystem QTESystem => _qTESystem;

    public void Init(GameScoreCounter counter, QTEEventConfigs qTEEventConfigs) {
        _counter = counter;
        _qTESystem.Init(qTEEventConfigs.Configs);

        AddListener();
        ResetLabelText();

        _resultPanel.Init(_counter.Score);
        _resultPanel.gameObject.SetActive(false);       
    }

    private void AddListener() {
        _counter.JumpCountChanged += OnJumpCountAction;
        _counter.StrikeCountChanged += OnStrikeCountChanged;
        _counter.ScoreChanged += OnScoreChanged;
        _counter.SheepIsOver += OnSheepIsOver;

        _resultPanel.ResetClicked += OnResetClicked;
    }

    private void RemoveLisener() {
        _counter.JumpCountChanged -= OnJumpCountAction;
        _counter.StrikeCountChanged -= OnStrikeCountChanged;
        _counter.ScoreChanged -= OnScoreChanged;
        _counter.SheepIsOver -= OnSheepIsOver;

        _resultPanel.ResetClicked -= OnResetClicked;
    }

    private void ResetLabelText() {
        OnScoreChanged(0);
        OnJumpCountAction(0);
        OnStrikeCountChanged(0);
    }

    private void OnScoreChanged(float value) {
        _score.text = $"{value}%";
    }

    private void OnJumpCountAction(int value) {
        _jumpCount.text = $"{value}";
    }

    private void OnStrikeCountChanged(int value) {
        _strikeCount.text = $"{value}";
    }

    private void OnSheepIsOver() {
        _resultPanel.gameObject.SetActive(true);
    }

    private void OnResetClicked() {
        Debug.Log("GameDialog: OnResetClicked");
        _resultPanel.gameObject.SetActive(false);
        ResetLabelText();

        ResetClicked?.Invoke();
    }

    public void Dispose() {
        RemoveLisener();
    }
}
