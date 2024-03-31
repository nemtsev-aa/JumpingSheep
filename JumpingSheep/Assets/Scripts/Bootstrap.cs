using Zenject;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Bootstrap : MonoBehaviour {
    public const int InitDelay = 1;

    [SerializeField] private GameplayMediator _gameplayMediator;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;
    [SerializeField] private SheepSFXManager _sheepSFXManager;

    private Logger _logger;
    private PlayerProgressManager _playerProgressManager;
    private SoundsLoader _soundsLoader;

    [Inject]
    public void Construct(Logger logger, PlayerProgressManager playerProgressManager, SoundsLoader soundsLoader) {
        _logger = logger;
        _playerProgressManager = playerProgressManager;
        _soundsLoader = soundsLoader;
    }

    private async void Start() => await Init();

    private async Task Init() {
        _logger.Log("Bootstrap Init");

        await SoundsLoading();

        await _playerProgressManager.LoadProgress();

        _uIManager.Init(_gameplayMediator);
        _gameplayMediator.Init(_uIManager, _environmentSoundManager, _sheepSFXManager);

        _logger.Log("Bootstrap Complited");
    }

    private async Task<bool> SoundsLoading() {
        _soundsLoader.Init(_logger);

        bool envSound = await TryEnvironmentSoundManagerInit();
        bool sfx = await TrySheepSFXManagerInit();

        if (envSound == true && sfx == true) {
            _logger.Log("Sounds Loading Complited");
            return true;
        }
        else {
            _logger.Log("Sounds Loading Not Complited");
            return false;
        }
    }

    private async Task<bool> TryEnvironmentSoundManagerInit() {
        List<AudioClip> sounds = await _soundsLoader.LoadAssets(_environmentSoundManager.SoundConfig.ClipUrl);

        await Task.Delay(InitDelay);

        if (sounds != null) {
            _environmentSoundManager.SoundConfig.SetAudioClips(sounds[0], sounds[1], sounds[2]);
            _environmentSoundManager.Init();

            _logger.Log($"EnvironmentSoundManagerInit Complited: {sounds.Count}");
            return true;
        }

        _logger.Log($"EnvironmentSoundManagerInit Not Complited: {sounds.Count}");
        return false;
    }

    private async Task<bool> TrySheepSFXManagerInit() {
        List<AudioClip> sounds = await _soundsLoader.LoadAssets(_sheepSFXManager.SoundConfig.ClipUrl);

        await Task.Delay(InitDelay);

        if (sounds != null) {
            _sheepSFXManager.SoundConfig.SetAudioClips(sounds[0], sounds[1], sounds[2], sounds[3]);

            _logger.Log($"SheepSFXManagerInit Complited: {sounds.Count}");
            return true;
        }

        _logger.Log($"SheepSFXManagerInit Not Complited: {sounds.Count}");
        return false;
    }
}
