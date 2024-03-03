using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SheepCountPanel : UIPanel {
    [SerializeField] private GameplayConfig _configs;

    [SerializeField] private Slider _amountSlider;
    [SerializeField] private TextMeshProUGUI _labelText;

    public void Init() {
        _labelText.text = $"{_configs.SheepCount}";
        _amountSlider.value = _configs.SheepCount;

        AddListeners();
    }

    public override void AddListeners() {
        base.AddListeners();

        _amountSlider.onValueChanged.AddListener(AmountValueChanged);
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _amountSlider.onValueChanged.RemoveListener(AmountValueChanged);
    }

    private void AmountValueChanged(float value) {
        _labelText.text = $"{value}";
        _configs.SheepCount = Mathf.RoundToInt(value);
    }
}
