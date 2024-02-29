using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private SheepFactory _factory;
    [SerializeField] private SheepSpawner _spawner;
    [SerializeField] private GameDialog _gameDialog;

    [SerializeField] private int _maxSheepCount = 3;

    private void Start() {
        _spawner.Init(_factory);
        var scoreCounter = new GameScoreCounter(_maxSheepCount);
        _gameDialog.Init(scoreCounter);
        
        var gameplayMediator = new GameplayMediator(_spawner, scoreCounter, _gameDialog);
        gameplayMediator.Init();
        gameplayMediator.StartGame();
    }
}
