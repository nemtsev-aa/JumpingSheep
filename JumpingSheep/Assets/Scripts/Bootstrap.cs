using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour {
    public const float InitDelay = 0.1f;

    [SerializeField] private GameplayMediator _gameplayMediator;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;
    [SerializeField] private SheepSFXManager _sheepSFXManager;
    [SerializeField] private SoundsLoader _soundsLoader;

    private Logger _logger;
    //private SoundsLoader _soundsLoader;

    [Inject]
    public void Construct(Logger logger) {
        _logger = logger;
        //_soundsLoader = soundsLoader;
    }
    
    private void Start() => StartCoroutine(Init());

    private IEnumerator Init() {
        _logger.Log("Bootstrap Init");
        _soundsLoader.Init();

        yield return new WaitForSeconds(InitDelay);
        EnvironmentSoundManagerInit();
        SheepSFXManagerInit();

        yield return new WaitForSeconds(InitDelay);
        _uIManager.Init(_gameplayMediator);
        _gameplayMediator.Init(_uIManager, _environmentSoundManager, _sheepSFXManager);

        _logger.Log("Bootstrap Complited");
    }


    private void EnvironmentSoundManagerInit() {
        List<AudioClip> sounds = _soundsLoader.LoadAssets(_environmentSoundManager.SoundConfig.ClipUrl);

        if (sounds != null) {
            _environmentSoundManager.SoundConfig.SetAudioClips(sounds[0], sounds[1], sounds[2]);
            _environmentSoundManager.Init();
        }
    }

    private void SheepSFXManagerInit() {
        List<AudioClip> sounds = _soundsLoader.LoadAssets(_sheepSFXManager.SoundConfig.ClipUrl);

        if (sounds != null) 
            _sheepSFXManager.SoundConfig.SetAudioClips(sounds[0], sounds[1], sounds[2], sounds[3]);
    }
}
