using App.DTO;
using App.Models;
using App.Models.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;


[Route("api/marques")]
[ApiController]
[EnableCors("_myAllowSpecificOrigins")]
public class MarqueController(IMapper _mapper, IDataRepository<Marque> manager) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MarqueDto?>> Get(int id)
    {
        var result = await manager.GetByIdAsync(id);
        return result.Value == null ? NotFound() : _mapper.Map<MarqueDto>(result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResult<Marque?> Marque = await manager.GetByIdAsync(id);

        if (Marque.Value == null)
            return NotFound();

        await manager.DeleteAsync(Marque.Value);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<MarqueDto>>> GetAll()
    {
        IEnumerable<MarqueDto> marques = _mapper.Map<IEnumerable<MarqueDto>>((await manager.GetAllAsync()).Value);
        return new ActionResult<IEnumerable<MarqueDto>>(marques);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Marque>> Create([FromBody] MarqueDto marqueDto)
    {
        if (marqueDto is null)
            return BadRequest("Body is required.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (string.IsNullOrWhiteSpace(marqueDto.NomMarque))
            return BadRequest("NomMarque is required.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Marque marques = _mapper.Map<Marque>(marqueDto);
        
        await manager.AddAsync(marques);
        return CreatedAtAction("Get", new { id = marqueDto.IdMarque }, marques);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Marque marques)
    {
        if (id != marques.IdMarque)
        {
            return BadRequest();
        }

        ActionResult<Marque?> prodToUpdate = await manager.GetByIdAsync(id);

        if (prodToUpdate.Value == null)
        {
            return NotFound();
        }

        await manager.UpdateAsync(prodToUpdate.Value, marques);
        return NoContent();
    }
}
