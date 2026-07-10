namespace FurnitureERP.Application.Inventory.DTOs;

public class OpeningBalanceDto
{
    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public string? Notes { get; set; }
}