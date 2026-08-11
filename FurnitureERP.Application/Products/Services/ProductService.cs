using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Application.Products.DTOs.Responses;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using System.ComponentModel.DataAnnotations;
using FurnitureERP.Application.Common.Exceptions;

namespace FurnitureERP.Application.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    public ProductService(IProductRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> Add(CreateProductRequest request)
    {
        

          var product = new Product
        {
            Name = request.Name,
            Code= request.Code,
            Barcode= request.Barcode,
            CostPrice = request.CostPrice,
            SalePrice = request.SalePrice,
            CategoryId = request.CategoryId,
            UnitId = request.UnitId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        product.Code = await _repo.GenerateNextCode();

        product.Barcode = await _repo.GenerateNextBarcode();

        await Validate(product);

        //if (await _repo.CodeExists(product.Code))
        //    throw new ValidationExceptionApp("Product Code already exists");

        //if (!string.IsNullOrWhiteSpace(product.Barcode) &&
        //    await _repo.BarcodeExists(product.Barcode))
        //    throw new ValidationExceptionApp("Barcode al,ready exists");

        await _repo.Add(product);

        await _unitOfWork.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            SalePrice = product.SalePrice
        };
    }

    public async Task<ProductDto> Update(UpdateProductRequest request)
    {
        var product = await _repo.GetById(request.Id);

        if (product == null)
            throw new NotFoundExceptionApp("Product not found");

        product.Code = request.Code;
        product.Barcode = request.BarCode;
        product.Name = request.Name;
        product.CostPrice = request.CostPrice;
        product.SalePrice = request.SalePrice;
        product.CategoryId = request.CategoryId;
        product.UnitId = request.UnitId;

        await Validate(product);

        await _repo.Update(product);

        await _unitOfWork.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            SalePrice = product.SalePrice,
            CategoryName = product.Category?.Name ?? "",
            UnitName = product.Unit?.Name ?? ""
        };
    }
    public async Task Delete(int id)
    {
        var product = await _repo.GetById(id)
            ?? throw new NotFoundExceptionApp("Product not found");

        await _repo.Delete(product);

        await _unitOfWork.SaveChangesAsync();
        //  await _repo.SaveChanges();
    }


    //public Task<List<ProductCategory>> GetCategories()
    //    => _repo.GetCategories();

    //public Task<List<Unit>> GetUnits()
    //    => _repo.GetUnits();

    private async Task Validate(Product product)
    {
        if (await _repo.CodeExists(product.Code, product.Id))
            throw new ConflictExceptionApp("Product Code already exists");

        if (!string.IsNullOrWhiteSpace(product.Barcode))
        {
            if (await _repo.BarcodeExists(product.Barcode, product.Id))
                throw new ConflictExceptionApp("Barcode already exists");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ValidationExceptionApp("Name is required");

        if (product.CostPrice < 0)
            throw new ValidationExceptionApp("Cost Price cannot be negative");

        if (product.SalePrice < 0)
            throw new ValidationExceptionApp("Sale Price cannot be negative");
    }

    public async Task<List<Product>> GetLookup()
    {
        return await _repo.GetLookup();
    }


    public async Task<ProductDetailsDto?> GetById(int id)
    {

        var product = await _repo.GetById(id);

        if (product == null)
            throw new NotFoundExceptionApp("Product not found");

        return new ProductDetailsDto
        {
            Id = product.Id,
            Code = product.Code,
            BarCode = product.Barcode,
            Name = product.Name,
            CostPrice = product.CostPrice,
            SalePrice = product.SalePrice,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? "",
            UnitId = product.UnitId,
            UnitName = product.Unit?.Name ?? ""
        };
    }

    public async Task<PagedResult<ProductDto>> GetAll(
      string search,
      int page,
      int pageSize)
    {
        var result = await _repo.GetAll(search, page, pageSize);

        return new PagedResult<ProductDto>
        {
            TotalCount = result.TotalCount,
            Items = result.Items.Select(p => new ProductDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                SalePrice = p.SalePrice,
                CategoryName = p.Category?.Name ?? "",
                UnitName = p.Unit?.Name ?? ""
            }).ToList()
        };
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