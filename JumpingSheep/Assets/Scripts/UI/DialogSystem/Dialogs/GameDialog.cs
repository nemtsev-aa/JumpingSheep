using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;

public class GameDialog : Dialog {
    public event Action PlayClicked;
    public event Action LearningClicked;
    public event Action ResetClicked;
    public event Action MainMenuClicked;
    public event Action<bool> PauseClicked;

    [SerializeField] private Button _learningButton;
    [SerializeField] private Button _pauseButton;

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

        _learningButton.onClick.AddListener(LearningButtonClick);
        _pauseButton.onClick.AddListener(PauseButtonClick);

        LearningPanel.PlayClicked += OnPlayClicked;
        LearningPanel.MainMenuClicked += OnMainMenuClick;

        ResultPanel.ResetClicked += OnResetClicked;
        ResultPanel.MainMenuClick += OnMainMenuClick;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _learningButton.onClick.RemoveListener(LearningButtonClick);
        _pauseButton.onClick.RemoveListener(PauseButtonClick);

        _counter.SheepIsOver -= OnSheepIsOver;

        LearningPanel.PlayClicked -= OnPlayClicked;
        LearningPanel.MainMenuClicked -= OnMainMenuClick;

        ResultPanel.ResetClicked -= OnResetClicked;
        ResultPanel.MainMenuClick -= OnMainMenuClick;

        _qTESystem.Started -= OnQTESystemStarted;
        _qTESystem.AllEventsCompleted -= OnQTESystemAllEventsCompleted;
    }

    public override void ResetPanels() {
        LearningPanel.Reset();
        ResultPanel.Reset();
        InnerGlowPanel.Reset();
        SheepQuantityPanel.Reset();
    }

    public override void InitializationPanels() {
        NavigationPanel.Show(true);

        LearningPanel.Init();
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

    private void LearningButtonClick() {
        LearningClicked?.Invoke();
    }

    private void PauseButtonClick() {
        _pauseHandler.SetPause(!IsPaused);
        PauseClicked?.Invoke(IsPaused);
    }

    private void OnQTESystemStarted() {
        ShowInnerGlowPanel(true);
        QTEEventsPanel.Show(true);
    }

    private void OnQTESystemAllEventsCompleted(bool value) {
        ShowInnerGlowPanel(false);
        QTEEventsPanel.Show(false);
    }

    private void ShowInnerGlowPanel(bool panelStatus) {
        InnerGlowPanel.gameObject.SetActive(panelStatus);

        if (panelStatus) 
            InnerGlowPanel.StartedAnimation();
        else 
            InnerGlowPanel.FinishedAnimation();
    }
}
