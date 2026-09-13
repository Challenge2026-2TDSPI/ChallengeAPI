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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.Property(t => t.Nome).HasMaxLength(150).IsRequired();
            entity.Property(t => t.Telefone).HasMaxLength(20).IsRequired();
            entity.Property(t => t.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(t => t.Email).IsUnique();
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.Property(p => p.Nome).HasMaxLength(100).IsRequired();
            entity.Property(p => p.Especie).HasMaxLength(50).IsRequired();
            entity.Property(p => p.Raca).HasMaxLength(100).IsRequired();
            entity.HasOne(p => p.Tutor)
                .WithMany(t => t.Pets)
                .HasForeignKey(p => p.TutorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Vacina>(entity =>
        {
            entity.Property(v => v.NomeVacina).HasMaxLength(120).IsRequired();
            entity.Property(v => v.DataAplicacao).HasMaxLength(10).IsRequired();
            entity.Property(v => v.ProximaDose).HasMaxLength(10).IsRequired();
            entity.HasOne(v => v.Pet)
                .WithMany(p => p.Vacinas)
                .HasForeignKey(v => v.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.Property(c => c.DataConsulta).HasMaxLength(10).IsRequired();
            entity.Property(c => c.Descricao).HasMaxLength(500).IsRequired();
            entity.Property(c => c.Veterinario).HasMaxLength(150).IsRequired();
            entity.HasOne(c => c.Pet)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
