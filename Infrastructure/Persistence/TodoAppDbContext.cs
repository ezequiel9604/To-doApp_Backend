
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class TodoAppDbContext : DbContext
{

    public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        builder.Entity<Category>()
            .HasData(TodoAppDbContextSeed.SetPreconfigureCategory());

        builder.Entity<Frequency>()
            .HasData(TodoAppDbContextSeed.SetPreconfigureFrequency());
    }

    public DbSet<User>? Users { get; set; }

    public DbSet<UTask>? Tasks { get; set; }

    public DbSet<Category>? Categories { get; set; }

    public DbSet<Frequency>? Frequencies { get; set; }
    
    public DbSet<ChangeOfPassword>? ChangeOfPasswords { get; set; }

}

