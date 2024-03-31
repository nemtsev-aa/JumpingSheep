using System;
using UnityEngine;

[Serializable]
public class LevelProgressData {
    public LevelProgressData(LevelStatusTypes status, int index, int starsCount) {
        Status = status;
        Index = index;
        StarsCount = starsCount;
    }

    [field: SerializeField] public LevelStatusTypes Status { get; set; }
    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public int StarsCount { get; private set; }

    public void SetStatus(LevelStatusTypes status) => Status = status;

    public void SetStarsCount(int count) => StarsCount = count;
}
