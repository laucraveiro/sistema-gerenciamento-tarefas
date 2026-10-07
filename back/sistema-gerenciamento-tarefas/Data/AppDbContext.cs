using Microsoft.EntityFrameworkCore;
using sistema_gerenciamento_tarefas.Models;

namespace MinhaPrimeiraApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuaria> Usuarias { get; set; }
    public DbSet<Tarefas> Tarefas { get; set; }
}