using Microsoft.EntityFrameworkCore;
using VeciHub.IAM.Domain.Model;

namespace VeciHub.Shared.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Agrega aquí tus DbSets por módulo
        public DbSet<User> Users { get; set; }
    }
}
