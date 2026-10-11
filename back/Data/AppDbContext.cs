using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_tarefas.Models;

namespace sistema_gerenciamento_tarefas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuaria> Usuarias { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Nomes das tabelas existentes no banco
        modelBuilder.Entity<Usuaria>()
            .ToTable("Usuarias");
        modelBuilder.Entity<Tarefa>()
            .ToTable("Tarefas");

        // Cada tarefa pertence a uma usuária
        modelBuilder.Entity<Tarefa>()
            .HasOne<Usuaria>()
            .WithMany()
            .HasForeignKey(tarefa => tarefa.UsuariaId);
    }
}