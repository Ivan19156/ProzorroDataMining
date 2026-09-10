namespace ProzorroDataMining.Domain.Primitives;

public abstract class BaseEntity
{
    public string Id { get; protected set; } = string.Empty;
}