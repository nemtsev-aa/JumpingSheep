using System;
using System.Linq;
using Zenject;

public class LevelSelectionDialog : Dialog {
    public static event Action<LevelConfig> LevelStarted;

    private LevelConfigs _configs;
    private PlayerProgressManager _playerProgressManager;
    private Logger _logger;
    private UICompanentsFactory _factory;

    private LoadingPlayerProgressPanel _loadingProgressPanel;
    private LevelSelectionPanel _levelSelectionPanel;

    [Inject]
    public void Construct(Logger logger, UICompanentsFactory factory, LevelConfigs configs, PlayerProgressManager playerProgressManager) {
        _logger = logger;
        _factory = factory;
        _configs = configs;
        _playerProgressManager = playerProgressManager;
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value) {
            _loadingProgressPanel.Show(true);
            UpdateLevelSelectionPanel();
        }         
    }

    public override void InitializationPanels() {
        _loadingProgressPanel = GetPanelByType<LoadingPlayerProgressPanel>();
        _loadingProgressPanel.Init();

        _levelSelectionPanel = GetPanelByType<LevelSelectionPanel>();
        _levelSelectionPanel.Init(_logger, _playerProgressManager, _factory);
        _levelSelectionPanel.Show(false);

        _logger.Log("LevelSelectionDialog: InitializationPanels completed");
    }

    public override void AddListeners() {
        base.AddListeners();

        _levelSelectionPanel.LevelSelected += OnLevelSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _levelSelectionPanel.LevelSelected -= OnLevelSelected;
    }

    private async void UpdateLevelSelectionPanel() {      
        await _levelSelectionPanel.ShowCurrentStatuses();

        _loadingProgressPanel.Show(false);
        _levelSelectionPanel.Show(true);

        _logger.Log("LevelSelectionDialog: UpdateLevelSelectionPanel completed");
    }
    
    private void OnLevelSelected(int levelIndex) {
        LevelConfig config = _configs.Configs.FirstOrDefault(config => config.Index == levelIndex);
        LevelStarted?.Invoke(config);
    } 
    
    public override void ResetPanels() {
        base.ResetPanels();
    }
}
