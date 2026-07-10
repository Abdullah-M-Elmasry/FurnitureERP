using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Interfaces;

public interface ICategoryRepository
{
    Task<PagedResult<ProductCategory>> GetAll(
        string search,
        int page,
        int pageSize);
    Task<List<ProductCategory>> GetLookup();
    Task<ProductCategory?> GetById(int id);

    Task<bool> CodeExists(string code, int? ignoreId = null);

    Task<bool> NameExists(string name, int? ignoreId = null);

    Task<string> GenerateNextCode();

    Task Add(ProductCategory category);

    Task Update(ProductCategory category);

    Task Delete(ProductCategory category);

  //  Task SaveChanges();

    
}