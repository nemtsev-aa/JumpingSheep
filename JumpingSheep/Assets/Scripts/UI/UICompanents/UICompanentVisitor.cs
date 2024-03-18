using System.Collections.Generic;
using System.Linq;

public class UICompanentVisitor : ICompanentVisitor {
    private readonly List<UICompanent> _companents;

    public UICompanentVisitor(List<UICompanent> companents) {
        _companents = new List<UICompanent>();
        _companents.AddRange(companents);
    }

    public UICompanent Companent { get; private set; }

    public void Visit(UICompanentConfig config) => Visit((dynamic)config);

    public void Visit(LevelStatusViewConfig levelStatusConfig)
        => Companent = _companents.FirstOrDefault(companent => companent is LevelStatusView);

    public void Visit(QTEEventViewConfig eventViewConfig) 
        => Companent = _companents.FirstOrDefault(companent => companent is QTEEventView);

    public void Visit(SheepIconConfig sheepIconConfig)
        => Companent = _companents.FirstOrDefault(companent => companent is SheepIcon);
}