using System;
using Zenject;

public class LevelSelectionDialog : Dialog {
    public event Action<LevelConfig> LevelStarted;

    private LevelConfigs _configs;
    private UICompanentsFactory _factory;

    private LevelSelectionPanel _levelSelectionPanel;

    [Inject]
    public void Construct(UICompanentsFactory factory, LevelConfigs configs) {
        _factory = factory;
        _configs = configs;
    }

    public override void Init() {
        base.Init();
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value)
            UpdateLevelSelectionPanel();

    }

    public override void InitializationPanels() {
        _levelSelectionPanel = GetPanelByType<LevelSelectionPanel>();
        _levelSelectionPanel.Init(_configs, _factory);
    }

    public override void AddListeners() {
        base.AddListeners();

        _levelSelectionPanel.LevelSelected += OnLevelSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _levelSelectionPanel.LevelSelected -= OnLevelSelected;
    }

    private void UpdateLevelSelectionPanel() => _levelSelectionPanel.ShowCurrentStatuses();
   
    private void OnLevelSelected(LevelConfig config) => LevelStarted?.Invoke(config);
    

    public override void ResetPanels() {
        base.ResetPanels();
    }
}
