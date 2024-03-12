using System.IO;
using UnityEngine;
using Zenject;
using System;

public enum SheepColor {
    White,
    Gray,
    Red
}

public class SheepFactory {
    private const string WhiteSheep = "WhiteSheep";
    private const string GraySheep = "GraySheep";
    private const string RedSheep = "RedSheep";

    private const string ConfigsPath = "Prefabs/Sheeps";

    private Sheep _white, _gray, _red;

    private DiContainer _container;

    public SheepFactory(DiContainer container) {
        _container = container;
        Load();
    }

    public Sheep Get(Transform spawnPoint, SheepColor color) {
        Sheep prefab = GetPrefabByColor(color);
        Sheep newSheep = _container.InstantiatePrefabForComponent<Sheep>(prefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        return newSheep;
    }

    private void Load() {
        _white = Resources.Load<Sheep>(Path.Combine(ConfigsPath, WhiteSheep));
        _gray = Resources.Load<Sheep>(Path.Combine(ConfigsPath, GraySheep));
        _red = Resources.Load<Sheep>(Path.Combine(ConfigsPath, RedSheep));
    }

    private Sheep GetPrefabByColor(SheepColor color) {
        switch (color) {
            case SheepColor.White:
                return _white;

            case SheepColor.Gray:
                return _gray;

            case SheepColor.Red:
                return _red;

            default:
                throw new ArgumentException($"Invalid SheepColor is {color}");
        }
    }
}
