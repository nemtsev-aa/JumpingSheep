using System;
using System.Threading.Tasks;
using Zenject;

public class GameDialog : Dialog {
    public event Action MainMenuClicked;
    public event Action ResetClicked;
    public event Action NextLevelClicked;

    public event Action PlayClicked;
    public event Action<bool> PauseClicked;
    public event Action LearningClicked;

    private PauseHandler _pauseHandler;
    private UICompanentsFactory _factory;
    private SheepQuantityCounter _counter;
    private QTESystem _qTESystem;

    private LearningPanel _learningPanel;
    private SheepQuantityPanel _sheepQuantityPanel;
    private QTEEventsPanel _qTEEventsPanel;
    private ResultPanel _resultPanel;
    private InnerGlowPanel _innerGlowPanel;
    private NavigationPanel _navigationPanel;
    private PausePanel _pausePanel;

    [Inject]
    public void Constuct(PauseHandler pauseHandler, UICompanentsFactory factory, SheepQuantityCounter sheepCounter, QTESystem qTESystem) {
        _pauseHandler = pauseHandler;
        _factory = factory;
        _counter = sheepCounter;
        _qTESystem = qTESystem;

        _learningPanel = GetPanelByType<LearningPanel>();
        _sheepQuantityPanel = GetPanelByType<SheepQuantityPanel>();
        _qTEEventsPanel = GetPanelByType<QTEEventsPanel>();
        _resultPanel = GetPanelByType<ResultPanel>();
        _innerGlowPanel = GetPanelByType<InnerGlowPanel>();
        _navigationPanel = GetPanelByType<NavigationPanel>();
        _pausePanel = GetPanelByType<PausePanel>();

        _resultPanel.Init();
        _sheepQuantityPanel.Init(_counter, _factory);

        _qTEEventsPanel.Init(_qTESystem, _pauseHandler, _factory);
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value == true) {
            _learningPanel.Show(false);
            _sheepQuantityPanel.Show(true);
        }
    }

    public override void AddListeners() {
        base.AddListeners();

        _counter.SheepIsOver += OnSheepIsOver;

        _qTESystem.Started += OnQTESystemStarted;
        _qTESystem.AllEventsCompleted += OnQTESystemAllEventsCompleted;

        SettingsClicked += OnSettingsButtonClicked;

        _navigationPanel.LearningButtonClicked += OnLearningButtonClicked;
        _navigationPanel.PauseButtonClicked += OnPauseButtonClicked;

        _pausePanel.PlayButtonClicked += OnPlayClicked;
        _pausePanel.MainMenuButtonClicked += OnMainMenuClick;

        _learningPanel.PlayButtonClicked += OnPlayClicked;
        _learningPanel.MainMenuButtonClicked += OnMainMenuClick;

        _resultPanel.MainMenuButtonClicked += OnMainMenuClick;
        _resultPanel.ResetButtonClicked += OnResetClicked;
        _resultPanel.NextLevelClicked += OnNextLevelClicked;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();   
        _qTESystem.Started -= OnQTESystemStarted;
        _qTESystem.AllEventsCompleted -= OnQTESystemAllEventsCompleted;

        SettingsClicked -= OnSettingsButtonClicked;

        _navigationPanel.LearningButtonClicked -= OnLearningButtonClicked;
        _navigationPanel.PauseButtonClicked -= OnPauseButtonClicked;
        
        _pausePanel.PlayButtonClicked -= OnPlayClicked;
        _pausePanel.MainMenuButtonClicked -= OnMainMenuClick;
        
        _learningPanel.PlayButtonClicked -= OnPlayClicked;
        _learningPanel.MainMenuButtonClicked -= OnMainMenuClick;
        
        _resultPanel.MainMenuButtonClicked -= OnMainMenuClick;
        _resultPanel.ResetButtonClicked -= OnResetClicked;
        _resultPanel.NextLevelClicked -= OnNextLevelClicked;
    }

    public override void ResetPanels() {
        _navigationPanel.Reset();
        _pausePanel.Reset();
        _learningPanel.Reset();
        _resultPanel.Reset();
        _innerGlowPanel.Reset();
        _sheepQuantityPanel.Reset();
    }

    public override void InitializationPanels() {
        _navigationPanel.Init();
        _learningPanel.Init();
        _pausePanel.Init();

        _navigationPanel.Show(true);
        _pausePanel.Show(false);
        _learningPanel.Show(false);
        _qTEEventsPanel.Show(false);
        _sheepQuantityPanel.Show(false);
        _resultPanel.Show(false);
        _innerGlowPanel.Show(false);
    }

    private void OnSheepIsOver() {
        _resultPanel.Show(true);

        _sheepQuantityPanel.Show(false);
        _innerGlowPanel.Show(false);
        _navigationPanel.Show(false);
    }

    private void OnPlayClicked() {
        _learningPanel.Show(false);
        _pausePanel.Show(false);

        _sheepQuantityPanel.Show(true);
        _navigationPanel.Show(true);

        PauseClicked?.Invoke(false);
    }

    private void OnPauseButtonClicked() {
        _pausePanel.Show(true);

        _navigationPanel.Show(false);
        _sheepQuantityPanel.Show(false);

        PauseClicked?.Invoke(true);
    }

    private void OnMainMenuClick() {
        ResetPanels();
        MainMenuClicked?.Invoke();
    }

    private void OnResetClicked() {
        ResetPanels();

        _learningPanel.Show(false);
        _sheepQuantityPanel.Show(true);

        ResetClicked?.Invoke();
    }

    private void OnNextLevelClicked() {
        NextLevelClicked?.Invoke();
    }

    private void OnQTESystemStarted() {
        _navigationPanel.Show(false);

        ShowInnerGlowPanel(true);

        _qTEEventsPanel.Show(true);
    }

    private void OnQTESystemAllEventsCompleted(bool value) {
        ShowInnerGlowPanel(false);

        HideQTEEventsPanel();
        _navigationPanel.Show(true);
    }

    private async Task HideQTEEventsPanel() {
        await Task.Delay(TimeSpan.FromSeconds(2));
        _qTEEventsPanel.Show(false);
    }

    private void OnLearningButtonClicked() {
        _learningPanel.Show(true);

        _navigationPanel.Show(false);
        _sheepQuantityPanel.Show(false);

        PauseClicked?.Invoke(true);
    }

    private void OnSettingsButtonClicked() {
        _navigationPanel.Show(false);
        _sheepQuantityPanel.Show(false);
        _qTEEventsPanel.Show(false);

        PauseClicked?.Invoke(true);
    }

    private void ShowInnerGlowPanel(bool panelStatus) {
        _innerGlowPanel.Show(panelStatus);

        if (panelStatus)
            _innerGlowPanel.StartedAnimation();
        else 
            _innerGlowPanel.FinishedAnimation();
    }
}
