using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Interfaces;

public interface IProductRepository
{
    // Products
    Task<PagedResult<Product>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<Product?> GetById(int id);

    Task<bool> CodeExists(string code, int? ignoreId = null);

    Task<bool> BarcodeExists(string barcode, int? ignoreId = null);

    Task<bool> NameExists(string name, int? ignoreId = null);

    Task<string> GenerateNextCode();

    Task<string> GenerateNextBarcode();

    Task Add(Product product);

    Task Update(Product product);

    Task Delete(Product product);

    //Task SaveChanges();

    // Lookup
    Task<List<Product>> GetLookup();

    //Task<List<ProductCategory>> GetCategories();

   // Task<List<Unit>> GetUnits();

    //Task AddCategory(ProductCategory category);

    //Task AddUnit(Unit unit);

    //Task<ProductCategory?> GetCategoryById(int id);

    //Task UpdateCategory(ProductCategory category);

    //Task<bool> CategoryExists(string name, int? ignoreId = null);

    //Task<Unit?> GetUnitById(int id);

    //Task UpdateUnit(Unit unit);

    //Task<bool> UnitExists(string name, int? ignoreId = null);
}