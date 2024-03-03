using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QTEEventView : MonoBehaviour, IDisposable {
    private const int LoopCount = 7;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillerImage;
    [SerializeField] private TextMeshProUGUI _labelText;
    [SerializeField] private Image _iconImage;

    private QTEEvent _qteEvent;
    private float _eventTime => _qteEvent.Config.KeyTime;
    private float _time;

    private Sequence _sequence;
    private Guid _uid;

    public void Init(QTEEvent qteEvent) {
        _qteEvent = qteEvent;
        //_labelText.text = _qteEvent.Config.KeyText;
        _iconImage.sprite = _qteEvent.Config.KeyIcon;

        AddListener();
    }

    private void Update() {
        if (_qteEvent != null && _qteEvent.enabled == true) {
            _time += Time.deltaTime;

            if (_time <= _eventTime)
                _fillerImage.fillAmount = (_eventTime - _time) / _eventTime;
        }
    }

    private void AddListener() {
        _qteEvent.StateChanged += OnStateChanged;
    }

    private void RemoveLisener() {
        _qteEvent.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(QTEEventState state) {
        switch (state) {
            case QTEEventState.Started:
                ShowStartedAnimation();
                break;

            case QTEEventState.TrueFinished:
                ShowTrueFinishedAnimation();
                break;

            case QTEEventState.FailFinished:
                ShowFallFinishedAnimation();
                break;

            default:
                break;
        }
    }

    private void ShowStartedAnimation() {
        if (_sequence == null) {
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.InOutSine).SetLoops(LoopCount, LoopType.Yoyo)); 
            
            _uid = Guid.NewGuid();
            _sequence.id = _uid;
        }

        _sequence.Play();
    }

    private void ShowTrueFinishedAnimation() => FinishedAnimation(true);
    
    private void ShowFallFinishedAnimation() => FinishedAnimation(false);
    
    private void FinishedAnimation(bool status) {
        Color _backgroundColor = status ? Color.green : Color.red;

        DOTween.Kill(_uid);
        _sequence = null;

        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutSine);
        _backgroundImage.DOColor(_backgroundColor, 0.3f);
        _qteEvent = null;
    }

    public void Dispose() {
        RemoveLisener();
    }
}
