using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class QTESoundConfig : SoundConfig {
    private const string SUCCESEVENT = "https://s3.eponesh.com/games/files/13071/qte_event-success2.mp3";
    private const string FAILEVENT = "https://s3.eponesh.com/games/files/13071/qte_event-fail.mp3";
    private const string COMPLETESUCCESS = "https://s3.eponesh.com/games/files/13071/qte_complete-success.mp3";
    private const string COMPLETEFAIL = "https://s3.eponesh.com/games/files/13071/qte_complete-fail.mp3";

    private List<string> _clipUrl = new List<string>() { SUCCESEVENT, FAILEVENT, COMPLETESUCCESS, COMPLETEFAIL };

    public void SetAudioClips(AudioClip succesEvent, AudioClip failEvent, AudioClip completeSuccess, AudioClip completeFail) {
        SuccesEvent = succesEvent;
        FailEvent = failEvent;
        CompleteSuccess = completeSuccess;
        CompleteFail = completeFail;
    }

    public IReadOnlyList<string> ClipUrl => _clipUrl;

    [field: SerializeField] public AudioClip SuccesEvent { get; private set; }
    [field: SerializeField] public AudioClip FailEvent { get; private set; }
    [field: SerializeField] public AudioClip CompleteSuccess { get; private set; }
    [field: SerializeField] public AudioClip CompleteFail { get; private set; }
}
