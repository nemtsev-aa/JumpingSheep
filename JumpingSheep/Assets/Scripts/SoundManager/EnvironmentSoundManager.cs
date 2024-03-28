using System;
using UnityEngine;

public enum MusicType {
    UI,
    Gameplay,
    GameOver
}

public class EnvironmentSoundManager : SoundManager {
    [SerializeField] private EnvironmentSoundConfig _environmentSounds;

    public void Init() {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = Volume.Volume - 0.1f;

        AddListener();
    }

    public void PlaySound(MusicType type) {
        AudioClip currentClip;

        switch (type) {
            case MusicType.UI:
                currentClip = _environmentSounds.UI;
                break;

            case MusicType.Gameplay:
                currentClip = _environmentSounds.Gameplay;
                break;

            case MusicType.GameOver:
                currentClip = _environmentSounds.Gameover;
                break;

            default:
                throw new ArgumentException($"Invalid MusicType: {type}");
        }

        AudioSource.Stop();
        AudioSource.PlayOneShot(currentClip);
    }

    public override void AddListener() {
        Volume.VolumeChanged += OnVolumeChanged;
    }

    public override void RemoveLisener() {
        Volume.VolumeChanged -= OnVolumeChanged;
    }

    private void OnVolumeChanged(float value) => AudioSource.volume = Volume.Volume;
}
