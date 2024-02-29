using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class QTESystem : MonoBehaviour, IDisposable {
    public event Action<bool> Finished;

    [SerializeField] private TextMeshProUGUI _resultLabel;
    [SerializeField] private List<QTEEventConfig> Configs = new List<QTEEventConfig>();
    [SerializeField] private QTEEvent _qTEEventPrefab;
    [SerializeField] private QTEEventView _qTEEventViewPrefab;
    [SerializeField] private RectTransform _qTEEventViewsParent;

    private List<QTEEvent> _events = new List<QTEEvent>();
    private List<QTEEventView> _eventViews = new List<QTEEventView>();
    private QTEEvent _currentEvent;

    public void Init() {
        _qTEEventViewsParent.transform.gameObject.SetActive(false);
        CreateEvents();
    }

    public void StartFirstEvent() {
        _qTEEventViewsParent.transform.gameObject.SetActive(true);

        _currentEvent = _events[0];
        _currentEvent.enabled = true;
        _currentEvent.Start();
    }

    public void Reset() {
        _resultLabel.text = "";
        ClearCompanents();
    }

    private void CreateEvents() {
        for (int i = 0; i < Configs.Count; i++) {
            var index = UnityEngine.Random.Range(0, Configs.Count); 
            QTEEventConfig config = Configs[index];

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
            _resultLabel.text = "Неудача";
            Finished?.Invoke(false);
        }

        if (state == QTEEventState.TrueFinished) {
            ClearEvent(_currentEvent);

            if (_events.Count <= 0) {
                _resultLabel.text = "Успех!";
                _qTEEventViewsParent.transform.gameObject.SetActive(false);
                Finished?.Invoke(true);
            }
            else
                StartFirstEvent();
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
