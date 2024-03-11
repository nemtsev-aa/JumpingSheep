using System;
using UnityEngine;

public enum QTEEventState {
    Started,
    TrueFinished,
    FailFinished
}

public class QTEEvent : MonoBehaviour, IDisposable {
    public event Action<QTEEventState> StateChanged;

    private QTEEventState _currentState;
    private float _time;

    public QTEEventConfig Config { get; private set; }

    private SwipeHandler _movementHandler;
    private SwipeDirection _direction;

    private float _eventTime => Config.KeyTime;
    
    public void Init(QTEEventConfig config, SwipeHandler movementHandler) {
        Config = config;
        _movementHandler = movementHandler;
    }

    public void Start() {
        _currentState = QTEEventState.Started;

        AddListener();
        StateChanged?.Invoke(_currentState);
    }

    private void Update() {
        _time += Time.deltaTime;

        if (_time >= _eventTime) {
            _time = 0f;
            StateChanged?.Invoke(QTEEventState.FailFinished);
            return;
        }

        if (Input.GetKeyDown(Config.KeyKode) || _direction == Config.SwipeDirection) {
            StateChanged?.Invoke(QTEEventState.TrueFinished);
            return;
        }
    }

    private void AddListener() {
        _movementHandler.SwipeDirectionChanged += OnSwipeDirectionChanged;
    }

    private void RemoveLisener() {
        _movementHandler.SwipeDirectionChanged -= OnSwipeDirectionChanged;
    }

    private void OnSwipeDirectionChanged(SwipeDirection direction) {
        _direction = direction;
    }

    public void Dispose() {
        RemoveLisener();
    }

    internal void Init(QTEEventConfig config, object movementHandler) {
        throw new NotImplementedException();
    }
}
