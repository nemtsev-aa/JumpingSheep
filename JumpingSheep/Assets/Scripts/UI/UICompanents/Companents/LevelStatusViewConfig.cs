using System;

[Serializable]
public class LevelStatusViewConfig : UICompanentConfig {
    public string Name { get; private set; }
    public LevelStatusTypes CurrentStatus { get; private set; }
    public int StarsCount { get; private set; }

    public LevelStatusViewConfig(string name, LevelStatusTypes status, int starsCount = 0) {
        Name = name;
        CurrentStatus = status;
        StarsCount = starsCount;
    }

    public void SetLevelStatus(LevelStatusTypes status) => CurrentStatus = status;

    public override void OnValidate() {
        
    }
}
