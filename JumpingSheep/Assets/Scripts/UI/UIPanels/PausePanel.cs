using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PausePanel : UIPanel {
    public event Action PlayButtonClicked;
    public event Action MainMenuButtonClicked;

    [SerializeField] private Image _sheepLeftIcon;
    [SerializeField] private Image _sheepRightIcon;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _mainMenuButton;

    private Sequence _mySequence;

    public void Init() {
        AddListeners();
    }

    public override void Show(bool value) {
        base.Show(value);

        ShowAnimation(value);
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
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);
    }

    private void MainMenuButtonClick() => MainMenuButtonClicked?.Invoke();

    private void PlayButtonClick() => PlayButtonClicked?.Invoke();

    private void ShowAnimation(bool value) {

        if (value == false && _mySequence != null) {
            _mySequence.Kill();
            _mySequence = null;
        }

        var t1 = _sheepLeftIcon.rectTransform.DOScale(Vector3.one * 1.2f, 1f);
        var t2 = _sheepLeftIcon.rectTransform.DOScale(Vector3.one, 1f);

        var t3 = _sheepRightIcon.rectTransform.DOScale(Vector3.one * 1.2f, 1f);
        var t4 = _sheepRightIcon.rectTransform.DOScale(Vector3.one, 1f);

        _mySequence = DOTween.Sequence();
        _mySequence.Append(t1)
            .Insert(1f, t2)
            .Insert(2f, t3)
            .Insert(3f, t4)
            .SetLoops(-1);
    }
}
