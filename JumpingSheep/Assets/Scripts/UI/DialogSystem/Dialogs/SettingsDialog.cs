public class SettingsDialog : Dialog {
    private SheepCountPanel _sheepCount;
    private QTECountPanel _qteCount;
    private VolumePanel _volumePanel;

    public override void Init(Logger logger) {
        base.Init(logger);
    }

    public override void InitializationPanels() {
        _sheepCount = GetPanelByType<SheepCountPanel>();
        _sheepCount.Init();

        _qteCount = GetPanelByType<QTECountPanel>();
        _qteCount.Init();

        _volumePanel = GetPanelByType<VolumePanel>();
        _volumePanel.Init();
    }
}
