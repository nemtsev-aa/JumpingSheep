using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectionPanel : UIPanel {
    public event Action<int> LevelSelected;

    [SerializeField] private RectTransform _levelSelectionViewParent;

    private UICompanentsFactory _factory;
    private PlayerProgressManager _progressManager;

    private IReadOnlyList<LevelProgressData> _progressList => _progressManager.LevelProgress;
    private List<LevelStatusView> _views;

    public void Init(PlayerProgressManager progressManager, UICompanentsFactory factory) {
        _progressManager = progressManager;
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

        foreach (var iProgress in _progressList) {
            LevelStatusViewConfig newConfig = new LevelStatusViewConfig(iProgress.Index.ToString(), iProgress.Status, iProgress.StarsCount);
            LevelStatusView newLevelView = _factory.Get<LevelStatusView>(newConfig, _levelSelectionViewParent);

            newLevelView.Init(newConfig);
            newLevelView.Selected += LevelViewSelected;

            _views.Add(newLevelView);
        }
    }

    public void ShowCurrentStatuses() {
        foreach (var iProgress in _progressList) {
            LevelStatusView view = _views.First(config => config.Name == iProgress.Index.ToString());

            if (iProgress.Status != view.Status)
                view.SetStatus(iProgress.Status);
        }
    }

    private void LevelViewSelected(string name) {
        LevelProgressData data = _progressList.First(config => config.Index.ToString() == name);

        LevelSelected?.Invoke(data.Index);
    }
}
