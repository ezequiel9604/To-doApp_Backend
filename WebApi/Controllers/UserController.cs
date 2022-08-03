
using Domain.DTOs;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<List<UserDTO>>> GetAll()
    {
        var tasks = await _userRepository.GetAllAsync();

        return tasks;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserDTO userDTO)
    {

        var result = await _userRepository.LoginAsync(userDTO.Email, userDTO.Password);

        if (Convert.ToString(result) == "No empty allow!")
            return BadRequest("Error: There are empty values, No empty values allow!");

        else if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: Email does not exist!");

        else if (Convert.ToString(result) == "No password!")
            return BadRequest("Error: Password is incorrect!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        // returns an object with client's data and token.
        return Ok(result);

    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserDTO userDTO)
    {

        var result = await _userRepository.SignupAsync(userDTO);

        if (Convert.ToString(result) == "No empty allow!")
            return BadRequest("Error: There are empty values, No empty values allow!");

        else if (Convert.ToString(result) == "Already exists!")
            return BadRequest("Error: Email exists already!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: User not added!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [HttpPut("Edit")]
    public async Task<IActionResult> Edit(UserDTO userDTO)
    {

        var result = await _userRepository.UpdateAsync(userDTO);

        if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: Email does not exist!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: User not updated!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var result = await _userRepository.SendMailForgotPassword(email);

        if (Convert.ToString(result) == "No empty allow!")
            return BadRequest("Error: There are empty values, No empty values allow!");

        else if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: Email does not exist!");

        else if (Convert.ToString(result) == "Socket error!")
            return BadRequest("Error: There is a issue with the Socket!");

        else if (Convert.ToString(result) == "Smtp command error!")
            return BadRequest("Error: There is a issue with the Smtp commands!");

        else if (Convert.ToString(result) == "Smtp protocol error!")
            return BadRequest("Error: There is a issue while connecting with Smtp server!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [HttpPost("RestorePassword")]
    public async Task<IActionResult> RestirePassword(UserDTO userDTO)
    {

        var result = await _userRepository.RestorePassword(userDTO.Email, userDTO.Password);

        if (Convert.ToString(result) == "No empty allow!")
            return BadRequest("Error: There are empty values, No empty values allow!");

        else if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: Email does not exist!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: User not updated!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }

}

