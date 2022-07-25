
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChangeOfPasswordRepository : IChangeOfPasswordRepository
{

    private readonly TodoAppDbContext _dbContext;

    public ChangeOfPasswordRepository(TodoAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ChangeOfPassword>> GetAllAsync()
    {
        try
        {
            var changes = await _dbContext.ChangeOfPasswords.ToListAsync();

            return changes;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ChangeOfPassword> GetByIdAsync(int id)
    {
        try
        {
            var change = await _dbContext.ChangeOfPasswords.Where(x => x.Id == id).FirstOrDefaultAsync();

            return change;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async void CreateAsync(ChangeOfPassword change)
    {
        try
        {
            await _dbContext.ChangeOfPasswords.AddAsync(change);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Delete(ChangeOfPassword change)
    {
        try
        {
            _dbContext.ChangeOfPasswords.Remove(change);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Update(ChangeOfPassword change)
    {
        try
        {
            _dbContext.ChangeOfPasswords.Update(change);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async void SaveChangesAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

}
