using UnityEngine;


public class QTESoundManager : SoundManager {
    [SerializeField] private QTESoundConfig _config;

    private QTESystem _qTESystem;

    public void Init(QTESystem qTESystem) {
        _qTESystem = qTESystem;
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = Configs.Volume;

        AddListener();
    }

    public override void AddListener() {
        _qTESystem.EventFinished += OnQTESystemEventFinished;
        _qTESystem.Completed += OnQTESystemCompleted;
    }

    public override void RemoveLisener() {
        _qTESystem.EventFinished -= OnQTESystemEventFinished;
        _qTESystem.Completed -= OnQTESystemCompleted;
    }

    private void OnQTESystemEventFinished(bool result) {
        if (result)
            PlaySuccesEventClip();
        else
            PlayFailEventClip();
    }

    private void OnQTESystemCompleted(bool result) {
        if (result)
            PlayCompleteSuccessClip();
        else
            PlayCompleteFailClip();
    }

    private void PlaySuccesEventClip() => AudioSource.PlayOneShot(_config.SuccesEvent);

    private void PlayFailEventClip() => AudioSource.PlayOneShot(_config.FailEvent);

    private void PlayCompleteSuccessClip() => AudioSource.PlayOneShot(_config.CompleteSuccess);

    private void PlayCompleteFailClip() => AudioSource.PlayOneShot(_config.CompleteFail);

}
