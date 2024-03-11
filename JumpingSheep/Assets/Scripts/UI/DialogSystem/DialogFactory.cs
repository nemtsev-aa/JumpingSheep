using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DialogFactory {
    private const string PrefabsFilePath = "Dialogs/";
    private DiContainer _container;
    private RectTransform _dialogsParent;

    private static readonly Dictionary<Type, string> _prefabsDictionary = new Dictionary<Type, string>() {
            {typeof(MainMenuDialog), nameof(MainMenuDialog)},
            {typeof(LevelSelectionDialog), nameof(LevelSelectionDialog)},
            {typeof(SettingsDialog), nameof(SettingsDialog)},
            {typeof(GameDialog), nameof(GameDialog)},
            {typeof(AboutDialog), nameof(AboutDialog)},
    };

    public DialogFactory(DiContainer container) {
        _container = container;
    }

    public void SetDialogsParent(RectTransform dialogsParent) => _dialogsParent = dialogsParent;

    public T GetDialog<T>() where T : Dialog {
        var go = GetPrefabByType<T>();

        if (go == null)
            return null;

        var newDialog = _container.InstantiatePrefabForComponent<Dialog>(go, _dialogsParent);
        return (T)newDialog;
    }

    private T GetPrefabByType<T>() where T : Dialog {
        var prefabName = _prefabsDictionary[typeof(T)];

        if (string.IsNullOrEmpty(prefabName)) {
            Debug.LogError("Cant find prefab type of " + typeof(T) + "Do you added it in PrefabsDictionary?");
        }

        var path = PrefabsFilePath + _prefabsDictionary[typeof(T)];
        var dialog = Resources.Load<T>(path);

        if (dialog == null)
            Debug.LogError("Cant find prefab at path " + path);

        return dialog;
    }
}