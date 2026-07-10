using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Interfaces;

public interface ICategoryService
{
    Task<PagedResult<ProductCategory>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<List<ProductCategory>> GetLookup();
    Task<ProductCategory> Add(ProductCategory category);

    Task Update(ProductCategory category);

    Task Delete(int id);


    Task<bool> IsCodeExists(string code, int? ignoreId = null);

    Task<bool> IsNameExists(string name, int? ignoreId = null);

    Task<string> GenerateNextCode();
}