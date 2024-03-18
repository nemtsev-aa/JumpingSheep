public class SettingsDialog : Dialog {
    private VolumePanel _volumePanel;

    public override void Init() {
        base.Init();
    }

    public override void InitializationPanels() {
        _volumePanel = GetPanelByType<VolumePanel>();
        _volumePanel.Init();
    }
}
