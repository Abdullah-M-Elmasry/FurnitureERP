using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Interfaces;

public interface IUnitService
{
    Task<PagedResult<Unit>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<Unit> Add(Unit unit);

    Task Update(Unit unit);

    Task Delete(int id);

    Task<List<Unit>> GetLookup();

    Task<bool> IsCodeExists(string code, int? ignoreId = null);

    Task<bool> IsNameExists(string name, int? ignoreId = null);

    Task<string> GenerateNextCode();
}