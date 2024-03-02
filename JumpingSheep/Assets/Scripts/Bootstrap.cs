using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private QTEEventConfigs _qTEEventConfigs;
    [SerializeField] private SheepFactory _factory;

    [SerializeField] private UIManager _uIManager;
    [SerializeField] private SheepSpawner _spawner;
  
    [SerializeField] private int _maxSheepCount = 3;

    private Logger _logger;

    private void Start() {
        _logger = new Logger();
        

        _spawner.Init(_factory);
        var scoreCounter = new SheepQuantityCounter(_maxSheepCount);

        _uIManager.Init(_logger, scoreCounter, _qTEEventConfigs);

        var gameplayMediator = new GameplayMediator(_spawner, scoreCounter, _uIManager);
        gameplayMediator.Init();

        gameplayMediator.ShowMainMenuDialog();
    }
}
