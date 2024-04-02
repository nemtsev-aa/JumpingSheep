using TMPro;
using DG.Tweening;
using UnityEngine;


public class LoadingPlayerProgressPanel : UIPanel {
    private const string LoadingText = "Загрузка прогресса...";
    private const float FadeDuration = 0.3f;

    [SerializeField] private TextMeshProUGUI _loadingLabel;

    public void Init() {
        _loadingLabel.text = LoadingText;
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value)
            StartAnimation();
    }

    private void StartAnimation() {
        _loadingLabel.DOFade(0f, FadeDuration).SetEase(Ease.InElastic).Loops();
    }
}
