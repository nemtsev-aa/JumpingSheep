using UnityEngine;
using System;

[Serializable]
public class LevelProgressData {
     public LevelProgressData(LevelStatusTypes status, string name, int starsCount) {
        Status = status;
        Name = name;
        StarsCount = starsCount;
    }

    [field: SerializeField] public LevelStatusTypes Status { get; set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int StarsCount { get; private set; }

    public void SetStatus(LevelStatusTypes status) => Status = status;

    public void SetStarsCount(int count) => StarsCount = count;
}
