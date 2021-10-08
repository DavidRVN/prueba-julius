using Microsoft.EntityFrameworkCore;
using prueba_julius.Models;

namespace prueba_julius.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<usuarios> usuarios { get; set; }
    }
}
