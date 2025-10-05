using App.DTO;
using App.Models;
using App.Models.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;


[Route("api/producttypes")]
[ApiController]
[EnableCors("_myAllowSpecificOrigins")]
public class ProductTypeController(IMapper _mapper, IDataRepository<ProductType> manager) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductTypeDTO?>> Get(int id)
    {
        var result = await manager.GetByIdAsync(id);
        return result.Value == null ? NotFound() : _mapper.Map<ProductTypeDTO>(result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResult<ProductType?> TypeProduit = await manager.GetByIdAsync(id);

        if (TypeProduit.Value == null)
            return NotFound();

        await manager.DeleteAsync(TypeProduit.Value);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<ProductTypeDTO>>> GetAll()
    {
        IEnumerable<ProductTypeDTO> TypeProduits = _mapper.Map<IEnumerable<ProductTypeDTO>>((await manager.GetAllAsync()).Value);
        return new ActionResult<IEnumerable<ProductTypeDTO>>(TypeProduits);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductType>> Create([FromBody] ProductType TypeProduits)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await manager.AddAsync(TypeProduits);
        return CreatedAtAction("Get", new { id = TypeProduits.IdProductType }, TypeProduits);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] ProductType TypeProduits)
    {
        if (id != TypeProduits.IdProductType)
        {
            return BadRequest();
        }

        ActionResult<ProductType?> prodToUpdate = await manager.GetByIdAsync(id);

        if (prodToUpdate.Value == null)
        {
            return NotFound();
        }

        await manager.UpdateAsync(prodToUpdate.Value, TypeProduits);
        return NoContent();
    }
}
