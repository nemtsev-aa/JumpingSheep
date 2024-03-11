using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelConfigs), menuName = "Configs/" + nameof(LevelConfigs))]
public class LevelConfigs : ScriptableObject {
    [field: SerializeField] public List<LevelConfig> Configs;
}
