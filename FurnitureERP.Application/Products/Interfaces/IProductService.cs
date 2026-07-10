using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Interfaces;

public interface IProductService
{
    Task<PagedResult<Product>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<Product> Add(Product product);

    Task Update(Product product);

    Task Delete(int id);
    
    Task<List<Product>> GetLookup();

    Task<bool> IsCodeExists(string code, int? ignoreId = null);

    Task<bool> IsBarcodeExists(string barcode, int? ignoreId = null);

    Task<bool> IsNameExists(string name, int? ignoreId = null);

    Task<string> GenerateNextCode();

    Task<string> GenerateNextBarcode();
    //Task<List<ProductCategory>> GetCategories();

    //Task<List<Unit>> GetUnits();

    
    //Task<ProductCategory> AddCategory(string name);

    //Task<Unit> AddUnit(string name);

    //Task UpdateCategory(int id, string name);

    //Task UpdateUnit(int id, string name);


   
}