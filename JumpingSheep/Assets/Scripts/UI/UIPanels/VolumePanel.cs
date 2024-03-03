using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumePanel : UIPanel {
    [SerializeField] private SoundConfigs _configs;

    [SerializeField] private Slider _volumeSlideBar;
    [SerializeField] private TextMeshProUGUI _labelText;

    public void Init() {
        _labelText.text = $"{GetVolumeValue(_configs.Volume)}";
        _volumeSlideBar.value = _configs.Volume;

        AddListeners();
    }

    public override void AddListeners() {
        base.AddListeners();

        _volumeSlideBar.onValueChanged.AddListener(VolumeValueChanged);
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _volumeSlideBar.onValueChanged.RemoveListener(VolumeValueChanged);
    }

    private void VolumeValueChanged(float value) {
        _labelText.text = $"{GetVolumeValue(value)}";
        _configs.Volume = value;
    }

    private string GetVolumeValue(float value) {
        float currentValue = Mathf.Clamp(value, 0, 1);
        string valueText = $"{Mathf.RoundToInt(currentValue * 100)} %";
        
        return valueText;
    }
}
