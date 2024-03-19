using Microsoft.EntityFrameworkCore;
using Toolaccounting.Server.Models;

namespace Toolaccounting.Server.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Tool> Tools { get; set; } = null!;
        public DbSet<ToolUsed> ToolsUsed { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();   
            
        }
    }
}
