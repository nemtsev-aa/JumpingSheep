using System;
using UnityEngine;

public enum MusicType {
    UI,
    Gameplay,
    GameOver
}

public class EnvironmentSoundManager : SoundManager {
    [SerializeField] private EnvironmentSoundConfig _environmentSounds;

    public EnvironmentSoundConfig SoundConfig => _environmentSounds;

    public void Init() {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = Volume.Volume - 0.1f;

        AddListener();
        IsInit = true;
    }

    public void PlaySound(MusicType type) {
        if (IsInit == false)
            return;

        AudioSource.Stop();

        var clip = GetAudioClipByType(type);

        if (clip != null) {
            AudioSource.clip = clip;
            AudioSource.Play();
        } 
    }

    public override void AddListener() {
        Volume.VolumeChanged += OnVolumeChanged;
    }

    public override void RemoveLisener() {
        Volume.VolumeChanged -= OnVolumeChanged;
    }

    private void OnVolumeChanged(float value) => AudioSource.volume = Volume.Volume;

    private AudioClip GetAudioClipByType(MusicType type) {
        switch (type) {
            case MusicType.UI:
                return _environmentSounds.UI;
    
            case MusicType.Gameplay:
                return _environmentSounds.Gameplay;

            case MusicType.GameOver:
                return _environmentSounds.Gameover;

            default:
                throw new ArgumentException($"Invalid MusicType: {type}");
        }
    }
}
