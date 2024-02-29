using System;

public class GameScoreCounter {
    public event Action<int> StrikeCountChanged;
    public event Action<int> JumpCountChanged;
    public event Action<float> ScoreChanged;

    public event Action SheepIsOver;

    private int _strikeCount;
    private int _jumpCount;

    private int _maxSheepCount;
    private float _score;

    public string Score => $"{_jumpCount}/{_maxSheepCount}";

    public GameScoreCounter(int maxSheepCount) {
        _maxSheepCount = maxSheepCount;
    }

    public void AddStrike() {
        _strikeCount++;
        StrikeCountChanged?.Invoke(_strikeCount);

        SetScore();
    }

    public void AddJump() {
        _jumpCount++;
        JumpCountChanged?.Invoke(_jumpCount);
        ScoreChanged?.Invoke(_score);

        SetScore();
    }

    public void Reset() {
        _strikeCount = 0;
        _jumpCount = 0;
        _score = 0;
    }

    private void SetScore() {
        _score = (_jumpCount / _maxSheepCount) * 100;

        ScoreChanged?.Invoke(_score);

        if (_strikeCount + _jumpCount >= _maxSheepCount)
            SheepIsOver?.Invoke();
    }
}
