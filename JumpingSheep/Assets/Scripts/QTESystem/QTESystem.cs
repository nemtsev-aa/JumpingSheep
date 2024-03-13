using System;
using System.Collections.Generic;
using Zenject;

public class QTESystem : IPause, ITickable, IDisposable {
    public event Action Started;
    public event Action<bool> AllEventsCompleted;
    public event Action<bool> EventFinished;

    private PauseHandler _pauseHandler;
    private readonly QTESoundManager _qTESoundManager;
    private readonly SwipeHandler _swipeHandler;

    private List<QTEEvent> _events;
    private readonly List<QTEEventConfig> _eventConfigs;
    private LevelConfig _levelConfig;

    private QTEEvent _currentEvent;
    private int _successfulEventCount;
    private bool _isPaused;

    public QTESystem(PauseHandler pauseHandler, QTEEventConfigs configs, QTESoundManager qTESoundManager, SwipeHandler swipeHandler) {
        _pauseHandler = pauseHandler;
        _pauseHandler.Add(this);

        _eventConfigs = new List<QTEEventConfig>();
        _eventConfigs.AddRange(configs.Configs);
        
        _qTESoundManager = qTESoundManager;
        _qTESoundManager.Init(this);

        _swipeHandler = swipeHandler;
    }

    public IReadOnlyList<QTEEvent> Events => _events;
    private int MinSuccessfulEventCount => _levelConfig.QTEConfig.MinSuccessfulEventCount;
    private int EventsCount => _levelConfig.QTEConfig.EventsCount;

    public void SetLevelConfig(LevelConfig config) {
        _levelConfig = config;

        CreateEvents();
    }

    public void StartEvents() {
        if (_events.Count == 0) 
            CreateEvents();

        _currentEvent = _events[0];
        _currentEvent.Start();

        Started?.Invoke();
    }

    public void Tick() {
        if (_currentEvent != null && _isPaused == false)
            _currentEvent.Update();
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;

    public void Reset() {
        _successfulEventCount = 0;
        _levelConfig = null;

        ClearCompanents();
    }

    private void CreateEvents() {
        _events = new List<QTEEvent>();

        for (int i = 0; i < EventsCount; i++) {
            var index = UnityEngine.Random.Range(0, _eventConfigs.Count); 
            
            QTEEventConfig config = _eventConfigs[index];
            config.TimeToSwipe = _levelConfig.QTEConfig.EventDuration;

            CreateEvent(config);
        }
    }

    private void CreateEvent(QTEEventConfig config) {
        QTEEvent newEvent = new QTEEvent(_swipeHandler);
       
        newEvent.Init(config);
        newEvent.StateChanged += OnStateChanged;
        
        _events.Add(newEvent);
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
        bool executionResult = _successfulEventCount >= MinSuccessfulEventCount;

        AllEventsCompleted?.Invoke(executionResult);
    }

    private void ClearEvent(QTEEvent qTEEvent) {
        qTEEvent.StateChanged -= OnStateChanged;
        
        _events.Remove(qTEEvent);
        qTEEvent = null;
    }

    private void ClearCompanents() {
        if (_events.Count >= 0) {
            foreach (var iEvent in _events) {
                iEvent.StateChanged -= OnStateChanged;
            }

            _events.Clear();
        }
    }

    public void Dispose() {
        ClearCompanents();

        _pauseHandler.Remove(this);
    }

}
