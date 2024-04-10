using System;

[Serializable]
public class LevelStatusViewConfig : UICompanentConfig {
    public string Index { get; private set; }
    public LevelStatusTypes CurrentStatus { get; private set; }
    public int StarsCount { get; private set; }

    public LevelStatusViewConfig(string index) {
        Index = index;
    }

    public LevelStatusViewConfig(string index, LevelStatusTypes status, int starsCount = 0) {
        Index = index;
        CurrentStatus = status;
        StarsCount = starsCount;
    }

    public void SetLevelStatus(LevelStatusTypes status) => CurrentStatus = status;

    public override void OnValidate() {
        
    }
}
