using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameplayConfig), menuName = "Configs/" + nameof(GameplayConfig))]
public class GameplayConfig : ScriptableObject {
    [field: SerializeField] public int SheepCount { get; set; } = 3;
}
