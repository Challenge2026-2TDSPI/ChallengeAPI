using Microsoft.EntityFrameworkCore;
using ChallengeAPI.Models;

namespace ChallengeAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tutor> Tutores { get; set; }

    public DbSet<Pet> Pets { get; set; }

    public DbSet<Vacina> Vacinas { get; set; }

    public DbSet<Consulta> Consultas { get; set; }
}