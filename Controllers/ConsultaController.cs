using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeAPI.Data;
using ChallengeAPI.Models;

namespace ChallengeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultaController : ControllerBase
{
    private readonly AppDbContext _context;

    public ConsultaController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todas as consultas cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultas()
    {
        return Ok(await _context.Consultas.ToListAsync());
    }

    /// <summary>
    /// Busca uma consulta pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Consulta>> GetConsulta(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta == null)
        {
            return NotFound();
        }

        return Ok(consulta);
    }

    /// <summary>
    /// Busca consultas pelo nome do veterinário.
    /// </summary>
    [HttpGet("veterinario/{veterinario}")]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultaPorVeterinario(string veterinario)
    {
        var consultas = await _context.Consultas
            .Where(c => c.Veterinario.Contains(veterinario))
            .ToListAsync();

        if (!consultas.Any())
        {
            return NotFound();
        }

        return Ok(consultas);
    }

    /// <summary>
    /// Busca consultas pelo ID do pet.
    /// </summary>
    [HttpGet("pet/{petId}")]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultaPorPet(int petId)
    {
        var consultas = await _context.Consultas
            .Where(c => c.PetId == petId)
            .ToListAsync();

        if (!consultas.Any())
        {
            return NotFound();
        }

        return Ok(consultas);
    }

    /// <summary>
    /// Cadastra uma nova consulta.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Consulta>> PostConsulta(Consulta consulta)
    {
        _context.Consultas.Add(consulta);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, consulta);
    }

    /// <summary>
    /// Atualiza uma consulta existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutConsulta(int id, Consulta consulta)
    {
        if (id != consulta.Id)
        {
            return BadRequest();
        }

        _context.Entry(consulta).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove uma consulta pelo ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConsulta(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);

        if (consulta == null)
        {
            return NotFound();
        }

        _context.Consultas.Remove(consulta);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}