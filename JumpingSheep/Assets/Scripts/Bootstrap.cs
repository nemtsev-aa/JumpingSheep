using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameplayMediator _gameplayMediator;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;

    private void Start() {
        _uIManager.Init();
        _environmentSoundManager.Init();
        _gameplayMediator.Init(_uIManager, _environmentSoundManager);
    }
}
