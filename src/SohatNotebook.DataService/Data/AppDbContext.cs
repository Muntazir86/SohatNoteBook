using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.Data
{
    public class AppDbContext : IdentityDbContext 
    {
        public virtual DbSet<User> Users {  get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<HealthData> HealthData { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
    }
}