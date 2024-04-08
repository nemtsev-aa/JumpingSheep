using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProgressData {

    public PlayerProgressData(List<LevelProgressData> levelProgressDatas) {
        LevelProgressDatas = new List<LevelProgressData>();
        LevelProgressDatas.AddRange(levelProgressDatas);
    }
    
    public List<LevelProgressData> LevelProgressDatas { get; private set; }
}
