using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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

    private async void Start() {
        await Init();
    }

    private async UniTask Init() {
        _logger.Log("Bootstrap Init");

        await PlayerProgressLoading();

        _logger.Log("UIManager Init");
        _uIManager.Init(_gameplayMediator);

        _logger.Log("GameplayMediator Init");
        _gameplayMediator.Init(_uIManager, _soundsLoader, _environmentSoundManager, _sheepSFXManager);

        await SoundsLoading();

        _logger.Log("Bootstrap Complited");
    }

    private async UniTask<bool> SoundsLoading() {
        _logger.Log("Sounds Loading...");

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

    private async UniTask<bool> TryEnvironmentSoundManagerInit() {
        await _soundsLoader.LoadAsset(_environmentSoundManager.SoundConfig.ClipUrl[0], OnUIAudioClipLoaded);

        List<AudioClip> sounds = await _soundsLoader.LoadAssets(new List<string>() {
            _environmentSoundManager.SoundConfig.ClipUrl[1],
            _environmentSoundManager.SoundConfig.ClipUrl[2],
            }
        );

        if (sounds != null) {
            _environmentSoundManager.SoundConfig.SetAudioClips(sounds[0], sounds[1]);
            _logger.Log($"EnvironmentSoundManagerInit Complited: {sounds.Count}");

            return true;
        }

        _logger.Log($"EnvironmentSoundManagerInit Not Complited");
        return false;
    }

    private void OnUIAudioClipLoaded(AudioClip clip) {
        _environmentSoundManager.SoundConfig.SetUIAudioClip(clip);
        _environmentSoundManager.Init();
        _environmentSoundManager.PlaySound(MusicType.UI);
    }

    private async UniTask<bool> TrySheepSFXManagerInit() {
        List<AudioClip> sounds = await _soundsLoader.LoadAssets(_sheepSFXManager.SoundConfig.ClipUrl);

        if (sounds != null) {
            _sheepSFXManager.SoundConfig.SetAudioClips(sounds[0], sounds[1], sounds[2], sounds[3]);

            _logger.Log($"SheepSFXManagerInit Complited: {sounds.Count}");
            return true;
        }

        _logger.Log($"SheepSFXManagerInit Not Complited");
        return false;
    }

    private async UniTask PlayerProgressLoading() {
        await _playerProgressManager.LoadProgress();

    }
}
