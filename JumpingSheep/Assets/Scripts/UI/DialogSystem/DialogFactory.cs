using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class DialogFactory {
    private readonly DiContainer _container;
    private readonly Logger _logger;

    private readonly List<Dialog> _dialogs;
    private Dialog _dialogPrefab;

    private RectTransform _dialogsParent;

    public DialogFactory(DiContainer container, Logger logger, DialogPrefabs dialogPrefabs) {
        _container = container;
        _logger = logger;

        _dialogs = new List<Dialog>();
        _dialogs.AddRange(dialogPrefabs.Prefabs);
    }

    public void SetDialogsParent(RectTransform dialogsParent) => _dialogsParent = dialogsParent;

    public T GetDialog<T>() where T : Dialog {
        _dialogPrefab = _dialogs.First(dialog => dialog is T);

        if (_dialogPrefab == null) {
            _logger.Log($"Can't find prefab type of: {typeof(T)} ");

            return null;
        }

        return (T)_container.InstantiatePrefabForComponent<Dialog>(_dialogPrefab, _dialogsParent);
    }
}