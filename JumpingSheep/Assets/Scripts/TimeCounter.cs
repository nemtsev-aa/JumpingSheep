using System;
using UnityEngine;
using Zenject;

public class TimeCounter : ITickable {
    public event Action TimeIsOver;
    public event Action<float, float> HasBeenUpdated;

    private float _time;
    private float _remainingTime;
    private float _elapsedTime;

    private bool _countdown;
    private bool _countup;

    public TimeCounter() {

    }

    public float ElapsedTime => _elapsedTime;
    
    public float RemainingTime => _remainingTime;

    public void SetTimeValue(float time) {
        _time = time;
        _remainingTime = time;
        _elapsedTime = 0f;
    }

    public void SetTimerStatus(bool status) => _countdown = status;

    public void SetWatchStatus(bool status) => _countup = status;
    
    public void Reset() {
        _time = _remainingTime = _elapsedTime = 0;
        _countdown = _countup = false;
    }

    public void Tick() {
        if (_countdown == false && _countup == false)
            return;

        if (_countdown == true && _remainingTime > 0) {
            _remainingTime -= Time.deltaTime;
            
            if (_remainingTime <= 0) {
                TimeIsOver?.Invoke();
                Reset();
                
                return;
            }

            HasBeenUpdated?.Invoke(_remainingTime, _time);
        }

        if (_countup == true) {
            _elapsedTime += Time.deltaTime;

            HasBeenUpdated?.Invoke(_elapsedTime, _time);
        }
    }
}
