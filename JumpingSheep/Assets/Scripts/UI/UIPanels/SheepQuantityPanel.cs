using System;
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
        _sheepIconQuantity = _counter.MaxCount;

        AddListeners();
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value) {

            if (_sheepIcons != null && _sheepIcons.Count == _counter.MaxCount)
                return;

            ClearSheepIcons();
            _sheepIconQuantity = _counter.MaxCount;
            CreateIcons();
        }
    }

    public override void Reset() {
        Show(false);

        if (_sheepIcons != null && _sheepIconQuantity == _counter.MaxCount)
            ShowSheepIcons();
        else
            ClearSheepIcons(); 
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

    private void ClearSheepIcons() {
        if (_sheepIcons == null)
            return;

        foreach (var iIcon in _sheepIcons) {
            Destroy(iIcon.gameObject);
        }
    }

    private void ShowSheepIcons() {
        foreach (var iIcon in _sheepIcons) {
            iIcon.Show(true);
        }
    }
}
