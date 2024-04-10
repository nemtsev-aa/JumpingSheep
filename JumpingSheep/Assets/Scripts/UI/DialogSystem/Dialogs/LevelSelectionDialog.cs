using System;
using System.Linq;
using Zenject;

public class LevelSelectionDialog : Dialog {
    public static event Action<LevelConfig> LevelStarted;

    private Logger _logger;
    private LevelConfigs _configs;
    private UICompanentsFactory _factory;
    private PlayerProgressManager _playerProgressManager;

    private LevelSelectionPanel _levelSelectionPanel;
    private LoadingPlayerProgressPanel _loadingProgressPanel;

    [Inject]
    public void Construct(Logger logger, UICompanentsFactory factory, LevelConfigs configs, PlayerProgressManager playerProgressManager) {
        _logger = logger;
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
        _loadingProgressPanel = GetPanelByType<LoadingPlayerProgressPanel>();
        _loadingProgressPanel.Init();

        LevelSelectionPanelInit();
 
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

    public override void ResetPanels() {
        base.ResetPanels();
    }

    private async void LevelSelectionPanelInit() {
        _levelSelectionPanel = GetPanelByType<LevelSelectionPanel>();
        _levelSelectionPanel.Show(false);
        
        await _levelSelectionPanel.Init(_logger, _playerProgressManager, _factory);
        
        _loadingProgressPanel.Show(false);
        _levelSelectionPanel.Show(true);
    }

    private async void UpdateLevelSelectionPanel() {
        _loadingProgressPanel.Show(true);

        await _levelSelectionPanel.ShowCurrentStatuses();

        _loadingProgressPanel.Show(false);

        _logger.Log("LevelSelectionDialog: UpdateLevelSelectionPanel completed");
    }
    
    private void OnLevelSelected(int levelIndex) {
        LevelConfig config = _configs.Configs.FirstOrDefault(config => config.Index == levelIndex);
        LevelStarted?.Invoke(config);
    } 
   
}
