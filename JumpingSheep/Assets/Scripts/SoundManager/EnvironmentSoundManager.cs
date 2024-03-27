using System;
using UnityEngine;

public enum MusicType {
    UI,
    Gameplay,
    GameOver
}

public class EnvironmentSoundManager : SoundManager {
    [SerializeField] private EnvironmentSoundConfig _config;

    public void Init() {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = Configs.Volume - 0.1f;

        AddListener();
    }

    public void PlaySound(MusicType type) {
        AudioClip currentClip;

        switch (type) {
            case MusicType.UI:
                currentClip = _config.UI;
                break;

            case MusicType.Gameplay:
                currentClip = _config.Gameplay;
                break;

            case MusicType.GameOver:
                currentClip = _config.Gameover;
                break;

            default:
                throw new ArgumentException($"Invalid MusicType: {type}");
        }

        AudioSource.Stop();
        AudioSource.PlayOneShot(currentClip);
    }

    public override void AddListener() {
        Configs.VolumeChanged += OnVolumeChanged;
    }

    public override void RemoveLisener() {
        Configs.VolumeChanged -= OnVolumeChanged;
    }

    private void OnVolumeChanged(float value) => AudioSource.volume = Configs.Volume;
}
