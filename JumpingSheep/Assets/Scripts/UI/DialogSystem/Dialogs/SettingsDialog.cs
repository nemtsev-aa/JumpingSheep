using System;

public class SettingsDialog : Dialog {
    public event Action ResetClicked;

    private VolumePanel _volumePanel;
    private ResetProgressPanel _resetProgressPanel;

    public override void Init() {
        base.Init();
    }

    public override void InitializationPanels() {
        _volumePanel = GetPanelByType<VolumePanel>();
        _volumePanel.Init();

        _resetProgressPanel = GetPanelByType<ResetProgressPanel>();
        _resetProgressPanel.Init();
    }

    public override void AddListeners() {
        base.AddListeners();

        _resetProgressPanel.ResetButtonClicked += OnResetButtonClicked;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _resetProgressPanel.ResetButtonClicked -= OnResetButtonClicked;
    }

    private void OnResetButtonClicked() => ResetClicked?.Invoke();
}
