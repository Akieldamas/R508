using App.DTO;
using App.Models;
using App.Models.EntityFramework;
using App.Models.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Route("api/produits")]
[ApiController]
[EnableCors("_myAllowSpecificOrigins")]
public class ProduitController(IMapper _mapper, IDataRepository<Produit> manager) : ControllerBase
{

    // public async Task<ActionResult<ProductDetailsDTO>> Get(int id)
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Produit>> Get(int id)
    {
        var result = await manager.GetByIdAsync(id);

        if (result.Value == null)
        {
            return NotFound();
        }

        //var resultDTO = _mapper.Map<Produit, ProductDetailsDTO>(result.Value);
        //return resultDTO
        return result.Result;
    }

    //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<Produit>>> GetAll()
    {
        var produitsResult = await manager.GetAllAsync();

        if (produitsResult.Value == null || !produitsResult.Value.Any())
        {
            return NoContent();
        }

        //var produits = _mapper.Map<IEnumerable<Produit>, IEnumerable<ProductDTO>>(produitsResult.Value);

        //return new ActionResult<IEnumerable<ProductDTO>>(produits);
        return Ok(produitsResult.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResult<Produit?> produit = await manager.GetByIdAsync(id);
        
        if (produit.Value == null)
            return NotFound();
        
        await manager.DeleteAsync(produit.Value);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Produit>> Create([FromBody] Produit produit)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await manager.AddAsync(produit);
        return CreatedAtAction("Get", new { id = produit.IdProduit }, produit);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Produit produit)
    {
        if (id != produit.IdProduit)
        {
            return BadRequest();
        }
        
        ActionResult<Produit?> prodToUpdate = await manager.GetByIdAsync(id);
        
        if (prodToUpdate.Value == null)
        {
            return NotFound();
        }
        
        await manager.UpdateAsync(prodToUpdate.Value, produit);
        return NoContent();
    }
}