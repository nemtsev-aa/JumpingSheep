using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QTEEventView : UICompanent, IPause, IDisposable {
    private const int LoopCount = 7;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillerImage;
    [SerializeField] private TextMeshProUGUI _labelText;
    [SerializeField] private Image _iconImage;

    private QTEEvent _qteEvent;
    private PauseHandler _pauseHandler;

    private float TimeToSwipe => _qteEvent.Config.TimeToSwipe;
    private float _time;

    private Sequence _sequence;
    private Guid _uid;
    private bool _isPaused;

    public void Init(QTEEvent qteEvent, PauseHandler pauseHandler) {
        _qteEvent = qteEvent;
        
        _pauseHandler = pauseHandler;
        _pauseHandler.Add(this);

        _iconImage.sprite = _qteEvent.Config.SwipeDirectionIcon;

        AddListener();
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;

    private void Update() {
        if (_isPaused)
            return;

        if (_qteEvent != null && _qteEvent.CurrentState == QTEEventState.Started) {
            _time += Time.deltaTime;

            if (_time <= TimeToSwipe)
                _fillerImage.fillAmount = (TimeToSwipe - _time) / TimeToSwipe;
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

    public override void Dispose() {
        base.Dispose();

        RemoveLisener();
        _pauseHandler.Remove(this);
    }

}
