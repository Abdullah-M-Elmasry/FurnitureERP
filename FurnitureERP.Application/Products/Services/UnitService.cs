using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Services;

public class UnitService : IUnitService
{
    private readonly IUnitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UnitService(IUnitRepository repository ,IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public Task<PagedResult<Unit>> GetAll(
        string search,
        int page,
        int pageSize)
        => _repository.GetAll(search, page, pageSize);

    public Task<List<Unit>> GetLookup()
        => _repository.GetLookup();

    public Task<bool> IsCodeExists(
        string code,
        int? ignoreId = null)
        => _repository.CodeExists(code, ignoreId);

    public Task<bool> IsNameExists(
        string name,
        int? ignoreId = null)
        => _repository.NameExists(name, ignoreId);

    public Task<string> GenerateNextCode()
        => _repository.GenerateNextCode();

    public async Task<Unit> Add(Unit unit)
    {
        await _repository.Add(unit);

        await _unitOfWork.SaveChangesAsync();
        // await _repository.SaveChanges();

        return unit;
    }

    public async Task Update(Unit unit)
    {
        await _repository.Update(unit);

        await _unitOfWork.SaveChangesAsync();
        //await _repository.SaveChanges();
    }

    public async Task Delete(int id)
    {
        var unit = await _repository.GetById(id);

        if (unit == null)
            return;

        await _repository.Delete(unit);

        await _unitOfWork.SaveChangesAsync();
        // await _repository.SaveChanges();
    }
}