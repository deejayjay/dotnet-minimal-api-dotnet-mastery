using Microsoft.EntityFrameworkCore;
using TangyAPI.Models;

namespace TangyAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Appetizer",
                AddedDate = new DateTime(2024, 6, 1)
            },
            new Category
            {
                Id = 2,
                Name = "Entree",
                AddedDate = new DateTime(2024, 4, 1)
            },
            new Category
            {
                Id = 3,
                Name = "Dessert",
                AddedDate = new DateTime(2024, 5, 1)
            }
        );
    }
}
