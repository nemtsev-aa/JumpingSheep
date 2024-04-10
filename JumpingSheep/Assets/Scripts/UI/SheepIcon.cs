using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SheepIcon : UICompanent {
    private const float _animationDuration = 0.3f;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;

    private Tween _fadeTween;
    private float _fadeValue;

    public override void Show(bool value) {
        _fadeValue = value ? 1f : 0f;

        StartAnimation();
    }

    public void StopAnimation() {
        _fadeTween.Kill();
    }

    private void StartAnimation() =>
        _fadeTween = _iconImage.DOFade(_fadeValue, _animationDuration);
}
