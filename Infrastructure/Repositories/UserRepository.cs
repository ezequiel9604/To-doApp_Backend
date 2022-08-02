
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Helpers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;
using MailKit.Net.Smtp;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{ 
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly TodoAppDbContext _dbContext;
    private readonly IMailRepository _mailRepository;

    public UserRepository(IMapper mapper, IConfiguration config, 
        TodoAppDbContext dbContext, IMailRepository mailRepository)
    {
        _mapper = mapper;
        _config = config;
        _dbContext = dbContext;
        _mailRepository = mailRepository;
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

    public async Task<string> SignupAsync(UserDTO userDto)
    {

        if (string.IsNullOrEmpty(userDto.Name) || string.IsNullOrEmpty(userDto.Email) ||
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

    public async Task<object> LoginAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return "No empty allow!";

        try
        {
            var user = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null)
                return "No exists!";

            if (!Password.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return "No password!";

            var token = Tokens.CreateToken(user, _config);

            var userDto = _mapper.Map<UserDTO>(user);

            return new { Token = token, User = userDto };

        }
        catch (Exception)
        {
            return "Database error!";
        }

    }

    public async Task<string> LogoutAsync(string email)
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

    public async Task<string> SendMailForgotPassword(string email)
    {

        if(string.IsNullOrEmpty(email))
            return "No empty allow!";

        try
        {

            if(!await CheckUserExists(email))
                return "No Exists!";

            var mailRequest = new MailRequest()
            {
                ToEmail = email,
                Subject = "Restoring password",
                Body = "Click on the link below to start the process of restoring your password. " +
                "https://localhost:3000/restorePassword/User="+email
            };

            await _mailRepository.SendMailAsync(mailRequest);

            return "Success!";

        }
        catch (SocketException)
        {
            return "Socket error!";
        }
        catch (SmtpCommandException)
        {
            return "Smtp command error!";
        }
        catch (SmtpProtocolException)
        {
            return "Smtp protocol error!";
        }
        catch (Exception)
        {
            return "Database error!";
        }

    }

    public async Task<string> RestorePassword(string email, string password)
    {

        if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return "No empty allow!";


        try
        {

            var user = await _dbContext.Users
                .Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null)
                return "No Exists!";

            Password.CreatePassword(password, out byte[] hash, out byte[] salt);
            user.PasswordSalt = salt;
            user.PasswordHash = hash;

            _dbContext.Users.Update(user);

            var affectedRows = await SaveChangesAsync();

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