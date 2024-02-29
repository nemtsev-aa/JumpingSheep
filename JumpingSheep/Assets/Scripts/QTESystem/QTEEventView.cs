using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEEventView : MonoBehaviour, IDisposable {
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillerImage;
    [SerializeField] private TextMeshProUGUI _labelText;

    private QTEEvent _qteEvent;
    private float _eventTime => _qteEvent.Config.KeyTime;
    private float _time;

    public void Init(QTEEvent qteEvent) {
        _qteEvent = qteEvent;
        _labelText.text = _qteEvent.Config.KeyText;

        AddListener();
    }

    private void Update() {
        if (_qteEvent != null && _qteEvent.enabled == true) {
            _time += Time.deltaTime;

            if (_time <= _eventTime)
                _fillerImage.fillAmount = (_eventTime - _time) / _eventTime;
        }
    }

    private void AddListener() {
        _qteEvent.StateChanged += OnStateChanged;
    }

    private void RemoveLisener() {
        _qteEvent.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(QTEEventState state) {
        switch (state) {
            case QTEEventState.Started:
                ShowStartedAnimation();
                break;

            case QTEEventState.TrueFinished:
                ShowTrueFinishedAnimation();
                break;

            case QTEEventState.FailFinished:
                ShowFallFinishedAnimation();
                break;

            default:
                break;
        }
    }

    private void ShowStartedAnimation() {
        
    }

    private void ShowTrueFinishedAnimation() {
        _qteEvent = null;
    }

    private void ShowFallFinishedAnimation() {
        _qteEvent = null;
    }

    public void Dispose() {
        RemoveLisener();
    }
}
