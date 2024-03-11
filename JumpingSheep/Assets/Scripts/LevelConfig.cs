using UnityEngine;
using System;

[Serializable]
public class LevelConfig {
    [field: SerializeField] public LevelStatusTypes Status { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int SheepCount { get; private set; }
    [field: SerializeField] public int QTEEventCount { get; private set; }
    [field: SerializeField] public float QTEEventDuration { get; private set; }
    [field: SerializeField] public int StarsCount { get; private set; } 
}
