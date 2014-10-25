public interface ISetObject<TData>
{
    void Init(Position pos);
    void GenerateContent();

    TData ToData();
    void FromData(TData data);
}