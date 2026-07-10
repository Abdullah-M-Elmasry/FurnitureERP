using FurnitureERP.Application.Common.Models;
using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.Domain.Entities.Suppliers;

namespace FurnitureERP.Application.Customers.Interfaces;

public interface ICustomerRepository
{
    Task<PagedResult<Customer>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<List<Customer>> GetLookup();

    Task<Customer?> GetById(int id);

    Task Add(Customer customer);

    Task Update(Customer customer);

    Task Delete(Customer customer);

    Task<bool> PhoneExists(string phone, int? ignoreId = null);

    Task<bool> NameExists(string name, int? ignoreId = null);

    //Task SaveChanges();
}