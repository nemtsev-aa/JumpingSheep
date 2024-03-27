using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SoundConfigs), menuName = "Configs/" + nameof(SoundConfigs))]
public class SoundConfigs : ScriptableObject {
    public event Action<float> VolumeChanged;

    [SerializeField] public float _volume;
    
    public float Volume => _volume;

    public void SetVolume(float value) {
        _volume = value;

        VolumeChanged?.Invoke(_volume);
    }
    
}
