using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameplayConfig _gameplayConfig;
    [SerializeField] private QTEEventConfigs _qTEEventConfigs;
    [SerializeField] private SheepFactory _factory;

    [SerializeField] private UIManager _uIManager;
    [SerializeField] private SheepSpawner _spawner;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;

    private SwipeHandler _swipeHandler;


    private Logger _logger;
    private DialogFactory _dialogFactory;

    public void Construct(Logger logger, DialogFactory dialogFactory, SwipeHandler swipeHandler) {
        _logger = logger;
        _dialogFactory = dialogFactory;
        _swipeHandler = swipeHandler;
    }

    private void Start() {
        _spawner.Init(_factory);
        var scoreCounter = new SheepQuantityCounter(_gameplayConfig);

        _uIManager.Init(_logger, _dialogFactory);
        _environmentSoundManager.Init();

        var gameplayMediator = new GameplayMediator(_spawner, scoreCounter, _uIManager, _environmentSoundManager);
        gameplayMediator.Init();

        gameplayMediator.ShowMainMenuDialog();
    }
}
