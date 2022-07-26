﻿
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChangeOfPasswordRepository : IChangeOfPasswordRepository
{

    private readonly IMapper _mapper;
    private readonly TodoAppDbContext _dbContext;

    public ChangeOfPasswordRepository(IMapper mapper, TodoAppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<List<ChangeOfPasswordDTO>> GetAllAsync()
    {
        try
        {
            var changes = await _dbContext.ChangeOfPasswords.ToListAsync();

            if (changes.Any())
                return new List<ChangeOfPasswordDTO>();

            var changeDtos = _mapper.Map<List<ChangeOfPasswordDTO>>(changes);

            return changeDtos;
        }
        catch (Exception)
        {
            return new List<ChangeOfPasswordDTO>();
        }
    }

    public async Task<ChangeOfPasswordDTO> GetByIdAsync(int id)
    {
        try
        {
            var change = await _dbContext.ChangeOfPasswords.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (change == null)
                return new ChangeOfPasswordDTO();

            var changeDto = _mapper.Map<ChangeOfPasswordDTO>(change);

            return changeDto;
        }
        catch (Exception)
        {
            return new ChangeOfPasswordDTO();
        }
    }

    public async Task<List<ChangeOfPasswordDTO>> GetByUserId(int id)
    {
        try
        {

            var changes = await _dbContext.ChangeOfPasswords
                .Where(x => x.UserId == id).ToListAsync();

            if (!changes.Any())
                return new List<ChangeOfPasswordDTO>();

            var changesDtos = _mapper.Map<List<ChangeOfPasswordDTO>>(changes);

            return changesDtos;

        }
        catch (Exception)
        {
            return new List<ChangeOfPasswordDTO>();
        }
    }

    public async Task<string> CreateAsync(ChangeOfPasswordDTO changeDto)
    {

        try
        {
            var change = _mapper.Map<ChangeOfPassword>(changeDto);

            await _dbContext.AddAsync(change);

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

    public async Task<string> DeleteAsync(ChangeOfPasswordDTO change)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UpdateAsync(ChangeOfPasswordDTO change)
    {
        throw new NotImplementedException();
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
