using System;

public class Score {
    public event Action ScoreChanged;

    private int _strikeCount;
    private int _jumpCount;

    private int _trueSwipeCount;
    private int _falseSwipeCount;
    private float _trueSwipePercentage;

    private LevelConfig _levelConfig;

    public Score() {

    }

    public int StarsCount { get; private set; }
    public int MaxQuantity { get; private set; }
    public string Result => $"{_jumpCount}/{MaxQuantity}";
    
    public void SetLevelConfig(LevelConfig levelConfig) {
        _levelConfig = levelConfig;

        MaxQuantity = _levelConfig.SheepCount;
    }

    public void SetSwipeResult(bool value) {
        var count = value ? _trueSwipeCount++ : _falseSwipeCount++;
        _trueSwipePercentage = (float)_trueSwipeCount / (_trueSwipeCount + _falseSwipeCount);

        GetStarsCount();
    }

    public void SetQTEEventResult(bool value) {
        var count = value ? _jumpCount++ : _strikeCount++;
        ScoreChanged?.Invoke();
    }

    public void Reset() {
        _strikeCount = 0;
        _jumpCount = 0;
    }

    private void GetStarsCount() {
        const float ZeroStarsLimit = 0f;
        const float OneStarLimit = 0.3f;
        const float TwoStarsLimit = 0.9f;

        if (_trueSwipePercentage == ZeroStarsLimit) {
            StarsCount = 0;
            return;
        }

        if (_trueSwipePercentage > ZeroStarsLimit && _trueSwipePercentage <= OneStarLimit) {
            StarsCount = 1;
            return;
        }

        if (_trueSwipePercentage > OneStarLimit && _trueSwipePercentage <= TwoStarsLimit) {
            StarsCount = 2;
            return;
        }

        if (_trueSwipePercentage >= TwoStarsLimit) {
            StarsCount = 3;
            return;
        }
    }
}
