using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using Cysharp.Threading.Tasks;

public class QTEEventsPanel : UIPanel {
    public const string TrueResultLabelText = "Успех";
    public const string FalseResultLabelText = "Неудача";
    private const int TimeDelay = 400;

    [SerializeField] private RectTransform _qTEEventViewsParent;
    [SerializeField] private TextMeshProUGUI _resoverallResultsultText;

    private IReadOnlyList<QTEEvent> _events;

    private QTESystem _qTESystem;
    private UICompanentsFactory _factory;
    private PauseHandler _pauseHandler;

    private List<QTEEventView> _eventViews;
    private Sequence _finishSequence;

    public void Init(QTESystem qTESystem, PauseHandler pauseHandler, UICompanentsFactory factory) {
        _qTESystem = qTESystem;
        _factory = factory;
        _pauseHandler = pauseHandler;

        _eventViews = new List<QTEEventView>();
        
        AddListeners();
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value == true)
            _resoverallResultsultText.text = "";

        if (_finishSequence != null)
            _finishSequence.Kill();
    }

    public override void Reset() {
        base.Reset();

        _eventViews.Clear();
        _events = null;
    }

    public override void AddListeners() {
        base.AddListeners();

        _qTESystem.EventsCreated += OnEventsCreated;
        _qTESystem.AllEventsCompleted += OnAllEventsCompleted;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _qTESystem.EventsCreated -= OnEventsCreated;
        _qTESystem.AllEventsCompleted -= OnAllEventsCompleted;
    }


    private void OnEventsCreated(IReadOnlyList<QTEEvent> events) {
        _events = events;

        _qTEEventViewsParent.gameObject.SetActive(true);
        CreateEventViews();
    }

    private async void OnAllEventsCompleted(bool resoverallResultsult) {
        ClearCompanents();
        await FinishedAnimation(resoverallResultsult);
    }

    private void CreateEventViews() {
        if (_eventViews.Count > 0)
            ClearCompanents();

        foreach (var iEvent in _events) {
            QTEEventViewConfig viewConfig = new QTEEventViewConfig();

            QTEEventView eventView = _factory.Get<QTEEventView>(viewConfig, _qTEEventViewsParent);
            eventView.Init(iEvent, _pauseHandler);

            _eventViews.Add(eventView);
        }
    }

    private void ClearCompanents() {
        if (_eventViews.Count == 0)
            return;

        foreach (var iView in _eventViews) {
            Destroy(iView.gameObject);
        }

        Reset();
    }

    private async UniTask FinishedAnimation(bool status) {
        _qTEEventViewsParent.gameObject.SetActive(false);

        string text = status ? TrueResultLabelText : FalseResultLabelText;
        _resoverallResultsultText.text = text;

        Color textColor = status ? Color.green : Color.red;

        RectTransform textTransform = _resoverallResultsultText.rectTransform;

        _finishSequence = DOTween.Sequence();
        _finishSequence.Append(textTransform.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.InOutSine))
            .Append(textTransform.DOScale(Vector3.one, 1f).SetEase(Ease.InElastic))
            .Insert(0, _resoverallResultsultText.DOColor(textColor, 0.3f))
            .AppendCallback(() => { Show(false); })
            .Play();


        await UniTask.WhenAll<Sequence>(_finishSequence);
    }


    public override void Dispose() {
        base.Dispose();

        ClearCompanents();
    }
}
