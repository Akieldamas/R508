using App.DTO;
using App.Models;
using App.Models.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;


[Route("api/brands")]
[ApiController]
[EnableCors("_myAllowSpecificOrigins")]
public class BrandController(IMapper _mapper, IDataRepository<Brand, int, string> manager) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BrandDTO?>> Get(int id)
    {
        var result = await manager.GetByIdAsync(id);
        return result == null ? NotFound() : _mapper.Map<BrandDTO>(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResult<Brand?> Brand = await manager.GetByIdAsync(id);

        if (Brand.Value == null)
            return NotFound();

        await manager.DeleteAsync(Brand.Value);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAll()
    {
        IEnumerable<BrandDTO> brands = _mapper.Map<IEnumerable<BrandDTO>>((await manager.GetAllAsync()));
        return new ActionResult<IEnumerable<BrandDTO>>(brands);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Brand>> Create([FromBody] BrandDTO brandDTO)
    {
        if (brandDTO is null)
            return BadRequest("Body is required.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (string.IsNullOrWhiteSpace(brandDTO.NameBrand))
            return BadRequest("NomMarque is required.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Brand brands = _mapper.Map<Brand>(brandDTO);

        if (brands == null)
            return BadRequest("Mapper Error (Dto -> Brand).");

        await manager.AddAsync(brands);
        return CreatedAtAction("Get", new { id = brandDTO.IdBrand }, brands);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Brand brands)
    {
        if (id != brands.IdBrand)
        {
            return BadRequest();
        }

        ActionResult<Brand?> brandToUpdate = await manager.GetByIdAsync(id);
            
        if (brandToUpdate.Value == null)
        {
            return NotFound();
        }

        await manager.UpdateAsync(brandToUpdate.Value, brands);
        return NoContent();
    }
}
