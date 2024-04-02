using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LevelSelectionPanel : UIPanel {
    public event Action<int> LevelSelected;

    [SerializeField] private RectTransform _levelSelectionViewParent;

    private UICompanentsFactory _factory;
    private Logger _logger;
    private PlayerProgressManager _progressManager;

    private IReadOnlyList<LevelProgressData> _progressList => _progressManager.LevelProgress;
    private List<LevelStatusView> _views;

    public void Init(Logger logger, PlayerProgressManager progressManager, UICompanentsFactory factory) {
        _logger = logger;
        _progressManager = progressManager;
        _factory = factory;

        StartCoroutine(CreateLevelStatusViews());
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

    private IEnumerator CreateLevelStatusViews() {
        _logger.Log($"LevelSelectionPanel: CreateLevelStatusViews started");

        _views = new List<LevelStatusView>();

        foreach (var iProgress in _progressList) {
            LevelStatusViewConfig newConfig = new LevelStatusViewConfig(iProgress.Index.ToString(), iProgress.Status, iProgress.StarsCount);
            LevelStatusView newLevelView = _factory.Get<LevelStatusView>(newConfig, _levelSelectionViewParent);

            newLevelView.Init(newConfig);
            newLevelView.Selected += LevelViewSelected;

            _views.Add(newLevelView);
        }

        _logger.Log($"LevelSelectionPanel: CreateLevelStatusViews finished");

        yield return new WaitForSeconds(0.1f);
    }

    public async Task ShowCurrentStatuses() {
        foreach (var iProgress in _progressList) {
            LevelStatusView view = _views.First(config => config.Name == iProgress.Index.ToString());

            if (iProgress.Status != view.Status)
                view.SetStatus(iProgress.Status);
        }

        await Task.Yield();
    }

    private void LevelViewSelected(string name) {
        LevelProgressData data = _progressList.First(config => config.Index.ToString() == name);

        LevelSelected?.Invoke(data.Index);
    }
}
