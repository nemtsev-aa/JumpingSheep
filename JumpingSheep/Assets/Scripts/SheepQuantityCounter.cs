using System;

public class SheepQuantityCounter {
    public event Action<string> LevelNameChanged;
    public event Action<int> RemainingQuantityChanged;
    public event Action SheepIsOver;

    private LevelConfig _levelConfig;
    private int _remainingQuantity;

    public SheepQuantityCounter() {
        
    }

    public int MaxQuantity { get; private set; }

    public void SetLevelConfig(LevelConfig levelConfig) {
        _levelConfig = levelConfig;

        MaxQuantity = _levelConfig.SheepCount;
        _remainingQuantity = MaxQuantity;

        LevelNameChanged?.Invoke(_levelConfig.Progress.Name);
    }

    public void TakeSheep() {
        _remainingQuantity--;
        RemainingQuantityChanged?.Invoke(_remainingQuantity);

        if (_remainingQuantity <= 0)
            SheepIsOver?.Invoke();
    }

    public void Reset() {
        _remainingQuantity = MaxQuantity;
    }

}
