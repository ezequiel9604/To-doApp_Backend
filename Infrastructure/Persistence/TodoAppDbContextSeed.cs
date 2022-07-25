
using Domain.Entities;

namespace Infrastructure.Persistence;

public class TodoAppDbContextSeed
{

    public static IEnumerable<Category> SetPreconfigureCategory()
    {
        return new List<Category>()
        {
            new Category() { Name = "Family" },
            new Category() { Name = "Health" },
            new Category(){ Name = "Home" },
            new Category{ Name = "Work" }
        };
    }

    public static IEnumerable<Frequency> SetPreconfigureFrequency()
    {
        return new List<Frequency>()
        {
            new Frequency() { Name = "Daily" },
            new Frequency() { Name = "Weekly" },
            new Frequency(){ Name = "Monthly" }
        };
    }

}

