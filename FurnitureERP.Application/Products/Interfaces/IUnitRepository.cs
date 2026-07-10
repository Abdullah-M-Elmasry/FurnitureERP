using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Interfaces;

public interface IUnitRepository
{
    Task<PagedResult<Unit>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<Unit?> GetById(int id);

    Task<bool> CodeExists(string code, int? ignoreId = null);

    Task<bool> NameExists(string name, int? ignoreId = null);

    Task<string> GenerateNextCode();

    Task Add(Unit unit);

    Task Update(Unit unit);

    Task Delete(Unit unit);

    Task<List<Unit>> GetLookup();
}