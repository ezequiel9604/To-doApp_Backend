
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PassRecoveryRepository : IPassRecoveryRepository
{
    private readonly IMapper _mapper;
    private readonly TodoAppDbContext _dbContext;

    public PassRecoveryRepository(IMapper mapper, TodoAppDbContext dbContext)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<PassRecoveryDTO>> GetAllAsync()
    {
        try
        {
            var recoveries = await _dbContext.PassRecoveries.ToListAsync();

            var recoveriesDtos = _mapper.Map<List<PassRecoveryDTO>>(recoveries);

            return recoveriesDtos;

        }
        catch (Exception)
        {
            return new List<PassRecoveryDTO>();
        }
    }

    public async Task<PassRecoveryDTO> GetByIdAsync(int id)
    {
        try
        {
            var recovery = await _dbContext.PassRecoveries
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if(recovery is null)
                return new PassRecoveryDTO();

            var recoveryDto = _mapper.Map<PassRecoveryDTO>(recovery);

            return recoveryDto;

        }
        catch (Exception)
        {
            return new PassRecoveryDTO();
        }
    }

    public async Task<List<PassRecoveryDTO>> GetByUserId(int id)
    {
        try
        {
            var recoveries = await _dbContext.PassRecoveries
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (recoveries is null)
                return new List<PassRecoveryDTO>();

            var recoveryDto = _mapper.Map<List<PassRecoveryDTO>>(recoveries);

            return recoveryDto;
        }
        catch (Exception)
        {
            return new List<PassRecoveryDTO>();
        }
    }

    public async Task<string> CreateAsync(PassRecoveryDTO recoveryDTO)
    {

        if(string.IsNullOrEmpty(recoveryDTO.Code) || recoveryDTO.Date is null)
            return "No empty values allow!";

        try
        {

            var recovery = _mapper.Map<PassRecovery>(recoveryDTO);

            await _dbContext.AddAsync(recovery);

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

    public async Task<string> DeleteAsync(PassRecoveryDTO recoveryDTO)
    {
        try
        {
            var recovery = await _dbContext.PassRecoveries
                .Where(x => x.Id == recoveryDTO.Id).FirstOrDefaultAsync();

            if (recovery is null)
                return "No Exists";

            _dbContext.PassRecoveries.Remove(recovery);

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

    public async Task<string> UpdateAsync(PassRecoveryDTO recoveryDTO)
    {
        try
        {
            var recovery = await _dbContext.PassRecoveries
                .Where(x => x.Id == recoveryDTO.Id).FirstOrDefaultAsync();

            if(recovery is null)
                return "No Exists!";

            if (!string.IsNullOrEmpty(recoveryDTO.Code))
                recovery.Code = recoveryDTO.Code;

            if (recoveryDTO.Date is not null)
                recovery.Date = recoveryDTO.Date;

            if (recoveryDTO.Used is false)
                recovery.Used = true;

            _dbContext.PassRecoveries.Update(recovery);

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
}

