
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Helpers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{ 
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly TodoAppDbContext _dbContext;
    private readonly IUTaskRepository _uTaskRepository;
    private readonly IChangeOfPasswordRepository _changeOfPasswordRepository;
    private readonly IMailRepository _mailRepository;
    private readonly IPassRecoveryRepository _passRecoveryRepository;

    public UserRepository(IMapper mapper, IConfiguration config, 
        TodoAppDbContext dbContext, IMailRepository mailRepository, 
        IChangeOfPasswordRepository changeOfPasswordRepository,
        IPassRecoveryRepository passRecoveryRepository,
        IUTaskRepository uTaskRepository)
    {
        _mapper = mapper;
        _config = config;
        _dbContext = dbContext;
        _mailRepository = mailRepository;
        _uTaskRepository = uTaskRepository;
        _passRecoveryRepository = passRecoveryRepository;
        _changeOfPasswordRepository = changeOfPasswordRepository;
    }

    public async Task<List<UserDTO>> GetAllAsync()
    {
        try
        {
            var users = await _dbContext.Users.ToListAsync();

            var userDtos = new List<UserDTO>();

            foreach (var item in users)
            {
                var udto = _mapper.Map<UserDTO>(item);

                var tsks = await _uTaskRepository.GetByUserId(udto.Id);
                var chgs = await _changeOfPasswordRepository.GetByUserId(udto.Id);

                udto.UTasks = tsks;
                udto.ChangeOfPasswords = chgs;

                userDtos.Add(udto);
            }

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

            var tsks = await _uTaskRepository.GetByUserId(user.Id);
            var chgs = await _changeOfPasswordRepository.GetByUserId(user.Id);

            userDto.UTasks = tsks;
            userDto.ChangeOfPasswords = chgs;

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
            {
                var userr = await _dbContext.Users
                    .Where(x => x.Email == userDto.Email).FirstOrDefaultAsync();

                var changes = new ChangeOfPassword()
                {
                    ChangedDate = DateTime.Now,
                    UserId = userr.Id
                };

                await _dbContext.ChangeOfPasswords.AddAsync(changes);

                await _dbContext.SaveChangesAsync();

                return "Success!";
            }

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
            var user = await _dbContext.Users
                .Where(x => x.Email == userDto.Email).FirstOrDefaultAsync();

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

            if (user is null)
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

                var changeDto = new ChangeOfPasswordDTO()
                {
                    ChangedDate = DateTime.Now,
                    UserId = user.Id
                };

                await _changeOfPasswordRepository.CreateAsync(changeDto);
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
            {
                var userr = await _dbContext.Users
                    .Where(x => x.Email == userDto.Email).FirstOrDefaultAsync();

                var changes = new ChangeOfPassword()
                {
                    ChangedDate = DateTime.Now,
                    UserId = userr.Id
                };

                await _dbContext.ChangeOfPasswords.AddAsync(changes);

                await _dbContext.SaveChangesAsync();

                return "Success!";
            }

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
            var user = await _dbContext.Users
                .Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null)
                return "No exists!";

            if (!Password.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return "No password!";

            var token = Tokens.CreateToken(user, _config);

            var userDto = _mapper.Map<UserDTO>(user);

            /*
            var tsks = await _uTaskRepository.GetByUserId(user.Id);
            var chgs = await _changeOfPasswordRepository.GetByUserId(user.Id);

            userDto.UTasks = tsks;
            userDto.ChangeOfPasswords = chgs;
            */

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

            var user = await _dbContext.Users
                .Where(x => x.Email == email).FirstOrDefaultAsync();


            var recoveryDto = new PassRecoveryDTO()
            {
                Code = RandomCode.Generate(10),
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Used = false,
                UserId = user.Id
            };

            await _passRecoveryRepository.CreateAsync(recoveryDto);


            var mailRequest = new MailRequest()
            {
                ToEmail = email,
                Subject = "Restoring password",
                Body = "Open the link below to start the process of restoring your password. " +
                "http://localhost:3000/restorepassword?User=" + email + "&Code=" + recoveryDto.Code,
            };

            await _mailRepository.SendMailAsync(mailRequest);

            return "Check your email and go to the link!";

        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            return "Database error!";
        }

    }

    public async Task<string> RestorePassword(string email, string password, string code)
    {

        if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(code))
            return "No empty allow!";

        try
        {

            var user = await _dbContext.Users
                .Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user is null)
                return "No Exists!";


            var recovery = await _dbContext.PassRecoveries
                .Where(x => x.Code == code).FirstOrDefaultAsync();

            if(recovery is null)
                return "Wrong code!";

            if(recovery.UserId != user.Id)
                return "Code not match!";

            if(recovery.Date.Value.Year != DateTime.Now.Year || 
                recovery.Date.Value.Month != DateTime.Now.Month
                || recovery.Date.Value.Day != DateTime.Now.Day)
            {
                return "Code expired!";
            }

            await _passRecoveryRepository.UpdateAsync(_mapper.Map<PassRecoveryDTO>(recovery));

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