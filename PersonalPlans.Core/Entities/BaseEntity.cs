namespace PersonalPlans.Core.Entities;

/// <summary>
/// Base class for entities that have a common database identifier.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}
