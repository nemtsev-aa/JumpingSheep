using System;
using UnityEngine;

public class SheepSpawner : IPause {
    public event Action<Sheep> SheepCreated;

    private Transform _spawnPoint;
    private SheepFactory _factory;
    private bool _isPaused;

    public SheepSpawner(PauseHandler pauseHandler, SheepFactory factory, Transform spawnPoint) {
        pauseHandler.Add(this);

        _factory = factory;
        _spawnPoint = spawnPoint;
    }

    public void CreateSheep(SheepColor color) {
        if (_isPaused == false) {
            Sheep sheep = _factory.Get(_spawnPoint, color);
            SheepCreated?.Invoke(sheep);
        }
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;
}
