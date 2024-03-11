using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelSelectionPanel : UIPanel {
    private UICompanentsFactory _factory;

    [Inject]
    public void Construct(UICompanentsFactory factory) {
        _factory = factory;
    }

}
