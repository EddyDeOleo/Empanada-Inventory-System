using Microsoft.EntityFrameworkCore;
using ProyectoEmpanadas.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Empanada> Empanadas { get; set; }
}