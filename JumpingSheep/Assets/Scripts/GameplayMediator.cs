using System;
using UnityEngine;

public class GameplayMediator : IDisposable {
    private SheepSpawner _spawner;
    private GameScoreCounter _scoreCounter;
    private GameDialog _gameDialog;
    
    private QTESystem _qTESystem;
    private Sheep _currentSheep;
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
    }

    private void AddListener() {
        _spawner.SheepCreated += OnSheepCreated;
        _gameDialog.ResetClicked += OnResetClicked;
        _scoreCounter.SheepIsOver += OnSheepIsOver;
    }

    private void RemoveLisener() {
        _spawner.SheepCreated -= OnSheepCreated;
        _gameDialog.ResetClicked -= OnResetClicked;
        _scoreCounter.SheepIsOver -= OnSheepIsOver;
    }

    private void OnSheepCreated(Sheep sheep) {
        _currentSheep = sheep;
        _currentSheep.Init(_qTESystem);

        _currentSheep.EventsHandler.Striked += OnSheepStriked;
        _currentSheep.EventsHandler.Jumped += OnSheepJumped;
    }

    private void OnSheepStriked() {
        _scoreCounter.AddStrike();
        
        _spawner.DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep();
    }

    private void OnSheepJumped() {
        _scoreCounter.AddJump();
        _spawner.DestroyCurrentSheep();

        if (_sheepOver == false)
            _spawner.CreateSheep();
    }

    private void OnSheepIsOver() {
        _sheepOver = true;
    }

    private void OnResetClicked() {
        Debug.Log("GameplayMediator: OnResetClicked");

        _scoreCounter.Reset();
        _qTESystem.Reset();
        _sheepOver = false;

        StartGame();
    }

    public void Dispose() {
        RemoveLisener();
    }
}
