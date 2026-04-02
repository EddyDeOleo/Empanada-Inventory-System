using Microsoft.EntityFrameworkCore;
using EmpanadaInventory.Models;

namespace EmpanadaInventory.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Empanada> Empanadas { get; set; } = null!;
    }
}