namespace FurnitureERP.Application.Products.DTOs.Responses;

public class ProductDto
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal SalePrice { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string UnitName { get; set; } = string.Empty;
}