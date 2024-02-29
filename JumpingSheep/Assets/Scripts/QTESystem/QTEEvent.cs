using System;
using UnityEngine;

public enum QTEEventState {
    Started,
    TrueFinished,
    FailFinished
}

public class QTEEvent : MonoBehaviour {
    public event Action<QTEEventState> StateChanged;

    private QTEEventState _currentState;
    private float _time;

    public QTEEventConfig Config { get; private set; }
    private float _eventTime => Config.KeyTime;
    
    public void Init(QTEEventConfig config) {
        Config = config;
    }

    public void Start() {
        _currentState = QTEEventState.Started;
        StateChanged?.Invoke(_currentState);
    }

    private void Update() {
        _time += Time.deltaTime;

        if (_time >= _eventTime) {
            _time = 0f;
            StateChanged?.Invoke(QTEEventState.FailFinished);
            return;
        }

        if (Input.GetKeyDown(Config.KeyKode)) {
            StateChanged?.Invoke(QTEEventState.TrueFinished);
            return;
        }
    }
}
