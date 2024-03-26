using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData {

    public PlayerData(List<LevelProgressData> levelProgressDatas) {
        LevelProgressDatas = levelProgressDatas;
    }

    public List<LevelProgressData> LevelProgressDatas { get; private set; }

}
