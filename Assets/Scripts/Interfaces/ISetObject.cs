public interface ISetObject<TData>
{
    void Init(Position pos);
    void GenerateContent();

    Position GetPosition();

    TData ToData();
    void FromData(TData data);
}