using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(VolumeConfig), menuName = "Configs/" + nameof(VolumeConfig))]
public class VolumeConfig : ScriptableObject {
    public event Action<float> VolumeChanged;

    [SerializeField] public float _volume;
    
    public float Volume => _volume;

    public void SetVolume(float value) {
        _volume = value;

        VolumeChanged?.Invoke(_volume);
    }
    
}
