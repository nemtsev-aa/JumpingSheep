using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelConfigs), menuName = "Configs/" + nameof(LevelConfigs))]
public class LevelConfigs : ScriptableObject {
    [field: SerializeField] public List<LevelConfig> Configs;

    [ContextMenu(nameof(ResetProgress))]
    public void ResetProgress() {
        foreach (var iConfig in Configs) {
            iConfig.SetStatus(LevelStatusTypes.Locked);
            iConfig.StarsCount = 0;
        }

        Configs[0].SetStatus(LevelStatusTypes.Ready);
        Configs[0].StarsCount = 0;
    }
}
