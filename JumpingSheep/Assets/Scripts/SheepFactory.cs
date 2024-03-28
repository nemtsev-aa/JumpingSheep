using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public enum SheepColor {
    White,
    Gray,
    Red
}

public class SheepFactory {
    private readonly DiContainer _container;
    private readonly Transform _spawnPointPrefab;
    private Transform _spawnPoint;

    private readonly List<SheepData> _sheepDataList;

    public SheepFactory(DiContainer container, SheepPrefabs sheepPrefabs, Transform spawnPoint) {
        _container = container;
        _spawnPointPrefab = spawnPoint;

        _sheepDataList = new List<SheepData>();
        _sheepDataList.AddRange(sheepPrefabs.DataList);
    }

    public Sheep Get(SheepColor color) {
        if (_spawnPoint == null)
            CreateSpawnPoint();

        var position = _spawnPoint.position;
        var rotation = _spawnPoint.rotation;

        return _container.InstantiatePrefabForComponent<Sheep>(GetSheepPrefabByColor(color), position, rotation, _spawnPoint);
    }

    private void CreateSpawnPoint() => _spawnPoint = _container.InstantiatePrefabForComponent<Transform>(_spawnPointPrefab);
    
    private Sheep GetSheepPrefabByColor(SheepColor color) {
        return _sheepDataList.First(data => data.Color == color).Prefab;
    }
}
