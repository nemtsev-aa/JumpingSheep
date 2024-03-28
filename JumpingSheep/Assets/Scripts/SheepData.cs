using System;
using UnityEngine;

[Serializable]
public class SheepData {
    [field: SerializeField] public SheepColor Color { get; private set; }
    [field: SerializeField] public Sheep Prefab { get; private set; }
}
