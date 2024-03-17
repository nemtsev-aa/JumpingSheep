using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QTEEventView : UICompanent, IPause, IDisposable {
    private const int LoopCount = 7;

    [SerializeField] private Sprite _trueResult;
    [SerializeField] private Sprite _falseResult;

    [Space(5)]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillerImage;
    [SerializeField] private TextMeshProUGUI _labelText;
    [SerializeField] private Image _iconImage;

    private QTEEvent _qteEvent;
    private PauseHandler _pauseHandler;

    private float TimeToSwipe => _qteEvent.Config.TimeToSwipe;
    private float _time;

    private Tween _startTween;
    private Sequence _finishSequence;

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
        if (_isPaused || _qteEvent.CurrentState == QTEEventState.Created)
            return;

        if (_qteEvent.CurrentState == QTEEventState.Started) {
            _time += Time.deltaTime;

            if (_time <= TimeToSwipe)
                _fillerImage.fillAmount = (TimeToSwipe - _time) / TimeToSwipe;
        }
    }

    private void AddListener() {
        _qteEvent.StateChanged += OnStateChanged;
        _qteEvent.Disabled += OnDisabled;
    }

    private void RemoveLisener() {
        _qteEvent.StateChanged -= OnStateChanged;
        _qteEvent.Disabled -= OnDisabled;
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
        _startTween = transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.InOutSine).SetLoops(LoopCount, LoopType.Yoyo);
    }

    private void ShowTrueFinishedAnimation() => FinishedAnimation(true);
    
    private void ShowFallFinishedAnimation() => FinishedAnimation(false);
    
    private void FinishedAnimation(bool status) {
         _startTween.Kill();

        Sprite resultSprite = status ? _trueResult : _falseResult;
        _iconImage.sprite = resultSprite;

        Color _backgroundColor = status ? Color.green : Color.red;

        _finishSequence = DOTween.Sequence();
        _finishSequence.Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutSine))
            .Insert(0, _backgroundImage.DOColor(_backgroundColor, 0.3f))
            .Play();
    }

    private void OnDisabled() {
        RemoveLisener();

        _finishSequence.Kill();
    }

    public override void Dispose() {
        base.Dispose();

        RemoveLisener();

        _qteEvent = null;
        _pauseHandler.Remove(this);
    }

}
