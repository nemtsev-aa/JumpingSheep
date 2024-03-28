using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour {
    public const float InitDelay = 0.1f;

    [SerializeField] private GameplayMediator _gameplayMediator;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;

    private void Start() => StartCoroutine(Init());

    private IEnumerator Init() {
        yield return new WaitForSeconds(InitDelay);

        _uIManager.Init();
        _environmentSoundManager.Init();
        _gameplayMediator.Init(_uIManager, _environmentSoundManager);
    }
}
