using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameplayMediator _gameplayMediator;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private SheepSpawner _spawner;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;

    private PauseHandler _pauseHandler;
    private DialogFactory _dialogFactory;
    private SheepSpawner _sheepSpawner;
    private QTESystem _qTESystem;

    [Inject]
    public void Construct(PauseHandler pauseHandler, DialogFactory dialogFactory, SheepSpawner sheepSpawner, QTESystem qTESystem) {
        _dialogFactory = dialogFactory;
        _sheepSpawner = sheepSpawner;
        _qTESystem = qTESystem;
        _pauseHandler = pauseHandler;
    }

    private void Start() {
        _uIManager.Init(_dialogFactory);
        _environmentSoundManager.Init();

        _gameplayMediator.Init(_pauseHandler, _sheepSpawner, _qTESystem, _uIManager, _environmentSoundManager);
    }
}
