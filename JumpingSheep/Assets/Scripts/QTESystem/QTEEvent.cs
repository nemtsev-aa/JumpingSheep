using System;
using UnityEngine;
using Zenject;

public enum QTEEventState {
    Started,
    TrueFinished,
    FailFinished
}

public class QTEEvent : IDisposable {
    public event Action<QTEEventState> StateChanged;

    private float _time;
    private SwipeDirection _direction;
    private readonly SwipeHandler _swipeHandler;

    public QTEEvent(SwipeHandler swipeHandler) {
        _swipeHandler = swipeHandler;
    }
    ~QTEEvent() {
        Debug.Log("QTEEvent: Destructor was called");
    }

    public QTEEventState CurrentState { get; private set; }
    public QTEEventConfig Config { get; private set; }
    private float EventTime => Config.TimeToSwipe;
    
    public void Init(QTEEventConfig config) {
        Config = config;
    }

    public void Start() {
        CurrentState = QTEEventState.Started;

        AddListener();
        StateChanged?.Invoke(CurrentState);
    }

    public void Update() {
        _time += Time.deltaTime;

        if (_time >= EventTime) {
            _time = 0f;
            StateChanged?.Invoke(QTEEventState.FailFinished);
            return;
        }

        if (_direction == Config.SwipeDirectionValue) {
            StateChanged?.Invoke(QTEEventState.TrueFinished);
            return;
        }
    }

    private void AddListener() {
        _swipeHandler.SwipeDirectionChanged += OnSwipeDirectionChanged;
    }

    private void RemoveLisener() {
        _swipeHandler.SwipeDirectionChanged -= OnSwipeDirectionChanged;
    }

    private void OnSwipeDirectionChanged(SwipeDirection direction) {
        _direction = direction;
    }

    public void Dispose() {
        RemoveLisener();
    }
}
