using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Customers.Interfaces;
using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.Domain.Entities.Suppliers;

namespace FurnitureERP.Application.Customers.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public CustomerService(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<Customer>> GetAll(
        string search,
        int page,
        int pageSize)
    {
        return await _repository.GetAll(
            search,
            page,
            pageSize);
    }

    public async Task <List<Customer>> GetLookup()
    {
        return await _repository.GetLookup();
    }
    public async Task Add(Customer customer)
    {
        await _repository.Add(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Update(Customer customer)
    {
        await _repository.Update(customer);

        await _unitOfWork.SaveChangesAsync();

    }

    public async Task Delete(int id)
    {
        var customer =
            await _repository.GetById(id);

        if (customer == null)
            return;

        await _repository.Delete(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public Task<bool> IsNameExists(string name, int? ignoreId = null)
        => _repository.NameExists(name, ignoreId);

    public Task<bool> IsPhoneExists(string name, int? ignoreId = null)
         => _repository.PhoneExists(name, ignoreId);

}