using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UICompanentsFactory {
    private List<UICompanent> _companents;
    
    private UICompanentVisitor _visitor;
    private DiContainer _container;

    public UICompanentsFactory(DiContainer container, UICompanentPrefabs companents) {
        _container = container;
        _companents = companents.Prefabs;
    }

    private UICompanent Companent => _visitor.Companent;

    public T Get<T>(UICompanentConfig config, RectTransform parent) where T : UICompanent {
        _visitor = new UICompanentVisitor(_companents);
        _visitor.Visit(config);

        var newCompanent = _container.InstantiatePrefabForComponent<UICompanent>(Companent, parent);
        return (T)newCompanent;
    }
}
