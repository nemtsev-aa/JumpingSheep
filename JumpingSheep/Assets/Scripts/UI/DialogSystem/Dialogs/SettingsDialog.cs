using System;
using UnityEngine;

public class SettingsDialog : Dialog {
    
    public override void Init(Logger logger) {
        base.Init(logger);

    }

    public override void InitializationPanels() {
        //_selectionColor = GetPanelByType<SelectionColorPanel>();
        //_selectionColor.Init(_materialConfig, _companentsFactory);
    }

    public override void AddListeners() {
        base.AddListeners();

        //_selectionColor.MaterialsColorChanged += OnMaterialsColorChanged;
        //_selectionColor.SettingsApplyed += OnSettingsApplyed;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        //_selectionColor.MaterialsColorChanged -= OnMaterialsColorChanged;
        //_selectionColor.SettingsApplyed -= OnSettingsApplyed;
    }

    //private void OnMaterialsColorChanged(PolyhedrasCompanentTypes type, Color color) => ElementColorChanged?.Invoke(type, color);
    //private void OnSettingsApplyed() => ColorSettingsChanged?.Invoke();
}
