
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UTaskRepository : IUTaskRepository
{
    private readonly IMapper _mapper;
    private readonly TodoAppDbContext _dbContext;

    public UTaskRepository(IMapper mapper, TodoAppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<List<UTaskDTO>> GetAllAsync()
    {
        try
        {
            var tasks = await _dbContext.Tasks.ToListAsync();

            var taskDtos = new List<UTaskDTO>();
            foreach (var item in tasks)
            { 
                var tdto = _mapper.Map<UTaskDTO>(item);

                var frequency = await _dbContext.Frequencies
                    .Where(x => x.Id == item.FrequencyId).FirstOrDefaultAsync();
                var category = await _dbContext.Categories
                    .Where(x => x.Id == item.CategoryId).FirstOrDefaultAsync();

                tdto.Category = category.Name;
                tdto.Frequency = frequency.Name;

                taskDtos.Add(tdto);
            }

            return taskDtos;
        }
        catch (Exception)
        {
            return new List<UTaskDTO>();
        }
    }

    public async Task<UTaskDTO> GetByIdAsync(int id)
    {
        try
        {
            var task = await _dbContext.Tasks
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (task == null)
                return new UTaskDTO();

            var taskDto = _mapper.Map<UTaskDTO>(task);

            var frequency = await _dbContext.Frequencies
                .Where(x => x.Id == task.FrequencyId).FirstOrDefaultAsync();
            var category = await _dbContext.Categories
                .Where(x => x.Id == task.CategoryId).FirstOrDefaultAsync();

            taskDto.Category = frequency.Name;
            taskDto.Frequency = category.Name;

            return taskDto;
        }
        catch (Exception)
        {
            return new UTaskDTO();
        }
    }

    public async Task<List<UTaskDTO>> GetByUserId(int id)
    {
        try
        {
            var tasks = await _dbContext.Tasks
                .Where(x => x.UserId == id).ToListAsync();

            if (tasks == null)
                return new List<UTaskDTO>();

            var taskDto = new List<UTaskDTO>();
            foreach (var item in tasks)
            {
                var tskdto = _mapper.Map<UTaskDTO>(item);

                var frequency = await _dbContext.Frequencies
                    .Where(x => x.Id == item.FrequencyId).FirstOrDefaultAsync();
                var category = await _dbContext.Categories
                    .Where(x => x.Id == item.CategoryId).FirstOrDefaultAsync();

                tskdto.Category = category.Name;
                tskdto.Frequency = frequency.Name;

                taskDto.Add(tskdto);

            }

            return taskDto;
        }
        catch (Exception)
        {
            return new List<UTaskDTO>();
        }
    }

    public async Task<string> CreateAsync(UTaskDTO taskDto)
    {
        if(string.IsNullOrEmpty(taskDto.Description) || string.IsNullOrEmpty(taskDto.Category) ||
            string.IsNullOrEmpty(taskDto.Frequency) || taskDto.Hour < 0 || taskDto.Minute < 0 ||
            taskDto.Day < 0 || taskDto.Month < 0 || taskDto.Year < 0)
        {
            return "No empty values allow!";
        }

        try
        {
            var task = _mapper.Map<UTask>(taskDto);
            
            var frequency = await _dbContext.Frequencies
                .Where(x => x.Name == taskDto.Frequency).FirstOrDefaultAsync();
            var category = await _dbContext.Categories
                .Where(x => x.Name == taskDto.Category).FirstOrDefaultAsync();

            if (frequency != null)
                task.FrequencyId = frequency.Id;

            if (category != null)
                task.CategoryId = category.Id;
            

            await _dbContext.Tasks.AddAsync(task);

            int affectedRows = await SaveChangesAsync();

            if (affectedRows > 0)
                return "Success!";

            return "No action!";

        }
        catch (Exception)
        {
            return "Database error!";
        }
    }

    public async Task<string> DeleteAsync(UTaskDTO taskDto)
    {
        try
        {
            var task = await _dbContext.Tasks
                .Where(x => x.Id == taskDto.Id).FirstOrDefaultAsync();

            if (task == null)
                return "No Exists!";

            _dbContext.Tasks.Remove(task);

            int affectedRows = await SaveChangesAsync();

            if (affectedRows > 0)
                return "Success!";

            return "No action!";
        }
        catch (Exception)
        {
            return "Database error!";
        }
    }

    public async Task<string> UpdateAsync(UTaskDTO taskDto)
    {
        try
        {
            var task = await _dbContext.Tasks
                .Where(x => x.Id == taskDto.Id).FirstOrDefaultAsync();

            if (task == null)
                return "No Exists!";

            if(!string.IsNullOrEmpty(taskDto.Description))
                task.Description = taskDto.Description;

            if (!string.IsNullOrEmpty(taskDto.Frequency))
            {
                var frequency = await _dbContext.Frequencies
                    .Where(x => x.Name == taskDto.Frequency).FirstOrDefaultAsync();
                if (frequency != null)
                    task.FrequencyId = frequency.Id;
            }

            if (!string.IsNullOrEmpty(taskDto.Category))
            {
                var category = await _dbContext.Categories
                    .Where(x => x.Name == taskDto.Category).FirstOrDefaultAsync();
                if (category != null)
                    task.CategoryId = category.Id;
            }

            // check daily
            if (taskDto.Hour > 0 && taskDto.Minute > 0)
            {
                DateTime newDate;

                // check weekly
                if (taskDto.Hour > 0 && taskDto.Minute > 0 && taskDto.Day > 0)
                {
                    // check monthly
                    if (taskDto.Hour > 0 && taskDto.Minute > 0 && taskDto.Day > 0
                        && taskDto.Month > 0 && taskDto.Year > 0)
                    {
                        newDate = new DateTime(taskDto.Year, taskDto.Month, taskDto.Day,
                            taskDto.Hour, taskDto.Minute, 0);

                    }
                    else
                    {
                        newDate = new DateTime(task.Date.Value.Year, task.Date.Value.Month,
                            taskDto.Day, taskDto.Hour, taskDto.Minute, 0);
                    }  

                }
                else
                {
                    newDate = new DateTime(task.Date.Value.Year, task.Date.Value.Month,
                        task.Date.Value.Day, taskDto.Hour, taskDto.Minute, 0);
                }

                task.Date = newDate;
            }

            _dbContext.Tasks.Update(task);

            int affectedRows = await SaveChangesAsync();

            if (affectedRows > 0)
                return "Success!";

            return "No action!";

        }
        catch (Exception)
        {
            return "Database error!";
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    
}
