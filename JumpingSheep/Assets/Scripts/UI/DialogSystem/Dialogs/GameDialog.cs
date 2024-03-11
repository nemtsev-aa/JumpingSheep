using System;
using UnityEngine;
using Zenject;

public class GameDialog : Dialog {
    public event Action PlayClicked;
    public event Action ResetClicked;
    public event Action MainMenuClicked;

    [SerializeField] private QTESystem _qTESystem;
    private SheepQuantityCounter _counter;

    private LearningPanel LearningPanel => GetPanelByType<LearningPanel>();
    private SheepQuantityPanel SheepQuantityPanel => GetPanelByType<SheepQuantityPanel>();
    private ResultPanel ResultPanel => GetPanelByType<ResultPanel>();
    private InnerGlowPanel InnerGlowPanel => GetPanelByType<InnerGlowPanel>();
    public QTESystem QTESystem => _qTESystem;

    [Inject]
    public void Constuct(QTEEventConfigs qTEEventConfigs, SwipeHandler swipeHandler) {
        _qTESystem.Init(qTEEventConfigs.Configs, swipeHandler);
    }

    public void SetSheepCounter(SheepQuantityCounter sheepCounter) {
        _counter = sheepCounter;
        
        ResultPanel.Init(_counter);
        SheepQuantityPanel.Init(_counter);

        _counter.SheepIsOver += OnSheepIsOver;
    }

    public override void AddListeners() {
        LearningPanel.PlayClicked += OnPlayClicked;
        LearningPanel.MainMenuClicked += OnMainMenuClick;

        ResultPanel.ResetClicked += OnResetClicked;
        ResultPanel.MainMenuClick += OnMainMenuClick;

        _qTESystem.Started += OnQTESystemStarted;
        _qTESystem.Completed += OnQTESystemFinished;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _counter.SheepIsOver -= OnSheepIsOver;

        LearningPanel.PlayClicked -= OnPlayClicked;
        LearningPanel.MainMenuClicked -= OnMainMenuClick;

        ResultPanel.ResetClicked -= OnResetClicked;
        ResultPanel.MainMenuClick -= OnMainMenuClick;

        _qTESystem.Started -= OnQTESystemStarted;
        _qTESystem.Completed -= OnQTESystemFinished;
    }

    public override void ResetPanels() {
        LearningPanel.Reset();
        ResultPanel.Reset();
        InnerGlowPanel.Reset();
        SheepQuantityPanel.Reset();
    }

    public override void InitializationPanels() {
        LearningPanel.Show(true);
        LearningPanel.Init();

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
    }

    private void OnQTESystemFinished(bool value) {
        ShowInnerGlowPanel(false);
    }

    private void ShowInnerGlowPanel(bool panelStatus) {
        InnerGlowPanel.gameObject.SetActive(panelStatus);

        if (panelStatus) 
            InnerGlowPanel.StartedAnimation();
        else 
            InnerGlowPanel.FinishedAnimation();
    }
}
