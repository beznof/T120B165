namespace Languages.Domain.Entities;

public abstract class BaseEntity
{
    public int ID { get; set; }
    public DateTime CreatedAtUTC { get; set; }
    public DateTime ModifiedAtUTC { get; set; }
}