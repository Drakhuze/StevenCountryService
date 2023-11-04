using Microsoft.EntityFrameworkCore;
using StevenCountryService.Models;

namespace StevenCountryService.Data
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
    }
}
