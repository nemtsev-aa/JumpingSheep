using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelConfigs), menuName = "Configs/" + nameof(LevelConfigs))]
public class LevelConfigs : ScriptableObject {
    [field: SerializeField] public List<LevelConfig> Configs;

    public List<LevelProgressData> Progress {
        get {
            var progress = new List<LevelProgressData>();
            
            foreach (var iConfig in Configs) {
                progress.Add(iConfig.Progress);
            }

            return progress;
        }
    }

    public void ResetProgress() {
        PlayerPrefs.DeleteAll();

        foreach (var iConfig in Configs) {
            iConfig.Progress.SetStatus(LevelStatusTypes.Locked);
            iConfig.Progress.SetStarsCount(0);
        }

        Configs[0].Progress.SetStatus(LevelStatusTypes.Ready);
        Configs[0].Progress.SetStarsCount(0);
    }

    public void UpdateProgress(List<LevelProgressData> levelProgressDatas) {
        var progress = levelProgressDatas;

        foreach (var iLevelProgress in levelProgressDatas) {
            LevelConfig iConfig = Configs.First(config => config.Progress.Name == iLevelProgress.Name);

            iConfig.Progress.SetStatus(iLevelProgress.Status);
            iConfig.Progress.SetStarsCount(iLevelProgress.StarsCount);
        }
    }
}
