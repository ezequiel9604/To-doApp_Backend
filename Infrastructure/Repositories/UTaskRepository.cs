
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UTaskRepository : IUTaskRepository
{
    private readonly TodoAppDbContext _dbContext;

    public UTaskRepository(TodoAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UTask>> GetAllAsync()
    {
        try
        {
            var tasks = await _dbContext.Tasks.ToListAsync();

            return tasks;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<UTask> GetByIdAsync(int id)
    {
        try
        {
            var task = await _dbContext.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();

            return task;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async void CreateAsync(UTask task)
    {
        try
        {
            await _dbContext.Tasks.AddAsync(task);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Delete(UTask task)
    {
        try
        {
            _dbContext.Tasks.Remove(task);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Update(UTask task)
    {
        try
        {
            _dbContext.Tasks.Update(task);
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
