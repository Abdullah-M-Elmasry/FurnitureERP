using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Suppliers;

namespace FurnitureERP.Application.Suppliers.Interfaces;

public interface ISupplierRepository
{
    Task<PagedResult<Supplier>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<List<Supplier>> GetLookup();

    Task<Supplier?> GetById(int id);

    Task Add(Supplier supplier);

    Task Update(Supplier supplier);

    Task Delete(Supplier supplier);

    Task<bool> NameExists(string name, int? ignoreId = null);

    Task<bool> PhoneExists(string phone, int? ignoreId = null);
}