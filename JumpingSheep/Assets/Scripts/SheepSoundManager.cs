using System;
using UnityEngine;

public class SheepSoundManager : SoundManager {
    [SerializeField] private SheepSoundConfig _config;
    
    private AnimatorEventsHandler _eventsHandler;

    public void Init(Sheep sheep) {
        _eventsHandler = sheep.EventsHandler;
        AudioSource = GetComponent<AudioSource>();

        AddListener();
        OnSheepMoved();
    }

    public override void AddListener() {
        _eventsHandler.JumpStarted += OnJumpStarted;
        _eventsHandler.StrikStarted += OnStrikStarted;
        _eventsHandler.JumpProgressed += OnSheepJumpProgressed;
    }

    public override void RemoveLisener() {
        _eventsHandler.JumpStarted -= OnJumpStarted;
        _eventsHandler.StrikStarted -= OnStrikStarted;
        _eventsHandler.JumpProgressed -= OnSheepJumpProgressed;
    }

    private void OnSheepMoved() => AudioSource.PlayOneShot(_config.Move);
    private void OnJumpStarted() => AudioSource.PlayOneShot(_config.JumpStart);
    private void OnStrikStarted() => AudioSource.PlayOneShot(_config.Strike);
    private void OnSheepJumpProgressed() => AudioSource.PlayOneShot(_config.JumpEnd);
}
