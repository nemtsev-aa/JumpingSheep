using System.Collections.Generic;
using System.Linq;

public class UICompanentVisitor : ICompanentVisitor {
    private List<UICompanent> _companents = new List<UICompanent>();

    public UICompanentVisitor(List<UICompanent> companents) {
        _companents = companents;
    }

    public UICompanent Companent { get; private set; }

    public void Visit(UICompanentConfig config) => Visit((dynamic)config);

    public void Visit(LevelStatusViewConfig levelStatusConfig)
        => Companent = _companents.FirstOrDefault(companent => companent is LevelStatusView);

}