using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour {
    private Logger _logger;
    private DialogFactory _factory;

    private Dictionary<DialogTypes, Dialog> _dialogsDictionary;

    public MainMenuDialog MainMenuDialog { get; private set; }
    public LevelSelectionDialog LevelSelectionDialog { get; private set; }
    public GameDialog GameDialog { get; private set; }
    public SettingsDialog SettingsDialog { get; private set; }
    public AboutDialog AboutDialog { get; private set; }
    
    public DialogSwitcher DialogSwitcher { get; private set; }

    public void Init(Logger logger, DialogFactory factory) {
        _logger = logger;
        _factory = factory;

        CreateDialogsDictionary();

        DialogSwitcher = new DialogSwitcher(this);
    }

    public Dialog GetDialogByType(DialogTypes type) {
        if (_dialogsDictionary.Keys.Count == 0)
            throw new ArgumentNullException("DialogsDictionary is empty");

        return _dialogsDictionary[type];
    }

    private void CreateDialogsDictionary() {
        MainMenuDialog = _factory.GetDialog<MainMenuDialog>();
        LevelSelectionDialog = _factory.GetDialog<LevelSelectionDialog>();
        SettingsDialog = _factory.GetDialog<SettingsDialog>();
        AboutDialog = _factory.GetDialog<AboutDialog>();
        GameDialog = _factory.GetDialog<GameDialog>();

        _dialogsDictionary = new Dictionary<DialogTypes, Dialog> {
                { DialogTypes.MainMenu, MainMenuDialog },
                { DialogTypes.LevelSelection, LevelSelectionDialog },
                { DialogTypes.Settings, SettingsDialog},
                { DialogTypes.About,  AboutDialog},
                { DialogTypes.Game,  GameDialog},
            };

        foreach (var iDialog in _dialogsDictionary.Values) {
            iDialog.Init(_logger);
            iDialog.Show(false);
        }
    }
}
