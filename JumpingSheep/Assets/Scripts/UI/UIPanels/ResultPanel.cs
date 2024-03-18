using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultPanel : UIPanel {
    public const string ResultLabelText = "Ñ÷¸ò";

    public event Action MainMenuButtonClicked;
    public event Action ResetButtonClicked;
    public event Action NextLevelClicked;

    [SerializeField] private Transform _starIconsParent;
    [SerializeField] private TextMeshProUGUI _resultLabel;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Space(10), Header("Buttons")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _nextLevelButton;

    private Score _score;
    private Sequence _mySequence;

    public void Init(Score score) {
        _score = score;

        AddListeners();
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value == true)
            ShowAnimation();
    }

    public override void Reset() {
        Show(false);

        _scoreText.text = "";
    }

    public override void AddListeners() {
        base.AddListeners();

        _score.ScoreChanged += OnScoreChanged;

        _resetButton.onClick.AddListener(ResetButtonClick);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);
        _nextLevelButton.onClick.AddListener(NextLevelClick);
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _score.ScoreChanged -= OnScoreChanged;

        _resetButton.onClick.RemoveListener(ResetButtonClick);
        _mainMenuButton.onClick.RemoveListener(MainMenuButtonClick);
        _nextLevelButton.onClick.RemoveListener(NextLevelClick);
    }

    private void OnScoreChanged() {
        _resultLabel.text = ResultLabelText;
        _scoreText.text = _score.Result;
    }

    private void MainMenuButtonClick() => MainMenuButtonClicked?.Invoke();

    private void ResetButtonClick() => ResetButtonClicked?.Invoke();

    private void NextLevelClick() => NextLevelClicked?.Invoke();

    private void ShowAnimation() {
        for (int i = 0; i < _starIconsParent.childCount; i++) {
            Transform star = _starIconsParent.GetChild(i);
            star.transform.localScale = Vector3.zero;
        }

        _mySequence = DOTween.Sequence();
        for (int i = 0; i < _score.StarsCount; i++) {
            Tween tween = _starIconsParent.GetChild(i).DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            _mySequence.Append(tween);
        }

        _mySequence.Play();
    }
}
