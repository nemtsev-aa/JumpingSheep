using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelConfigs), menuName = "Configs/" + nameof(LevelConfigs))]
public class LevelConfigs : ScriptableObject {
    [field: SerializeField] public List<LevelConfig> Configs;

    [ContextMenu(nameof(ResetProgress))]
    public void ResetProgress() {
        foreach (var iConfig in Configs) {
            iConfig.Progress.SetStatus(LevelStatusTypes.Locked);
            iConfig.Progress.SetStarsCount(0);
        }

        Configs[0].Progress.SetStatus(LevelStatusTypes.Ready);
        Configs[0].Progress.SetStarsCount(0);
    }
}
