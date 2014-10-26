public interface INode<TGrid, TData>
{
    Position Pos { get; }
    
    void Init(TGrid parent, Position pos);
    void GenerateContent();

    TData ToData();
    void FromData(TData data);
}