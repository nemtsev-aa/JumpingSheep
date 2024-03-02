using System.Collections.Generic;
using System.Linq;

public class DialogSwitcher {
    private UIManager _uIManager;

    private List<Dialog> _showedDialogs;
    private Dialog _activeDialog;

    public DialogSwitcher(UIManager uIManager) {
        _uIManager = uIManager;
        _showedDialogs = new List<Dialog>();
    }

    public void ShowDialog(DialogTypes type) {
        if (_activeDialog != null) {
            _activeDialog.ResetPanels();
            _activeDialog.Show(false);
        }

        _activeDialog = _uIManager.GetDialogByType(type);
        _showedDialogs.Add(_activeDialog);
        _activeDialog.Show(true);
    }

    public void ShowPreviousDialog() {
        if (_showedDialogs.Count > 0) {
            _activeDialog.ResetPanels();
            _activeDialog.Show(false);
            _showedDialogs.Remove(_activeDialog);
        }

        _activeDialog = _showedDialogs.Last();
        _activeDialog.Show(true);
    }
}
