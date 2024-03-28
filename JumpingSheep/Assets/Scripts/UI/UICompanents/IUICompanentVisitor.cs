public interface IUICompanentVisitor {
    void Visit(UICompanentConfig companent);
    void Visit(LevelStatusViewConfig levelStatusConfig);
    void Visit(QTEEventViewConfig eventViewConfig);
    void Visit(SheepIconConfig sheepIconConfig);
}