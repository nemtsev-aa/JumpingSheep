using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

public class QTESystem : IPause, ITickable, IDisposable {
    public event Action Started;
    public event Action<IReadOnlyList<QTEEvent>> EventsCreated;
    public event Action<bool> AllEventsCompleted;
    public event Action<bool> EventFinished;

    private PauseHandler _pauseHandler;
    private readonly QTESoundManager _qTESoundManager;
    private readonly SwipeHandler _swipeHandler;

    private Queue<QTEEvent> _events;
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

        _events = new Queue<QTEEvent>();
    }

    private int MinSuccessfulEventCount => _levelConfig.QTEConfig.MinSuccessfulEventCount;
    private int EventsCount => _levelConfig.QTEConfig.EventsCount;

    public void SetLevelConfig(LevelConfig config) => _levelConfig = config;

    public void StartEvents() {
        CreateEvents();

        StartNextEvent();

        Started?.Invoke();
    }

    public void Tick() {
        if (_currentEvent != null && _isPaused == false)
            _currentEvent.Update();
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;

    public void Reset() {
        _successfulEventCount = 0;

        ClearEvents();
    }

    private void CreateEvents() {
        for (int i = 0; i < EventsCount; i++) {
            var index = UnityEngine.Random.Range(0, _eventConfigs.Count);

            QTEEventConfig config = _eventConfigs[index];
            config.TimeToSwipe = _levelConfig.QTEConfig.EventDuration;

            CreateEvent(config);
        }

        EventsCreated?.Invoke(new List<QTEEvent>(_events));
    }

    private void CreateEvent(QTEEventConfig config) {
        QTEEvent newEvent = new QTEEvent(_swipeHandler);

        newEvent.Init(config);
        newEvent.StateChanged += OnStateChanged;

        _events.Enqueue(newEvent);
    }

    private void StartNextEvent() {
        if (_events.Count > 0) {
            _currentEvent = _events.Dequeue();
            _currentEvent.Start();

            return;
        }

        SummingUpResults(300f);
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

        StartNextEvent();
    }

    private async void SummingUpResults(double delay) {
        bool executionResult = _successfulEventCount >= MinSuccessfulEventCount;
        _successfulEventCount = 0;

        await Task.Delay(TimeSpan.FromMilliseconds(delay));
        AllEventsCompleted?.Invoke(executionResult);
    }

    private void ClearEvents() {
        foreach (var iEvent in _events) {
            ClearEvent(iEvent);
        }

        _events.Clear();
    }

    private void ClearEvent(QTEEvent qTEEvent) {
        qTEEvent.StateChanged -= OnStateChanged;
        qTEEvent.Dispose();
    }

    public void Dispose() {
        ClearEvents();

        _levelConfig = null;
        _pauseHandler.Remove(this);
    }

}
