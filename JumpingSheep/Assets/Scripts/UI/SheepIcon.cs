using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SheepIcon : MonoBehaviour {
    private const float _animationDuration = 0.3f;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    private float _fadeValue;

    public void Show(bool value) {
        _fadeValue = value ? 1f : 0f;

        StartAnimation();
    }

    private void StartAnimation() => _iconImage.DOFade(_fadeValue, _animationDuration);
}
