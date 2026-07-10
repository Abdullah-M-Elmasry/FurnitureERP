using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // GET PAGED
    // =====================================================

    public async Task<PagedResult<ProductCategory>> GetAll(
        string search,
        int page,
        int pageSize)
    {
        return await _repository.GetAll(search, page, pageSize);
    }

    // =====================================================
    // LOOKUP
    // =====================================================

    public async Task<List<ProductCategory>> GetLookup()
    {
        return await _repository.GetLookup();
    }

    // =====================================================
    // VALIDATION
    // =====================================================

    public async Task<bool> IsCodeExists(
        string code,
        int? ignoreId = null)
    {
        return await _repository.CodeExists(code, ignoreId);
    }

    public async Task<bool> IsNameExists(
        string name,
        int? ignoreId = null)
    {
        return await _repository.NameExists(name, ignoreId);
    }

    // =====================================================
    // GENERATE CODE
    // =====================================================

    public async Task<string> GenerateNextCode()
    {
        return await _repository.GenerateNextCode();
    }

    // =====================================================
    // ADD
    // =====================================================

    public async Task<ProductCategory> Add(ProductCategory category)
    {
        await _repository.Add(category);

        //await _repository.SaveChanges();

        return category;
    }

    // =====================================================
    // UPDATE
    // =====================================================

    public async Task Update(ProductCategory category)
    {
        await _repository.Update(category);

      //  await _repository.SaveChanges();
    }

    // =====================================================
    // DELETE
    // =====================================================

    public async Task Delete(int id)
    {
        var category = await _repository.GetById(id);

        if (category == null)
            return;

        await _repository.Delete(category);

       // await _repository.SaveChanges();
    }
}