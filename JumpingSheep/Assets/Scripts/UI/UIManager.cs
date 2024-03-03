using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour {
    private Logger _logger;
    private Dictionary<DialogTypes, Dialog> _dialogsDictionary;

    [field: SerializeField] public MainMenuDialog MainMenuDialog { get; private set; }
    [field: SerializeField] public GameDialog GameDialog { get; private set; }
    [field: SerializeField] public SettingsDialog SettingsDialog { get; private set; }
    [field: SerializeField] public AboutDialog AboutDialog { get; private set; }
    
    public DialogSwitcher DialogSwitcher { get; private set; }

    public void Init(Logger logger, SheepQuantityCounter sheepCounter, QTEEventConfigs qTEEventConfigs, MovementHandler movementHandler) {
        _logger = logger;

        GameDialog.SetDependency(sheepCounter, qTEEventConfigs, movementHandler);

        CreateDialogsDictionary();

        DialogSwitcher = new DialogSwitcher(this);
    }

    public Dialog GetDialogByType(DialogTypes type) {
        if (_dialogsDictionary.Keys.Count == 0)
            throw new ArgumentNullException("DialogsDictionary is empty");

        return _dialogsDictionary[type];
    }

    private void CreateDialogsDictionary() {
        _dialogsDictionary = new Dictionary<DialogTypes, Dialog> {
                { DialogTypes.Settings, SettingsDialog},
                { DialogTypes.MainMenu, MainMenuDialog},
                { DialogTypes.About,  AboutDialog},
                { DialogTypes.Game,  GameDialog},
            };

        foreach (var iDialog in _dialogsDictionary.Values) {
            iDialog.Init(_logger);
            iDialog.Show(false);
        }
    }
}
