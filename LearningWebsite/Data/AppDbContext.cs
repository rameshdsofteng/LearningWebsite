using LearningWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningWebsite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; } = null!;
        public DbSet<Learning> Learnings { get; set; } = null!;
        public DbSet<LearningAssignment> LearningAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map ApplicationUser to its own Users table so seeding works independently of Identity
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");

            // Manager-Employee relationship
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Manager)
                .WithMany(u => u.TeamMembers)
                .HasForeignKey(u => u.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure relationships
            modelBuilder.Entity<LearningAssignment>()
                .HasOne(la => la.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(la => la.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LearningAssignment>()
                .HasOne(la => la.Learning)
                .WithMany(l => l.Assignments)
                .HasForeignKey(la => la.LearningId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
