using System;
using UnityEngine;
using UnityEngine.UI;

public class ResetProgressPanel : UIPanel {
    public event Action ResetButtonClicked;

    [SerializeField] private Button _resetButton;

    public void Init() {
        AddListeners();
    }

    public override void AddListeners() {
        base.AddListeners();

        _resetButton.onClick.AddListener(ResetButtonClick);
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _resetButton.onClick.RemoveListener(ResetButtonClick);
    }

    private void ResetButtonClick() => ResetButtonClicked?.Invoke();
}
