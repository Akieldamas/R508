using App.DTO;
using App.Models;
using App.Models.EntityFramework;
using App.Models.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Route("api/products")]
[ApiController]
[EnableCors("_myAllowSpecificOrigins")]
public class ProductController(IMapper _mapper, IDataRepository<Product, int, string> manager) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDetailsDTO>> Get(int id)
    {
        var result = await manager.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        var resultDTO = _mapper.Map<Product, ProductDetailsDTO>(result);
        return resultDTO;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
    {
        var productsResult = await manager.GetAllAsync();

        if (productsResult == null || !productsResult.Any())
        {
            return NoContent();
        }

        var produits = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(productsResult);

        return new ActionResult<IEnumerable<ProductDTO>>(produits);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResult<Product?> product = await manager.GetByIdAsync(id);
        
        if (product.Value == null)
            return NotFound();
        
        await manager.DeleteAsync(product.Value);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await manager.AddAsync(product);
        return CreatedAtAction("Get", new { id = product.IdProduct }, product);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (id != product.IdProduct)
        {
            return BadRequest();
        }
        
        ActionResult<Product?> prodToUpdate = await manager.GetByIdAsync(id);
        
        if (prodToUpdate.Value == null)
        {
            return NotFound();
        }
        
        await manager.UpdateAsync(prodToUpdate.Value, product);
        return NoContent();
    }
}