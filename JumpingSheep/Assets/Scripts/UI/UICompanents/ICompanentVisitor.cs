public interface ICompanentVisitor {
    void Visit(UICompanentConfig companent);
    void Visit(LevelStatusViewConfig levelStatusConfig);
    void Visit(QTEEventViewConfig eventViewConfig);
}