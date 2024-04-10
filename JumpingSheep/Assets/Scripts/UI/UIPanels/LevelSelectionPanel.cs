using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectionPanel : UIPanel {
    public event Action<int> LevelSelected;

    [SerializeField] private RectTransform _levelSelectionViewParent;

    private Logger _logger;
    private UICompanentsFactory _factory;
    private PlayerProgressManager _progressManager;

    private IReadOnlyList<LevelProgressData> _progressList => _progressManager.GetLevelProgress();
    private List<LevelStatusView> _views;
    private bool _isInit;

    public async UniTask Init(Logger logger, PlayerProgressManager progressManager, UICompanentsFactory factory) {
        _logger = logger;
        _progressManager = progressManager;
        _factory = factory;

        await PreparationPanel();
        
        _isInit = true;
    }

    public async UniTask ShowCurrentStatuses() {
        if (_isInit == false)
            return;

        foreach (var iProgress in _progressList) {
            LevelStatusView view = _views.First(config => config.Name == iProgress.Index.ToString());

            if (iProgress.Status != view.Status)
                view.SetStatus(iProgress.Status);
        }

        await UniTask.Yield();
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

    private async UniTask PreparationPanel() {
        await CreateLevelStatusViews();
        await LevelStatusViewsInit();
    }

    private async UniTask CreateLevelStatusViews() {
        _logger.Log($"LevelSelectionPanel: CreateLevelStatusViews started");

        _views = new List<LevelStatusView>();

        for (int i = 0; i < _progressList.Count; i++) {
            LevelStatusViewConfig newConfig = new LevelStatusViewConfig(_progressList[i].Index.ToString());
            LevelStatusView newLevelView = _factory.Get<LevelStatusView>(newConfig, _levelSelectionViewParent);

            _views.Add(newLevelView);

            await UniTask.Delay(1);
        }

        _logger.Log($"LevelSelectionPanel: CreateLevelStatusViews finished");
    }

    private async UniTask LevelStatusViewsInit() {
        _logger.Log($"LevelSelectionPanel: LevelStatusViewsInit started");

        for (int i = 0; i < _progressList.Count; i++) {
            LevelProgressData levelProgress = _progressList[i];

            LevelStatusViewConfig newConfig = new LevelStatusViewConfig(levelProgress.Index.ToString(), levelProgress.Status, levelProgress.StarsCount);
            LevelStatusView newLevelView = _views[i];

            newLevelView.Init(newConfig);
            newLevelView.Selected += LevelViewSelected;

            await UniTask.Delay(1);
        }

        _logger.Log($"LevelSelectionPanel: LevelStatusViewsInit finished");
    }

    private void LevelViewSelected(string name) {
        LevelProgressData data = _progressList.First(config => config.Index.ToString() == name);

        LevelSelected?.Invoke(data.Index);
    }
}
