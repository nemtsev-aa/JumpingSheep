using UnityEngine;

[CreateAssetMenu(fileName = nameof(SoundConfigs), menuName = "Configs/" + nameof(SoundConfigs))]
public class SoundConfigs : ScriptableObject {
    [field: SerializeField] public float Volume { get; set; }
}
