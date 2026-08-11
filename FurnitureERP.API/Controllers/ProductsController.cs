using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Application.Products.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    string search = "",
    int page = 1,
    int pageSize = 10)
    {
        var result = await _productService.GetAll(search, page, pageSize);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var product = await _productService.Add(request);

        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetById(id);

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request)
    {
        if (id != request.Id)
            return BadRequest("Id mismatch");

        var product = await _productService.Update(request);

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.Delete(id);

        return NoContent();
    }

}