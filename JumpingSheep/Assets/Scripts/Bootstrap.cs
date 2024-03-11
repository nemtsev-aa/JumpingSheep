using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour {
    
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private SheepSpawner _spawner;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;

    private DialogFactory _dialogFactory;
    private SheepFactory _sheepFactory;

    [Inject]
    public void Construct(DialogFactory dialogFactory, SheepFactory sheepFactory) {
        _dialogFactory = dialogFactory;
        _sheepFactory = sheepFactory;
    }

    private void Start() {
        _spawner.Init(_sheepFactory);
       
        _uIManager.Init(_dialogFactory);
        _environmentSoundManager.Init();

        var gameplayMediator = new GameplayMediator(_spawner, _uIManager, _environmentSoundManager);
        gameplayMediator.Init();

        
    }
}
