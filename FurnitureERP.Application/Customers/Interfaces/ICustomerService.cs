using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.Domain.Entities.Suppliers;

namespace FurnitureERP.Application.Customers.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<Customer>> GetAll(
        string search,
        int page,
        int pageSize);
    Task<List<Customer>> GetLookup();
    Task Add(Customer customer);

    Task Update(Customer customer);

    Task Delete(int id);

    Task<bool> IsNameExists(string name, int? ignoreId = null);

    Task<bool> IsPhoneExists(string phone, int? ignoreId = null);
}