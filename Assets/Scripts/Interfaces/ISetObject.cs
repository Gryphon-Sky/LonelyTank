public interface ISetObject<TData>
{
    void Init(Position pos);
    void GenerateContent();

    Position Pos { get; }

    TData ToData();
    void FromData(TData data);
}