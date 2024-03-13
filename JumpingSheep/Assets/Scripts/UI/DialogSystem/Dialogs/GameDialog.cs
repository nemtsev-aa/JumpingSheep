using System;
using Zenject;

public class GameDialog : Dialog {
    public event Action PlayClicked;
    public event Action MainMenuClicked;
    public event Action ResetClicked;
    public event Action<bool> PauseClicked;
    public event Action LearningClicked;

    private PauseHandler _pauseHandler;
    private UICompanentsFactory _factory;
    private SheepQuantityCounter _counter;
    private QTESystem _qTESystem;
    
    private LearningPanel LearningPanel => GetPanelByType<LearningPanel>();
    private SheepQuantityPanel SheepQuantityPanel => GetPanelByType<SheepQuantityPanel>();
    private QTEEventsPanel QTEEventsPanel => GetPanelByType<QTEEventsPanel>();
    private ResultPanel ResultPanel => GetPanelByType<ResultPanel>();
    private InnerGlowPanel InnerGlowPanel => GetPanelByType<InnerGlowPanel>();
    private NavigationPanel NavigationPanel => GetPanelByType<NavigationPanel>();
    private PausePanel PausePanel => GetPanelByType<PausePanel>();
    
    private bool IsPaused => _pauseHandler.IsPaused;
    
    [Inject]
    public void Constuct(PauseHandler pauseHandler, UICompanentsFactory factory) {
        _pauseHandler = pauseHandler;
        _factory = factory;
    }

    public void SetServices(SheepQuantityCounter sheepCounter, QTESystem qTESystem) {
        _counter = sheepCounter;
        _qTESystem = qTESystem;

        ResultPanel.Init(_counter);
        SheepQuantityPanel.Init(_counter);
        QTEEventsPanel.Init(_qTESystem.Events, _factory);

        _counter.SheepIsOver += OnSheepIsOver;

        _qTESystem.Started += OnQTESystemStarted;
        _qTESystem.AllEventsCompleted += OnQTESystemAllEventsCompleted;
    }

    public override void AddListeners() {
        base.AddListeners();

        NavigationPanel.LearningButtonClicked += OnLearningButtonClicked;
        NavigationPanel.PauseButtonClicked += OnPauseButtonClicked;

        PausePanel.PlayButtonClicked += OnPlayClicked;
        PausePanel.MainMenuButtonClicked += OnMainMenuClick;

        LearningPanel.PlayButtonClicked += OnPlayClicked;
        LearningPanel.MainMenuButtonClicked += OnMainMenuClick;

        ResultPanel.ResetButtonClicked += OnResetClicked;
        ResultPanel.MainMenuButtonClicked += OnMainMenuClick;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _counter.SheepIsOver -= OnSheepIsOver;

        NavigationPanel.LearningButtonClicked -= OnLearningButtonClicked;
        NavigationPanel.PauseButtonClicked -= OnPauseButtonClicked;

        PausePanel.PlayButtonClicked -= OnPlayClicked;
        PausePanel.MainMenuButtonClicked -= OnMainMenuClick;

        LearningPanel.PlayButtonClicked -= OnPlayClicked;
        LearningPanel.MainMenuButtonClicked -= OnMainMenuClick;

        ResultPanel.ResetButtonClicked -= OnResetClicked;
        ResultPanel.MainMenuButtonClicked -= OnMainMenuClick;

        _qTESystem.Started -= OnQTESystemStarted;
        _qTESystem.AllEventsCompleted -= OnQTESystemAllEventsCompleted;
    }

    public override void ResetPanels() {
        NavigationPanel.Reset();
        PausePanel.Reset();
        LearningPanel.Reset();
        ResultPanel.Reset();
        InnerGlowPanel.Reset();
        SheepQuantityPanel.Reset();
    }

    public override void InitializationPanels() {
        NavigationPanel.Init();
        LearningPanel.Init();
        PausePanel.Init();

        NavigationPanel.Show(true);
        PausePanel.Show(false);
        LearningPanel.Show(false);
        QTEEventsPanel.Show(false);
        SheepQuantityPanel.Show(false);
        ResultPanel.Show(false);
        InnerGlowPanel.Show(false);
    }

    private void OnSheepIsOver() {
        ResultPanel.Show(true);
        SheepQuantityPanel.Show(false);
        InnerGlowPanel.Show(false);
    }

    private void OnPlayClicked() {
        PlayClicked?.Invoke();

        LearningPanel.Show(false);
        SheepQuantityPanel.Show(true);        
    }

    private void OnResetClicked() {
        ResetPanels();

        LearningPanel.Show(false);
        SheepQuantityPanel.Show(true);

        ResetClicked?.Invoke();
    }

    private void OnMainMenuClick() {
        ResetPanels();
        MainMenuClicked?.Invoke();
    }

    private void OnQTESystemStarted() {
        ShowInnerGlowPanel(true);
        QTEEventsPanel.Show(true);
    }

    private void OnQTESystemAllEventsCompleted(bool value) {
        ShowInnerGlowPanel(false);
        QTEEventsPanel.Show(false);
    }

    private void OnPauseButtonClicked() {
        NavigationPanel.Show(false);
        PausePanel.Show(true);

        _pauseHandler.SetPause(!IsPaused);
        PauseClicked?.Invoke(IsPaused);
    }

    private void OnLearningButtonClicked() {
        OnPauseButtonClicked();

        NavigationPanel.Show(false);
        LearningPanel.Show(true);
    }

    private void ShowInnerGlowPanel(bool panelStatus) {
        InnerGlowPanel.gameObject.SetActive(panelStatus);

        if (panelStatus) 
            InnerGlowPanel.StartedAnimation();
        else 
            InnerGlowPanel.FinishedAnimation();
    }
}
