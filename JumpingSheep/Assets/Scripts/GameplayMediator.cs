using System;

public class GameplayMediator : IDisposable {
    private SheepSpawner _spawner;
    private GameScoreCounter _scoreCounter;
    private QTESystem _qTESystem;
    private Sheep _currentSheep;
    private GameDialog _gameDialog;
    private bool _sheepOver;

    public GameplayMediator(SheepSpawner spawner, GameScoreCounter scoreCounter, GameDialog dialog) {
        _spawner = spawner;
        _scoreCounter = scoreCounter;
        _qTESystem = dialog.QTESystem;
        _gameDialog = dialog;
    }

    public void Init() {
          AddListener();
    }

    public void StartGame() {
        _spawner.CreateSheep();

        _currentSheep.EventsHandler.Striked += OnStriked;
        _currentSheep.EventsHandler.Jumped += OnJumped;
    }

    private void AddListener() {
        _spawner.SheepCreated += OnSheepCreated;
        _gameDialog.ResetClicked += OnResetClicked;
        _scoreCounter.SheepIsOver += OnSheepIsOver;
    }


    private void RemoveLisener() {
        _spawner.SheepCreated -= OnSheepCreated;
        _currentSheep.EventsHandler.Striked -= OnStriked;
        _currentSheep.EventsHandler.Jumped -= OnJumped;

        _gameDialog.ResetClicked -= OnResetClicked;
        _scoreCounter.SheepIsOver -= OnSheepIsOver;
    }


    private void OnSheepCreated(Sheep sheep) {
        _currentSheep = sheep;
        _currentSheep.Init(_qTESystem);
    }

    private void OnStriked() {
        _scoreCounter.AddStrike();

        _spawner.DestroyCurrentSheep();
        _spawner.CreateSheep();
    }

    private void OnJumped() {
        _scoreCounter.AddJump();

        _spawner.DestroyCurrentSheep();
        _spawner.CreateSheep();
    }

    private void OnSheepIsOver() {
        _sheepOver = true;
    }

    private void OnResetClicked() {
        _scoreCounter.Reset();
        _qTESystem.Reset();


    }

    public void Dispose() {
        RemoveLisener();
    }
}
