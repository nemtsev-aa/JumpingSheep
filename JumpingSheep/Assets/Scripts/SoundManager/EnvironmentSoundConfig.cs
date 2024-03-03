using UnityEngine;
using System;

[Serializable]
public class EnvironmentSoundConfig : SoundConfig {
    [field: SerializeField] public AudioClip UI { get; private set; }
    [field: SerializeField] public AudioClip Gameplay { get; private set; }
    [field: SerializeField] public AudioClip Gameover { get; private set; }
}
