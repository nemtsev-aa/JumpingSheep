using UnityEngine;
using System;

[Serializable]
public class LevelConfig {
    [field: SerializeField] public LevelProgressData Progress { get; private set; }
    [field: SerializeField] public int SheepCount { get; private set; }
    [field: SerializeField] public SheepColor Color { get; private set; }
    [field: SerializeField] public QTESystemConfig QTEConfig { get; private set; }

    public void SetCurrentProgress(LevelProgressData progress) => Progress = progress;
}
