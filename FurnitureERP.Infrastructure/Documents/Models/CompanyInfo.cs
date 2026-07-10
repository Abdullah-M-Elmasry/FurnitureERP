namespace FurnitureERP.Infrastructure.Documents.Models;

public class CompanyInfo
{
    public string Name { get; set; } = "Furniture ERP";

    public string Address { get; set; } =
        "Cairo - Egypt";

    public string Phone { get; set; } =
        "01000000000";

    public string Email { get; set; } =
        "info@company.com";

    public byte[]? Logo { get; set; }
}