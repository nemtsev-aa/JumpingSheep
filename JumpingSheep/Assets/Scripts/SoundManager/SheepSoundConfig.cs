using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SheepSoundConfig : SoundConfig {
    private const string MOVE = "https://s3.eponesh.com/games/files/13071/animal-walking-on-grass3.mp3";
    private const string STRIKE = "https://s3.eponesh.com/games/files/13071/sfx-strike1.mp3";
    private const string JUMPSTART = "https://s3.eponesh.com/games/files/13071/sfx-jump1.mp3";
    private const string JUMPEND = "https://s3.eponesh.com/games/files/13071/sfx-end-jump.mp3";

    private List<string> _clipUrl = new List<string>() { MOVE, STRIKE, JUMPSTART, JUMPEND };

    public IReadOnlyList<string> ClipUrl => _clipUrl;

    [field: SerializeField] public AudioClip Move { get; private set; }
    [field: SerializeField] public AudioClip Strike { get; private set; }
    [field: SerializeField] public AudioClip JumpStart { get; private set; }
    [field: SerializeField] public AudioClip JumpEnd { get; private set; }

    public void SetAudioClips(AudioClip move, AudioClip strike, AudioClip jumpStart, AudioClip jumpEnd) {
        Move = move;
        Strike = strike;
        JumpStart = jumpStart;
        JumpEnd = jumpEnd;
    }
}
