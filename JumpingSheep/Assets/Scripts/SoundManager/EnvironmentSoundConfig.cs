using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EnvironmentSoundConfig : SoundConfig {
    private const string MAIN_MENU = "https://s3.eponesh.com/games/files/13071/main_menu.mp3";
    private const string GAMEPLAY = "https://s3.eponesh.com/games/files/13071/gameplay.mp3";
    private const string GAMEOVER = "https://s3.eponesh.com/games/files/13071/gameover.mp3";

    private List<string> _clipUrl = new List<string>() { MAIN_MENU, GAMEPLAY, GAMEOVER };

    public IReadOnlyList<string> ClipUrl => _clipUrl;

    [field: SerializeField] public AudioClip UI { get; private set; }
    [field: SerializeField] public AudioClip Gameplay { get; private set; }
    [field: SerializeField] public AudioClip Gameover { get; private set; }

    public void SetUIAudioClip(AudioClip uI) {
        UI = uI;
    }

    public void SetAudioClips(AudioClip gameplay, AudioClip gameover) {
        Gameplay = gameplay;
        Gameover = gameover;
    }

    public void SetAudioClips(AudioClip uI, AudioClip gameplay, AudioClip gameover) {
        UI = uI;
        Gameplay = gameplay;
        Gameover = gameover;
    }
}
