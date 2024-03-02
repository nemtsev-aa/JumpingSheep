using System.Collections.Generic;
using UnityEngine;

public class SheepQuantityPanel : UIPanel {
    [SerializeField] private SheepIcon _sheepIconPrefab;
    [SerializeField] private RectTransform _sheepIconParent;

    private List<SheepIcon> _sheepIcons;

    private SheepQuantityCounter _counter;
    private int _sheepIconQuantity;

    public void Init(SheepQuantityCounter counter) {
        _counter = counter;
        _sheepIconQuantity = counter.MaxCount;

        CreateIcons();
        AddListeners();
    }

    public override void Reset() {
        Show(false);

        if (_sheepIcons != null)
            ShowSheepIcons();
    }

    public override void AddListeners() {
        base.AddListeners();

        _counter.RemainingQuantityChanged += OnRemainingQuantityChanged;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _counter.RemainingQuantityChanged -= OnRemainingQuantityChanged;
    }

    private void CreateIcons() {
        _sheepIcons = new List<SheepIcon>();

        for (int i = 0; i < _sheepIconQuantity; i++) {
            SheepIcon newSheepIcon = Instantiate(_sheepIconPrefab, _sheepIconParent);
            newSheepIcon.Show(true);

            _sheepIcons.Add(newSheepIcon);
        }
    }

    private void OnRemainingQuantityChanged(int value) {
        _sheepIcons[value].Show(false);
    }

    private void ShowSheepIcons() {
        foreach (var iIcon in _sheepIcons) {
            iIcon.Show(true);
        }
    }
}
