using UnityEngine;
using Zenject;

public class UICompanentsFactory {
    private UICompanentVisitor _visitor;
    private readonly DiContainer _container;
    private readonly Logger _logger;

    public UICompanentsFactory(DiContainer container, Logger logger, UICompanentPrefabs companents) {
        _container = container;
        _logger = logger;

        _visitor = new UICompanentVisitor(companents.Prefabs);
    }

    private UICompanent Companent => _visitor.Companent;

    public T Get<T>(UICompanentConfig config, RectTransform parent) where T : UICompanent {
        _visitor.Visit(config);

        if (Companent == null) {
            _logger.Log($"Can't find Companent type of: {config} ");

            return null;
        }

        return (T)_container.InstantiatePrefabForComponent<UICompanent>(Companent, parent);
        
    }
}
