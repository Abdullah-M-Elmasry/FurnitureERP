using FurnitureERP.Domain.Entities.Products;

public class Unit
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; }
        = new List<Product>();

    public override string ToString() => Name;
}