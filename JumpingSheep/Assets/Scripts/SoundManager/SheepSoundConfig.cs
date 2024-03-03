using System;
using UnityEngine;

[Serializable]
public class SheepSoundConfig : SoundConfig {
    [field: SerializeField] public AudioClip Move { get; private set; }
    [field: SerializeField] public AudioClip Strike { get; private set; }
    [field: SerializeField] public AudioClip JumpStart { get; private set; }
    [field: SerializeField] public AudioClip JumpEnd { get; private set; }
}
