using System.Collections.Generic;
using UnityEngine;

public class QTEEventsPanel : UIPanel {
    [SerializeField] private RectTransform _qTEEventViewsParent;

    private IReadOnlyList<QTEEvent> _events;
    private UICompanentsFactory _factory;
    private PauseHandler _pauseHandler;
    private List<QTEEventView> _eventViews;

    public void Init(PauseHandler pauseHandler, IReadOnlyList<QTEEvent> events, UICompanentsFactory factory) {
        _events = events;
        _factory = factory;
        _pauseHandler = pauseHandler;

        CreateEventViews();
    }

    private void CreateEventViews() {
        _eventViews = new List<QTEEventView>();

        foreach (var iEvent in _events) {
            QTEEventViewConfig viewConfig = new QTEEventViewConfig();
            
            QTEEventView eventView = _factory.Get<QTEEventView>(viewConfig, _qTEEventViewsParent);
            eventView.Init(iEvent, _pauseHandler);

            _eventViews.Add(eventView);
        }
    }

    private void ClearCompanents() {
        foreach (var iView in _eventViews) {
            Destroy(iView.gameObject);
        }

        _eventViews.Clear();
    }

    public override void Dispose() {
        base.Dispose();

        ClearCompanents();
    }
}
