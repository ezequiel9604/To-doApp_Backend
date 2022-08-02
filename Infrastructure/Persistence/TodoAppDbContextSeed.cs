
using Domain.Entities;

namespace Infrastructure.Persistence;

public class TodoAppDbContextSeed
{
    public static IEnumerable<Category> SetPreconfigureCategory()
    {
        return new List<Category>()
        {
            new Category() { Id= 1, Name = "Family" },
            new Category() { Id= 2, Name = "Health" },
            new Category() { Id= 3, Name = "Home" },
            new Category() { Id= 4, Name = "Work" }
        };
    }

    public static IEnumerable<Frequency> SetPreconfigureFrequency()
    {
        return new List<Frequency>()
        {
            new Frequency() { Id= 1, Name = "Daily" },
            new Frequency() { Id= 2, Name = "Weekly" },
            new Frequency() { Id= 3, Name = "Monthly" }
        };
    }

}

