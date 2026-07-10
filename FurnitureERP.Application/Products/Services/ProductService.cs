using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<Product> Add(Product product)
    {
        Validate(product);

        if (await _repo.CodeExists(product.Code))
            throw new Exception("Product Code already exists");

        if (!string.IsNullOrWhiteSpace(product.Barcode) &&
            await _repo.BarcodeExists(product.Barcode))
            throw new Exception("Barcode already exists");

        await _repo.Add(product);

      //  await _repo.SaveChanges();

        return product;
    }

    public async Task Update(Product product)
    {
        Validate(product);

        var existing = await _repo.GetById(product.Id)
            ?? throw new Exception("Product not found");

        if (await _repo.CodeExists(product.Code, product.Id))
            throw new Exception("Duplicate Code");

        if (!string.IsNullOrWhiteSpace(product.Barcode) &&
            await _repo.BarcodeExists(product.Barcode, product.Id))
            throw new Exception("Duplicate Barcode");

        existing.Code = product.Code;
        existing.Barcode = product.Barcode;
        existing.Name = product.Name;
        existing.CostPrice = product.CostPrice;
        existing.SalePrice = product.SalePrice;
        existing.CategoryId = product.CategoryId;
        existing.UnitId = product.UnitId;

        await _repo.Update(existing);

        //await _repo.SaveChanges();
    }

    public async Task Delete(int id)
    {
        var product = await _repo.GetById(id)
            ?? throw new Exception("Product not found");

        await _repo.Delete(product);

      //  await _repo.SaveChanges();
    }

   
    //public Task<List<ProductCategory>> GetCategories()
    //    => _repo.GetCategories();

    //public Task<List<Unit>> GetUnits()
    //    => _repo.GetUnits();

    private void Validate(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Code))
            throw new Exception("Code is required");

        if (string.IsNullOrWhiteSpace(product.Name))
            throw new Exception("Name is required");
    }


    public async Task<List<Product>> GetLookup()
    {
        return await _repo.GetLookup();
    }


    public Task<PagedResult<Product>> GetAll(
      string search,
      int page,
      int pageSize)
    {
        return _repo.GetAll(search, page, pageSize);
    }


    //public async Task<ProductCategory> AddCategory(string name)
    //{

    //    if (string.IsNullOrWhiteSpace(name))
    //        throw new Exception("Category name is required");

    //    name = name.Trim();

    //    var exists = await _repo.CategoryExists(name);

    //    if (exists)
    //        throw new InvalidOperationException("Category already exists");

    //    var cat = new ProductCategory { Name = name };

    //    await _repo.AddCategory(cat);
    //    return cat;
    //}

    //public async Task<Unit> AddUnit(string name)
    //{
    //    if (string.IsNullOrWhiteSpace(name))
    //        throw new Exception("Unit name is required");

    //    name = name.Trim();

    //    var exists = await _repo.UnitExists(name);

    //    if (exists)
    //        throw new InvalidOperationException("Unit already exists");
       

    //    var unit = new Unit { Name = name };

    //    await _repo.AddUnit(unit);
    //    return unit;
    //}

    //public async Task UpdateCategory(int id, string name)
    //{
    //    if (string.IsNullOrWhiteSpace(name))
    //        throw new Exception("Category name is required");

    //    name = name.Trim();

    //    var exists = await _repo.CategoryExists(name, id);

    //    if (exists)
    //        throw new InvalidOperationException("Category already exists");

    //    var cat = await _repo.GetCategoryById(id)
    //        ?? throw new Exception("Category not found");

    //    cat.Name = name;

    //    await _repo.UpdateCategory(cat);
    //}

    //public async Task UpdateUnit(int id, string name)
    //{
    //    if (string.IsNullOrWhiteSpace(name))
    //        throw new Exception("Unit name is required");

    //    name = name.Trim();

    //    var exists = await _repo.UnitExists(name, id);

    //    if (exists)
    //        throw new InvalidOperationException("Unit already exists");

    //    var unit = await _repo.GetUnitById(id)
    //        ?? throw new Exception("Unit not found");

    //    unit.Name = name;

    //    await _repo.UpdateUnit(unit);
    //}



    public Task<bool> IsCodeExists(string code, int? ignoreId = null)
    => _repo.CodeExists(code, ignoreId);

    public Task<bool> IsBarcodeExists(string barcode, int? ignoreId = null)
        => _repo.BarcodeExists(barcode, ignoreId);

    public Task<bool> IsNameExists(string name, int? ignoreId = null)
        => _repo.NameExists(name, ignoreId);

    public Task<string> GenerateNextCode()
        => _repo.GenerateNextCode();

    public Task<string> GenerateNextBarcode()
        => _repo.GenerateNextBarcode();


}