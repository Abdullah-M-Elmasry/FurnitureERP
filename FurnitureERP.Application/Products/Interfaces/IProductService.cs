using FurnitureERP.Application.Common.Models;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Application.Products.DTOs.Responses;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Application.Products.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetAll(
        string search,
        int page,
        int pageSize);

    Task<ProductDto> Add(CreateProductRequest request);

    Task<ProductDto> Update(UpdateProductRequest request);

    Task Delete(int id);
    
    Task<List<Product>> GetLookup();

    Task<ProductDetailsDto?> GetById(int id);

    Task<bool> IsCodeExists(string code, int? ignoreId = null);

    Task<bool> IsBarcodeExists(string barcode, int? ignoreId = null);

    Task<bool> IsNameExists(string name, int? ignoreId = null);

    Task<string> GenerateNextCode();

    Task<string> GenerateNextBarcode();
    //Task<List<ProductCategory>> GetCategories();

    //Task<List<Unit>> GetUnits();


    //Task<ProductCategory> AddCategory(string name);

    //Task<Unit> AddUnit(string name);

    //Task UpdateCategory(int id, string name);

    //Task UpdateUnit(int id, string name);
}