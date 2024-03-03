using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class InnerGlowPanel : UIPanel {
    private const int LoopCount = 7;

    [SerializeField] private Image _innerGlowImage;

    private Sequence _sequence;
    private Guid _uid;

    public void StartedAnimation() {
        if (_sequence == null) {
            _sequence = DOTween.Sequence();
            _sequence.Append(_innerGlowImage.DOFade(0.5f, 0.3f).SetLoops(LoopCount, LoopType.Yoyo));

            _uid = Guid.NewGuid();
            _sequence.id = _uid;
        }

        _sequence.Play();
    }

    public void FinishedAnimation() {
        DOTween.Kill(_uid);
        _sequence = null;
    }

    public override void Reset() {
        FinishedAnimation();
    }
}
