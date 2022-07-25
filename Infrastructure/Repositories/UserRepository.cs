
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{

    private readonly TodoAppDbContext _dbContext;

    public UserRepository(TodoAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllAsync()
    {
        try
        {
            var users = await _dbContext.Users.ToListAsync();

            return users;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<User> GetByIdAsync(int id)
    {
        try
        {
            var user = await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            return user;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async void CreateAsync(User user)
    {
        try
        {
            await _dbContext.Users.AddAsync(user);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Delete(User user)
    {
        try
        {
            _dbContext.Users.Remove(user);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Update(User user)
    {
        try
        {
            _dbContext.Users.Update(user);
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

