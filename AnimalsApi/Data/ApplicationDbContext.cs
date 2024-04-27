using AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimalsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .ToTable("Animal")
                .HasKey(a => a.IdAnimal);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Animal>()
                .Property(a => a.Description)
                .HasMaxLength(200);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Category)
                .HasMaxLength(200);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Area)
                .HasMaxLength(200);
        }
    }
}
