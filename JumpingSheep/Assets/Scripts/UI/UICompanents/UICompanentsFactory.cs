using UnityEngine;
using Zenject;
using System.Linq;
using System.Collections.Generic;

public class UICompanentsFactory {
    private readonly DiContainer _container;
    private readonly Logger _logger;

    private List<UICompanent> _uiCompanents;
    private UICompanent _companent;

    public UICompanentsFactory(DiContainer container, Logger logger, UICompanentPrefabs companents) {
        _container = container;
        _logger = logger;

        _uiCompanents = new List<UICompanent>();
        _uiCompanents.AddRange(companents.Prefabs);
    }

    public T Get<T>(UICompanentConfig config, RectTransform parent) where T : UICompanent {
        _companent = _uiCompanents.First(companent => companent is T);

        if (_companent == null) {
            _logger.Log($"Can't find Companent type of: {config} ");

            return null;
        }

        return (T)_container.InstantiatePrefabForComponent<UICompanent>(_companent, parent);
    }
}
