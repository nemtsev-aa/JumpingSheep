using System;
using UnityEngine;

public enum QTEEventState {
    Created,
    Started,
    TrueFinished,
    FailFinished
}

public class QTEEvent : IDisposable {
    public event Action<QTEEventState> StateChanged;
    public event Action Disabled;

    private float _time;
    private SwipeDirection _direction;
    private readonly SwipeHandler _swipeHandler;

    public QTEEvent(SwipeHandler swipeHandler) {
        _swipeHandler = swipeHandler;
    }

    ~QTEEvent() {
        
    }

    public QTEEventState CurrentState { get; private set; }
    public QTEEventConfig Config { get; private set; }
    private float TimeToSwipe => Config.TimeToSwipe;
    
    public void Init(QTEEventConfig config) {
        Config = config;
        CurrentState = QTEEventState.Created;
    }

    public void Start() {
        CurrentState = QTEEventState.Started;

        AddListener();
        StateChanged?.Invoke(CurrentState);
    }

    public void Update() {
        if (CurrentState != QTEEventState.Started)
            return;

        _time += Time.deltaTime;

        if (_time >= TimeToSwipe) {
            _time = 0f;
            CurrentState = QTEEventState.FailFinished;
            
            StateChanged?.Invoke(CurrentState);
            return;
        }

        if (_direction != SwipeDirection.None && _direction == Config.SwipeDirectionValue) {
            CurrentState = QTEEventState.TrueFinished;
            
            StateChanged?.Invoke(CurrentState);
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
        Disabled?.Invoke();

        RemoveLisener();
    }
}
