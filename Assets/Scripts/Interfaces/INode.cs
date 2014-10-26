public interface INode<TData>
{
    Position Pos { get; }
    
    void Init(Position pos);
    void GenerateContent();

    TData ToData();
    void FromData(TData data);
}