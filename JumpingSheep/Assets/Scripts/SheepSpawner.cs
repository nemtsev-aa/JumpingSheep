using System;

public class SheepSpawner : IPause {
    public event Action<Sheep> SheepCreated;

    private SheepFactory _factory;
    private bool _isPaused;

    public SheepSpawner(PauseHandler pauseHandler, SheepFactory factory) {
        pauseHandler.Add(this);

        _factory = factory;
    }

    public void CreateSheep(SheepColor color) {
        if (_isPaused == false) {
            Sheep sheep = _factory.Get(color);
            SheepCreated?.Invoke(sheep);
        }
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;
}
