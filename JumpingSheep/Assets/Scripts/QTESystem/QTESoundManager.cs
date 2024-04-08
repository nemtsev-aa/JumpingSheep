using UnityEngine;


public class QTESoundManager : SoundManager {
    [SerializeField] private QTESoundConfig _config;
    public QTESoundConfig SoundConfig => _config;


    private QTESystem _qTESystem;

    public void Init(QTESystem qTESystem) {
        _qTESystem = qTESystem;
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = Volume.Volume;

        AddListener();
    }

    public override void AddListener() {
        _qTESystem.EventFinished += OnQTESystemEventFinished;
        _qTESystem.AllEventsCompleted += OnQTESystemAllEventsCompleted;
    }

    public override void RemoveLisener() {
        _qTESystem.EventFinished -= OnQTESystemEventFinished;
        _qTESystem.AllEventsCompleted -= OnQTESystemAllEventsCompleted;
    }

    private void OnQTESystemEventFinished(bool result) {
        if (result)
            PlaySuccesEventClip();
        else
            PlayFailEventClip();
    }

    private void OnQTESystemAllEventsCompleted(bool result) {
        if (result)
            PlayCompleteSuccessClip();
        else
            PlayCompleteFailClip();
    }

    private void PlaySuccesEventClip() => PlayOneShot(_config.SuccesEvent);

    private void PlayFailEventClip() => PlayOneShot(_config.FailEvent);

    private void PlayCompleteSuccessClip() => PlayOneShot(_config.CompleteSuccess);

    private void PlayCompleteFailClip() => PlayOneShot(_config.CompleteFail);

    private void PlayOneShot(AudioClip clip) {
        
        if (clip != null) 
            AudioSource.PlayOneShot(clip);
        
    }

}
