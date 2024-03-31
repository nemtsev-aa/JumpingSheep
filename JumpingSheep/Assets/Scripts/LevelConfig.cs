using UnityEngine;
using System;

[Serializable]
public class LevelConfig {
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public int SheepCount { get; private set; }
    [field: SerializeField] public SheepColor Color { get; private set; }
    [field: SerializeField] public QTESystemConfig QTEConfig { get; private set; }
}
