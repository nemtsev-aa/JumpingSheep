using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameplayConfig _gameplayConfig;
    [SerializeField] private QTEEventConfigs _qTEEventConfigs;
    [SerializeField] private SheepFactory _factory;

    [SerializeField] private UIManager _uIManager;
    [SerializeField] private SheepSpawner _spawner;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;

    [SerializeField] private MovementHandler _movementHandler;


    private Logger _logger;

    private void Start() {
        _logger = new Logger();

        _movementHandler.Init();

        _spawner.Init(_factory);
        var scoreCounter = new SheepQuantityCounter(_gameplayConfig);

        _uIManager.Init(_logger, scoreCounter, _qTEEventConfigs, _movementHandler);
        _environmentSoundManager.Init();

        var gameplayMediator = new GameplayMediator(_spawner, scoreCounter, _uIManager, _environmentSoundManager);
        gameplayMediator.Init();

        gameplayMediator.ShowMainMenuDialog();
    }
}
