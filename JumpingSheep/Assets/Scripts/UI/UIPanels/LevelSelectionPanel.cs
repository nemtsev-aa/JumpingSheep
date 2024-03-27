using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LevelSelectionPanel : UIPanel {
    public event Action<LevelConfig> LevelSelected;

    [SerializeField] private RectTransform _levelSelectionViewParent;

    private UICompanentsFactory _factory;
    private List<LevelConfig> _configs;
    private List<LevelStatusView> _views;

    public void Init(LevelConfigs configs, UICompanentsFactory factory) {
        _configs = configs.Configs;
        _factory = factory;

        CreateLevelStatusViews();
    }

    public override void Reset() {
        base.Reset();
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        foreach (var iView in _views) {
            iView.Selected -= LevelViewSelected;
        }
    }

    private void CreateLevelStatusViews() {
        _views = new List<LevelStatusView>();

        foreach (var iConfig in _configs) {
            LevelProgressData data = iConfig.Progress;
            LevelStatusViewConfig newConfig = new LevelStatusViewConfig(data.Name, data.Status, data.StarsCount);
            LevelStatusView newLevelView = _factory.Get<LevelStatusView>(newConfig, _levelSelectionViewParent);

            newLevelView.Init(newConfig);
            newLevelView.Selected += LevelViewSelected;

            _views.Add(newLevelView);
        }
    }

    public void ShowCurrentStatuses() {
        foreach (var iConfig in _configs) {
            LevelStatusView view = _views.First(config => config.Name == iConfig.Progress.Name);

            if (iConfig.Progress.Status != view.Status)
                view.SetStatus(iConfig.Progress.Status);
        }
    }

    private void LevelViewSelected(string name) {
        LevelConfig levelConfig = _configs.First(config => config.Progress.Name == name);
        LevelStatusView view = _views.First(config => config.Name == name);

        LevelSelected?.Invoke(levelConfig);
    }
}
