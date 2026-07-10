using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Suppliers.Interfaces;
using FurnitureERP.Domain.Entities;
using FurnitureERP.Domain.Entities.Suppliers;

namespace FurnitureERP.Application.Suppliers.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;

    public SupplierService(
        ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<Supplier>> GetAll(
     string search,
     int page,
     int pageSize)
    {
        return  await _repository.GetAll(
            search,
            page,
            pageSize);
    }

    

    public async Task<List<Supplier>> GetLookup()
    {
        return await _repository.GetLookup();
    }

    
    public async Task Add(
        Supplier supplier)
    {
        await _repository.Add(
            supplier);

        //await _repository.SaveChanges();
    }

    public async Task Update(
     Supplier supplier)
    {
        await _repository.Update(
            supplier);

        //await _repository.SaveChanges();
    }

    public async Task Delete(
        int id)
    {
        var supplier =
            await _repository.GetById(id);

        if (supplier is null)
            return;

        await _repository.Delete(
            supplier);

       // await _repository.SaveChanges();
    }

    public Task<bool> IsNameExists(string name, int? ignoreId = null)
        => _repository.NameExists(name, ignoreId);

    public Task<bool> IsPhoneExists(string name, int? ignoreId = null)
         => _repository.PhoneExists(name, ignoreId);
}