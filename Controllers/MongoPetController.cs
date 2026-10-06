using ChallengeAPI.Models;
using ChallengeAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MongoPetController : ControllerBase
{
    private readonly MongoPetRepository _repository;

    public MongoPetController(MongoPetRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<MongoPet>>> GetAll() =>
        Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<MongoPet>> GetById(int id)
    {
        var pet = await _repository.GetByIdAsync(id);

        return pet is null ? NotFound() : Ok(pet);
    }

    [HttpPost]
    public async Task<ActionResult<MongoPet>> Create(MongoPetRequest request)
    {
        var pet = new MongoPet
        {
            Nome = request.Nome,
            Especie = request.Especie,
            Raca = request.Raca
        };

        await _repository.CreateAsync(pet);

        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, MongoPet pet)
    {
        var existingPet = await _repository.GetByIdAsync(id);

        if (existingPet is null)
            return NotFound();

        pet.Id = id;

        await _repository.UpdateAsync(id, pet);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existingPet = await _repository.GetByIdAsync(id);

        if (existingPet is null)
            return NotFound();

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}