public interface INode<TGrid, TData>
{
    void Init(TGrid parent, Position pos);
    void GenerateContent();

    Position Pos { get; }

    TData ToData();
    void FromData(TData data);
}