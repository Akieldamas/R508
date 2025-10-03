using App.DTO;
using App.Models;
using App.Models.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;


[Route("api/typeproduits")]
[ApiController]
[EnableCors("_myAllowSpecificOrigins")]
public class TypeProduitController(IMapper _mapper, IDataRepository<TypeProduit> manager) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TypeProduitDto?>> Get(int id)
    {
        var result = await manager.GetByIdAsync(id);
        return result.Value == null ? NotFound() : _mapper.Map<TypeProduitDto>(result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResult<TypeProduit?> TypeProduit = await manager.GetByIdAsync(id);

        if (TypeProduit.Value == null)
            return NotFound();

        await manager.DeleteAsync(TypeProduit.Value);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<TypeProduitDto>>> GetAll()
    {
        IEnumerable<TypeProduitDto> TypeProduits = _mapper.Map<IEnumerable<TypeProduitDto>>((await manager.GetAllAsync()).Value);
        return new ActionResult<IEnumerable<TypeProduitDto>>(TypeProduits);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TypeProduit>> Create([FromBody] TypeProduit TypeProduits)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await manager.AddAsync(TypeProduits);
        return CreatedAtAction("Get", new { id = TypeProduits.IdTypeProduit }, TypeProduits);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] TypeProduit TypeProduits)
    {
        if (id != TypeProduits.IdTypeProduit)
        {
            return BadRequest();
        }

        ActionResult<TypeProduit?> prodToUpdate = await manager.GetByIdAsync(id);

        if (prodToUpdate.Value == null)
        {
            return NotFound();
        }

        await manager.UpdateAsync(prodToUpdate.Value, TypeProduits);
        return NoContent();
    }
}
