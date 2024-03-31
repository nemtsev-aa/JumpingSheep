using System;
using System.Linq;
using Zenject;

public class LevelSelectionDialog : Dialog {
    public static event Action<LevelConfig> LevelStarted;

    private LevelConfigs _configs;
    private PlayerProgressManager _playerProgressManager;
    private UICompanentsFactory _factory;

    private LevelSelectionPanel _levelSelectionPanel;

    [Inject]
    public void Construct(UICompanentsFactory factory, LevelConfigs configs, PlayerProgressManager playerProgressManager) {
        _factory = factory;
        _configs = configs;
        _playerProgressManager = playerProgressManager;
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value)
            UpdateLevelSelectionPanel();
    }

    public override void InitializationPanels() {
        _levelSelectionPanel = GetPanelByType<LevelSelectionPanel>();
        _levelSelectionPanel.Init(_playerProgressManager, _factory);
    }

    public override void AddListeners() {
        base.AddListeners();

        _levelSelectionPanel.LevelSelected += OnLevelSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _levelSelectionPanel.LevelSelected -= OnLevelSelected;
    }

    private void UpdateLevelSelectionPanel() {
        _levelSelectionPanel.ShowCurrentStatuses();
    }
    

    private void OnLevelSelected(int levelIndex) {
        LevelConfig config = _configs.Configs.FirstOrDefault(config => config.Index == levelIndex);
        LevelStarted?.Invoke(config);
    } 
    
    public override void ResetPanels() {
        base.ResetPanels();
    }
}
