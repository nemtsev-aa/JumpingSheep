using UnityEngine;
using System;

[Serializable]
public class QTESoundConfig : SoundConfig {
    [field: SerializeField] public AudioClip SuccesEvent { get; private set; }
    [field: SerializeField] public AudioClip FailEvent { get; private set; }
    [field: SerializeField] public AudioClip CompleteSuccess { get; private set; }
    [field: SerializeField] public AudioClip CompleteFail { get; private set; }
}
