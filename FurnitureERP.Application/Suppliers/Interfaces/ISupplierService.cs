using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Suppliers;

namespace FurnitureERP.Application.Suppliers.Interfaces;

public interface ISupplierService
{
    Task<PagedResult<Supplier>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<List<Supplier>> GetLookup();

    Task Add(Supplier supplier);

    Task Update(Supplier supplier);

    Task Delete(int id);

    Task<bool> IsNameExists(
        string name,
        int? ignoreId = null);

    Task<bool> IsPhoneExists(
        string phone,
        int? ignoreId = null);
}