using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Jobs.Models;
using Jobs.ViewModels;

namespace Jobs.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Jobs.Models.Category> Category { get; set; }
        public DbSet<Jobs.Models.Job> Job { get; set; }
    }
}