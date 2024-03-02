using System;

public class SheepQuantityCounter {
    public event Action<int> StrikeCountChanged;
    public event Action<int> JumpCountChanged;
    public event Action<int> RemainingQuantityChanged;

    public event Action SheepIsOver;

    private int _strikeCount;
    private int _jumpCount;

    private int _remainingQuantity;

    public int MaxCount { get; private set; }
    public string Result => $"{_jumpCount}/{MaxCount}";

    public SheepQuantityCounter(int maxSheepCount) {
        MaxCount = maxSheepCount;
        _remainingQuantity = maxSheepCount;
    }

    public void AddStrike() {
        _strikeCount++;
        StrikeCountChanged?.Invoke(_strikeCount);

        SetScore();
    }

    public void AddJump() {
        _jumpCount++;
        JumpCountChanged?.Invoke(_jumpCount);

        SetScore();
    }

    public void Reset() {
        _strikeCount = 0;
        _jumpCount = 0;
        _remainingQuantity = MaxCount;
    }

    private void SetScore() {
        _remainingQuantity--;
        RemainingQuantityChanged?.Invoke(_remainingQuantity);

        if (_remainingQuantity <= 0)
            SheepIsOver?.Invoke();
    }
}
