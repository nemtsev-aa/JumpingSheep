using TMPro;
using UnityEngine;

public class TimerBar : Bar {
    [SerializeField] private TextMeshProUGUI _timeLabel;
    [SerializeField] private bool _showMilliseconds;

    private TimeCounter _timer;

    public void Init(TimeCounter timer) {
        _timer = timer;

        AddListeners();
    }

    public override void AddListeners() {
        base.AddListeners();

        if (_timer != null)
            _timer.HasBeenUpdated += OnValueChanged;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        if (_timer != null)
            _timer.HasBeenUpdated -= OnValueChanged;
    }

    public override void Reset() {
         _timeLabel.text = "";
    }

    protected override void OnValueChanged(float currentValue, float maxValue) {
        if(Filler != null)
            Filler.fillAmount = currentValue / maxValue;

        if (_timeLabel != null) {
            _timeLabel.text = GetFormattedTime(currentValue);
        }
    }

    private string GetFormattedTime(float timeValue) {
        int minutes = Mathf.FloorToInt(timeValue / 60f);
        int seconds = Mathf.FloorToInt(timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((timeValue * 10f) % 10f);

        string minutesText = (minutes < 10) ?  $"0{minutes}" : $"{minutes}";
        string secondsText = (seconds < 10) ? $"0{seconds}" : $"{seconds}";

        if (_showMilliseconds)
            return $"{minutesText}:{secondsText}.{milliseconds}";
        else
            return $"{minutesText}:{secondsText}";
    }
}
