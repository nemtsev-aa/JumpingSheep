using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SheepQuantityPanel : UIPanel {
    private const string LevelName = "Уровень ";

    [SerializeField] private TextMeshProUGUI _levelNameLabel;
    [SerializeField] private RectTransform _sheepIconParent;

    private List<SheepIcon> _sheepIcons;

    private SheepQuantityCounter _counter;
    private UICompanentsFactory _factory;
    private int _sheepIconQuantity;

    public void Init(SheepQuantityCounter counter, UICompanentsFactory factory) {
        _counter = counter;
        _factory = factory;

        _sheepIconQuantity = _counter.MaxQuantity;

        AddListeners();
    }

    public override void Show(bool value) {
        base.Show(value);

        if (value) {

            if (_sheepIcons != null && _sheepIcons.Count == _counter.MaxQuantity)
                return;

            ClearSheepIcons();
            _sheepIconQuantity = _counter.MaxQuantity;

            CreateIcons();
        }
    }

    public override void Reset() {
        Show(false);

        if (_sheepIcons != null && _sheepIconQuantity == _counter.MaxQuantity)
            ShowSheepIcons();
        else
            ClearSheepIcons();
    }

    public override void AddListeners() {
        base.AddListeners();

        _counter.LevelNameChanged += OnLevelNameChanged;
        _counter.RemainingQuantityChanged += OnRemainingQuantityChanged;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _counter.LevelNameChanged -= OnLevelNameChanged;
        _counter.RemainingQuantityChanged -= OnRemainingQuantityChanged;
    }

    private void ShowSheepIcons() {
        foreach (var iIcon in _sheepIcons) {
            iIcon.Show(true);
        }
    }

    private void ClearSheepIcons() {
        if (_sheepIcons == null)
            return;

        foreach (var iIcon in _sheepIcons) {
            Destroy(iIcon.gameObject);
        }
    }

    private void CreateIcons() {
        _sheepIcons = new List<SheepIcon>();

        for (int i = 0; i < _sheepIconQuantity; i++) {
            SheepIconConfig config = new SheepIconConfig();
            
            SheepIcon newSheepIcon = _factory.Get<SheepIcon>(config, _sheepIconParent);
            newSheepIcon.Show(true);

            _sheepIcons.Add(newSheepIcon);
        }
    }

    private void OnLevelNameChanged(string name) => _levelNameLabel.text = $"{LevelName} {name}";

    private void OnRemainingQuantityChanged(int value) => _sheepIcons[value].Show(false);
    
}
