using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeAPI.Data;
using ChallengeAPI.Models;

namespace ChallengeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacinasController : ControllerBase
{
    private readonly AppDbContext _context;

    public VacinasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna todas as vacinas cadastradas.
    /// </summary>
    /// <returns>Lista de vacinas.</returns>
    /// <response code="200">Vacinas encontradas com sucesso.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vacina>>> GetVacinas()
    {
        var vacinas = await _context.Vacinas
            .Include(v => v.Pet)
            .ToListAsync();

        return Ok(vacinas);
    }

    /// <summary>
    /// Busca uma vacina pelo ID.
    /// </summary>
    /// <param name="id">ID da vacina.</param>
    /// <response code="200">Vacina encontrada.</response>
    /// <response code="404">Vacina não encontrada.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Vacina>> GetVacinaById(int id)
    {
        var vacina = await _context.Vacinas
            .Include(v => v.Pet)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vacina == null)
        {
            return NotFound();
        }

        return Ok(vacina);
    }

    /// <summary>
    /// Busca vacinas pelo nome.
    /// </summary>
    /// <param name="nome">Nome da vacina.</param>
    /// <response code="200">Vacinas encontradas.</response>
    /// <response code="404">Nenhuma vacina encontrada.</response>
    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<IEnumerable<Vacina>>> GetVacinaPorNome(string nome)
    {
        var vacinas = await _context.Vacinas
            .Where(v => v.NomeVacina.Contains(nome))
            .ToListAsync();

        if (!vacinas.Any())
        {
            return NotFound();
        }

        return Ok(vacinas);
    }

    /// <summary>
    /// Busca vacinas pela data de aplicação.
    /// </summary>
    /// <param name="data">Data da aplicação da vacina.</param>
    /// <response code="200">Vacinas encontradas.</response>
    /// <response code="404">Nenhuma vacina encontrada.</response>
    [HttpGet("data-aplicacao/{data}")]
    public async Task<ActionResult<IEnumerable<Vacina>>> GetVacinaPorData(string data)
    {
        var vacinas = await _context.Vacinas
            .Where(v => v.DataAplicacao.ToString().Contains(data))
            .ToListAsync();

        if (!vacinas.Any())
        {
            return NotFound();
        }

        return Ok(vacinas);
    }

    /// <summary>
    /// Busca vacinas pela próxima dose.
    /// </summary>
    /// <param name="data">Data da próxima dose.</param>
    /// <response code="200">Vacinas encontradas.</response>
    /// <response code="404">Nenhuma vacina encontrada.</response>
    [HttpGet("proxima-dose/{data}")]
    public async Task<ActionResult<IEnumerable<Vacina>>> GetVacinaPorProximaDose(string data)
    {
        var vacinas = await _context.Vacinas
            .Where(v => v.ProximaDose.ToString().Contains(data))
            .ToListAsync();

        if (!vacinas.Any())
        {
            return NotFound();
        }

        return Ok(vacinas);
    }

    /// <summary>
    /// Cadastra uma nova vacina.
    /// </summary>
    /// <param name="vacina">Dados da vacina.</param>
    /// <response code="201">Vacina criada com sucesso.</response>
    [HttpPost]
    public async Task<ActionResult<Vacina>> PostVacina(Vacina vacina)
    {
        _context.Vacinas.Add(vacina);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVacinaById), new { id = vacina.Id }, vacina);
    }

    /// <summary>
    /// Atualiza uma vacina existente.
    /// </summary>
    /// <param name="id">ID da vacina.</param>
    /// <param name="vacina">Dados atualizados da vacina.</param>
    /// <response code="204">Vacina atualizada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVacina(int id, Vacina vacina)
    {
        if (id != vacina.Id)
        {
            return BadRequest();
        }

        _context.Entry(vacina).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove uma vacina pelo ID.
    /// </summary>
    /// <param name="id">ID da vacina.</param>
    /// <response code="204">Vacina removida com sucesso.</response>
    /// <response code="404">Vacina não encontrada.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVacina(int id)
    {
        var vacina = await _context.Vacinas.FindAsync(id);

        if (vacina == null)
        {
            return NotFound();
        }

        _context.Vacinas.Remove(vacina);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}