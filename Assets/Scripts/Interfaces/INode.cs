public interface INode<TData>
{
    void Init(Position pos);
    void GenerateContent();

    Position Pos { get; }

    TData ToData();
    void FromData(TData data);
}