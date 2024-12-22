using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RBAC2.Database.Entities;

namespace RBAC2.Database
{
    public class RbacDbContext : IdentityDbContext<IdentityUser,IdentityRole,string>
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<ClaimDictionary> ClaimDictionaries { get; set; }
        public RbacDbContext(DbContextOptions<RbacDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<User>()
                .HasOne(u => u.IdentityUser)
                .WithOne()
                .HasForeignKey<User>(u => u.IdentityUserId);

            modelBuilder.Entity<ClaimDictionary>(entity =>
            {
                entity.HasKey(e => e.ClaimId);
                entity.Property(e => e.ClaimType).IsRequired();
                entity.Property(e => e.ClaimValue).IsRequired();
            });
        }

    }
}
