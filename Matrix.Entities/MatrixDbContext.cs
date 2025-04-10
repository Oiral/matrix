using Matrix.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Matrix.Entities;

public class MatrixDbContext(DbContextOptions<MatrixDbContext> options) : DbContext(options)
{
    public DbSet<Bike> Bikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}