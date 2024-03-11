using System;
using System.Collections.Generic;
using UnityEngine;


public class QTESystem : MonoBehaviour, IDisposable {
    public event Action Started;
    public event Action<bool> Completed;
    public event Action<bool> EventFinished;

    [SerializeField] private QTESystemConfig _systemConfig;

    [Space(10)]
    [SerializeField] private QTEEvent _qTEEventPrefab;
    [SerializeField] private QTEEventView _qTEEventViewPrefab;
    [SerializeField] private RectTransform _qTEEventViewsParent;
    [SerializeField] private QTESoundManager _qTESoundManager;

    private readonly List<QTEEvent> _events = new List<QTEEvent>();
    private readonly List<QTEEventView> _eventViews = new List<QTEEventView>();
    private readonly List<QTEEventConfig> _configs = new List<QTEEventConfig>();
    private QTEEvent _currentEvent;
    private int _successfulEventCount;
    

    private int _minSuccessfulEventCount => _systemConfig.MinSuccessfulEventCount;
    private int _eventsCount => _systemConfig.EventsCount;

    private SwipeHandler _movementHandler;


    public void Init(List<QTEEventConfig> configs, SwipeHandler movementHandler) {
        _configs.AddRange(configs);
        _qTEEventViewsParent.transform.gameObject.SetActive(false);
        _qTESoundManager.Init(this);

        _movementHandler = movementHandler;
    }

    public void StartEvents() {
        if (_events.Count == 0) {
            _qTEEventViewsParent.transform.gameObject.SetActive(true);
            CreateEvents();
        }

        _currentEvent = _events[0];
        _currentEvent.enabled = true;
        _currentEvent.Start();

        Started?.Invoke();
    }

    public void Reset() {
        _successfulEventCount = 0;
        ClearCompanents();
    }

    private void CreateEvents() {
        for (int i = 0; i < _eventsCount; i++) {
            var index = UnityEngine.Random.Range(0, _configs.Count); 
            QTEEventConfig config = _configs[index];

            CreateEvent(config);
        }
    }

    private void CreateEvent(QTEEventConfig config) {
        QTEEvent newEvent = Instantiate(_qTEEventPrefab, transform);
        newEvent.Init(config, _movementHandler);
        newEvent.enabled = false;

        QTEEventView eventView = Instantiate(_qTEEventViewPrefab, _qTEEventViewsParent);
        eventView.Init(newEvent);

        newEvent.StateChanged += OnStateChanged;

        _events.Add(newEvent);
        _eventViews.Add(eventView);
    }

    private void OnStateChanged(QTEEventState state) {
        if (state == QTEEventState.Started)
            return;

        if (state == QTEEventState.FailFinished) {
            EventFinished?.Invoke(false);
            ClearEvent(_currentEvent);
        }

        if (state == QTEEventState.TrueFinished) {
            _successfulEventCount++;
            EventFinished?.Invoke(true);
            ClearEvent(_currentEvent);
        }

        if (_events.Count > 0) {
            StartEvents();
            return;
        }

        SummingUpResults();  
    }

    private void SummingUpResults() {
        _qTEEventViewsParent.transform.gameObject.SetActive(false);
        bool executionResult = _successfulEventCount >= _minSuccessfulEventCount ? true : false;

        Completed?.Invoke(executionResult);
    }

    private void ClearEvent(QTEEvent qTEEvent) {
        qTEEvent.StateChanged -= OnStateChanged;
        
        _events.Remove(qTEEvent);
        Destroy(qTEEvent.gameObject);
    }

    private void ClearCompanents() {
        if (_events.Count >= 0) {
            foreach (var iEvent in _events) {
                iEvent.StateChanged -= OnStateChanged;
                Destroy(iEvent.gameObject);
            }

            _events.Clear();
        }

        foreach (var iView in _eventViews) {
            Destroy(iView.gameObject);
        }

        _eventViews.Clear();
    }

    public void Dispose() {
        ClearCompanents();
    }
}
