namespace Languages.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAtUTC { get; set; }
    public DateTime ModifiedAtUTC { get; set; }
}
