using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeAPI.Data;
using ChallengeAPI.Models;
using Microsoft.Extensions.Logging;

namespace ChallengeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<PetsController> _logger;

    public PetsController(
        AppDbContext context,
        ILogger<PetsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retorna todos os pets cadastrados.
    /// </summary>
    /// <returns>Lista de pets.</returns>
    /// <response code="200">Pets encontrados com sucesso.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
    {
        var pets = await _context.Pets
            .Include(p => p.Tutor)
            .ToListAsync();

        return Ok(pets);
    }

    /// <summary>
    /// Busca um pet pelo ID.
    /// </summary>
    /// <param name="id">ID do pet.</param>
    /// <response code="200">Pet encontrado.</response>
    /// <response code="404">Pet não encontrado.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Pet>> GetPetById(int id)
    {

        var pet = await _context.Pets
            .Include(p => p.Tutor)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pet == null)
        {
            _logger.LogWarning("Pet com ID {PetId} não encontrado", id);
            return NotFound();
        }

        return pet;
    }

    /// <summary>
    /// Busca pets pela espécie.
    /// </summary>
    /// <param name="especie">Espécie do pet.</param>
    /// <response code="200">Pets encontrados.</response>
    /// <response code="404">Nenhum pet encontrado.</response>
    [HttpGet("especie/{especie}")]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPetPorEspecie(string especie)
    {
        var pets = await _context.Pets
            .Where(p => p.Especie.Contains(especie))
            .ToListAsync();

        if (!pets.Any())
        {
            return NotFound();
        }

        return Ok(pets);
    }

    /// <summary>
    /// Busca pets pela raça.
    /// </summary>
    /// <param name="raca">Raça do pet.</param>
    /// <response code="200">Pets encontrados.</response>
    /// <response code="404">Nenhum pet encontrado.</response>
    [HttpGet("raca/{raca}")]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPetPorRaca(string raca)
    {
        var pets = await _context.Pets
            .Where(p => p.Raca.Contains(raca))
            .ToListAsync();

        if (!pets.Any())
        {
            return NotFound();
        }

        return Ok(pets);
    }

    /// <summary>
    /// Busca pets pela idade.
    /// </summary>
    /// <param name="idade">Idade do pet.</param>
    /// <response code="200">Pets encontrados.</response>
    /// <response code="404">Nenhum pet encontrado.</response>
    [HttpGet("idade/{idade}")]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPetPorIdade(int idade)
    {
        var pets = await _context.Pets
            .Where(p => p.Idade == idade)
            .ToListAsync();

        if (!pets.Any())
        {
            return NotFound();
        }

        return Ok(pets);
    }

    /// <summary>
    /// Busca pets pelo nome.
    /// </summary>
    /// <param name="nome">Nome do pet.</param>
    /// <response code="200">Pets encontrados.</response>
    /// <response code="404">Nenhum pet encontrado.</response>

    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPetPorNome(string nome)
    {
        var pets = await _context.Pets
            .Where(p => p.Nome.Contains(nome))
            .ToListAsync();

        if (!pets.Any())
        {
            return NotFound();
        }

        return Ok(pets);
    }

    /// <summary>
    /// Cadastra um novo pet.
    /// </summary>
    /// <param name="pet">Dados do pet.</param>
    /// <response code="201">Pet criado com sucesso.</response>
    [HttpPost]
    public async Task<ActionResult<Pet>> PostPet(Pet pet)
    {
        _context.Pets.Add(pet);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPetById), new { id = pet.Id }, pet);
    }

    /// <summary>
    /// Atualiza um pet existente.
    /// </summary>
    /// <param name="id">ID do pet.</param>
    /// <param name="pet">Dados atualizados do pet.</param>
    /// <response code="204">Pet atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPet(int id, Pet pet)
    {
        if (id != pet.Id)
        {
            return BadRequest();
        }

        _context.Entry(pet).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove um pet pelo ID.
    /// </summary>
    /// <param name="id">ID do pet.</param>
    /// <response code="204">Pet removido com sucesso.</response>
    /// <response code="404">Pet não encontrado.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePet(int id)
    {
        var pet = await _context.Pets.FindAsync(id);

        if (pet == null)
        {
            return NotFound();
        }

        _context.Pets.Remove(pet);

        await _context.SaveChangesAsync();

        return NoContent();
    }

}