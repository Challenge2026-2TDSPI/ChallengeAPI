using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeAPI.Data;
using ChallengeAPI.Models;

namespace ChallengeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TutoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public TutoresController(AppDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Retorna todos os tutores cadastrados.
    /// </summary>
    /// <returns>Lista de tutores.</returns>
    /// <response code="200">Tutores encontrados com sucesso.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tutor>>> GetTutores()
    {
        var tutores = await _context.Tutores.ToListAsync();

        return Ok(tutores);
    }

    /// <summary>
    /// Busca um tutor pelo ID.
    /// </summary>
    /// <param name="id">ID do tutor.</param>
    /// <response code="200">Tutor encontrado.</response>
    /// <response code="404">Tutor não encontrado.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Tutor>> GetTutorById(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);

        if (tutor == null)
        {
            return NotFound();
        }

        return Ok(tutor);
    }

    /// <summary>
    /// Busca tutores pelo nome.
    /// </summary>
    /// <param name="nome">Nome do tutor.</param>
    /// <response code="200">Tutores encontrados.</response>
    /// <response code="404">Nenhum tutor encontrado.</response>
    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<IEnumerable<Tutor>>> GetTutorPorNome(string nome)
    {
        var tutores = await _context.Tutores
            .Where(t => t.Nome.Contains(nome))
            .ToListAsync();

        if (!tutores.Any())
        {
            return NotFound();
        }

        return Ok(tutores);
    }

    /// <summary>
    /// Busca tutores pelo e-mail.
    /// </summary>
    /// <param name="email">E-mail do tutor.</param>
    /// <response code="200">Tutor encontrado.</response>
    /// <response code="404">Tutor não encontrado.</response>
    [HttpGet("email/{email}")]
    public async Task<ActionResult<Tutor>> GetTutorPorEmail(string email)
    {
        var tutor = await _context.Tutores
            .FirstOrDefaultAsync(t => t.Email == email);

        if (tutor == null)
        {
            return NotFound();
        }

        return Ok(tutor);
    }

    /// <summary>
    /// Busca tutores pelo telefone.
    /// </summary>
    /// <param name="telefone">Telefone do tutor.</param>
    /// <response code="200">Tutor encontrado.</response>
    /// <response code="404">Tutor não encontrado.</response>
    [HttpGet("telefone/{telefone}")]
    public async Task<ActionResult<Tutor>> GetTutorPorTelefone(string telefone)
    {
        var tutor = await _context.Tutores
            .FirstOrDefaultAsync(t => t.Telefone == telefone);

        if (tutor == null)
        {
            return NotFound();
        }

        return Ok(tutor);
    }

    /// <summary>
    /// Cadastra um novo tutor.
    /// </summary>
    /// <param name="tutor">Dados do tutor.</param>
    /// <response code="201">Tutor criado com sucesso.</response>
    [HttpPost]
    public async Task<ActionResult<Tutor>> PostTutor(Tutor tutor)
    {
        _context.Tutores.Add(tutor);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTutorById), new { id = tutor.Id }, tutor);
    }

    /// <summary>
    /// Atualiza um tutor existente.
    /// </summary>
    /// <param name="id">ID do tutor.</param>
    /// <param name="tutor">Dados atualizados do tutor.</param>
    /// <response code="204">Tutor atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTutor(int id, Tutor tutor)
    {
        if (id != tutor.Id)
        {
            return BadRequest();
        }

        _context.Entry(tutor).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove um tutor pelo ID.
    /// </summary>
    /// <param name="id">ID do tutor.</param>
    /// <response code="204">Tutor removido com sucesso.</response>
    /// <response code="404">Tutor não encontrado.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTutor(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);

        if (tutor == null)
        {
            return NotFound();
        }

        _context.Tutores.Remove(tutor);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}