using System;
using UnityEngine;

public class SheepSpawner : MonoBehaviour {
    public event Action<Sheep> SheepCreated;

    [SerializeField] private Transform _spawnPoint;


    private SheepFactory _factory;

    private Sheep _currentSheep;

    public void Init(SheepFactory factory) {
        _factory = factory;
    }

    public void CreateSheep() {
        _currentSheep = _factory.Get(_spawnPoint);
        SheepCreated?.Invoke(_currentSheep);
    }

    public void DestroyCurrentSheep() {
        Destroy(_currentSheep.gameObject);
        _currentSheep = null;
    }
}
