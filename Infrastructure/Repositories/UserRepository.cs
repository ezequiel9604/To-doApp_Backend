
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Helpers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{ 
    private readonly IMapper _mapper;
    private readonly TodoAppDbContext _dbContext;

    public UserRepository(IMapper mapper, TodoAppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<List<UserDTO>> GetAllAsync()
    {
        try
        {
            var users = await _dbContext.Users.ToListAsync();

            var userDtos = _mapper.Map<List<UserDTO>>(users);

            return userDtos;
        }
        catch (Exception)
        {
            return new List<UserDTO>();
        }
    }

    public async Task<UserDTO> GetByIdAsync(int id)
    {
        try
        {
            var user = await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            var userDto = _mapper.Map<UserDTO>(user);

            return userDto;
        }
        catch (Exception)
        {
            return new UserDTO();
        }
    }

    public async Task<string> CreateAsync(UserDTO userDto)
    {
        if(string.IsNullOrEmpty(userDto.Name) || string.IsNullOrEmpty(userDto.Email) ||
            string.IsNullOrEmpty(userDto.Password))
        {
            return "No empty values allow!";
        }

        try
        {

            if (await CheckUserExists(userDto.Email))
                return "Already exists!";


            Password.CreatePassword(userDto.Password, out byte[] hash, out byte[] salt);

            var user = _mapper.Map<User>(userDto);
            user.PasswordSalt = salt;
            user.PasswordHash = hash;

            await _dbContext.Users.AddAsync(user);

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

    public async Task<string> DeleteAsync(UserDTO userDto)
    {

        if(string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
        {
            return "No empty values allow!";
        }

        try
        {
            var user = await _dbContext.Users.Where(x => x.Email == userDto.Email).FirstOrDefaultAsync();

            if (user == null)
                return "No Exists";

            if (!Password.VerifyPassword(userDto.Password, user.PasswordHash, user.PasswordSalt))
                return "Access Denied!";

            _dbContext.Users.Remove(user);

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

    public async Task<string> UpdateAsync(UserDTO userDto)
    {
        try
        {
            var user = await _dbContext.Users.Where(x => x.Id == userDto.Id).FirstOrDefaultAsync();

            if (user == null)
                return "No Exists!";

            if (!Password.VerifyPassword(userDto.Password, user.PasswordHash, user.PasswordSalt))
                return "Access Denied!";

            if (!string.IsNullOrEmpty(userDto.Name))
                user.Name = userDto.Name;

            if(!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;

            if(!string.IsNullOrEmpty(userDto.Password))
            {
                Password.CreatePassword(userDto.Password, out byte[] hash, out byte[] salt);
                user.PasswordSalt = salt;
                user.PasswordHash = hash;
            }

            _dbContext.Users.Update(user);

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

    public async Task<bool> CheckUserExists(string email)
    {
        try
        {
            var user = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null)
                return false;
            else
                return true;
        }
        catch (Exception)
        {
            throw;
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