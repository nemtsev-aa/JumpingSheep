using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class QTESystem : MonoBehaviour, IDisposable {
    public event Action<bool> Finished;

    [SerializeField] private TextMeshProUGUI _resultLabel;
    [SerializeField] private QTEEvent _qTEEventPrefab;
    [SerializeField] private QTEEventView _qTEEventViewPrefab;
    [SerializeField] private RectTransform _qTEEventViewsParent;

    private List<QTEEvent> _events = new List<QTEEvent>();
    private List<QTEEventView> _eventViews = new List<QTEEventView>();
    private QTEEvent _currentEvent;
    private List<QTEEventConfig> _configs = new List<QTEEventConfig>();

    public void Init(List<QTEEventConfig> configs) {
        _configs.AddRange(configs);
        _qTEEventViewsParent.transform.gameObject.SetActive(false);
    }

    public void StartEvents() {
        if (_events.Count == 0) {
            _qTEEventViewsParent.transform.gameObject.SetActive(true);
            CreateEvents();
        }

        _currentEvent = _events[0];
        _currentEvent.enabled = true;
        _currentEvent.Start();
    }

    public void Reset() {
        _resultLabel.text = "";
        ClearCompanents();
    }

    private void CreateEvents() {
        for (int i = 0; i < _configs.Count; i++) {
            var index = UnityEngine.Random.Range(0, _configs.Count); 
            QTEEventConfig config = _configs[index];

            CreateEvent(config);
        }
    }

    private void CreateEvent(QTEEventConfig config) {
        QTEEvent newEvent = Instantiate(_qTEEventPrefab, transform);
        newEvent.Init(config);
        newEvent.enabled = false;

        QTEEventView eventView = Instantiate(_qTEEventViewPrefab, _qTEEventViewsParent);
        eventView.Init(newEvent);

        newEvent.StateChanged += OnStateChanged;

        _events.Add(newEvent);
        _eventViews.Add(eventView);
    }

    private void OnStateChanged(QTEEventState state) {
        if (state == QTEEventState.FailFinished) {
            _qTEEventViewsParent.transform.gameObject.SetActive(false);
            _resultLabel.text = "�������";
            Finished?.Invoke(false);
        }

        if (state == QTEEventState.TrueFinished) {
            ClearEvent(_currentEvent);

            if (_events.Count <= 0) {
                _resultLabel.text = "�����!";
                _qTEEventViewsParent.transform.gameObject.SetActive(false);
                Finished?.Invoke(true);
            }
            else
                StartEvents();
        }
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
