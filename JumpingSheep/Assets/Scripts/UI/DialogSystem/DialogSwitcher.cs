using System;
using System.Collections.Generic;
using System.Linq;

public class DialogSwitcher {
    private UIManager _uIManager;

    private List<Type> _showedDialogs;
    private Dialog _activeDialog;

    public DialogSwitcher(UIManager uIManager) {
        _uIManager = uIManager;
        _showedDialogs = new List<Type>();
    }

    public void ShowDialog<T>() where T : Dialog {
        if (_activeDialog != null) {
            _activeDialog.ResetPanels();
            _activeDialog.Show(false);
        }

        _activeDialog = _uIManager.GetDialogByType<T>();
        _showedDialogs.Add(_activeDialog.GetType());
        _activeDialog.Show(true);
    }

    public void ShowPreviousDialog() {
        if (_showedDialogs.Count > 0) {
            _activeDialog.ResetPanels();
            _activeDialog.Show(false);

            _showedDialogs.Remove(_activeDialog.GetType());
        }

        GetDialogByType(_showedDialogs.Last());
        _activeDialog.Show(true);
    }

    public void GetDialogByType(Type type) {
        if (type == typeof(MainMenuDialog))
            _activeDialog = _uIManager.GetDialogByType<MainMenuDialog>();

        if (type == typeof(LevelSelectionDialog))
            _activeDialog = _uIManager.GetDialogByType<LevelSelectionDialog>();

        if (type == typeof(SettingsDialog))
            _activeDialog = _uIManager.GetDialogByType<SettingsDialog>();

        if (type == typeof(GameDialog))
            _activeDialog = _uIManager.GetDialogByType<GameDialog>();

        if (type == typeof(AboutDialog))
            _activeDialog = _uIManager.GetDialogByType<AboutDialog>();
    }
}
