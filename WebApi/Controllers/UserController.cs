
using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<UserDTO>>> GetAll()
    {
        var tasks = await _userRepository.GetAllAsync();

        return tasks;
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserDTO userDTO)
    {

        var result = await _userRepository.SignupAsync(userDTO);

        if (Convert.ToString(result) == "No empty values allow!")
            return BadRequest("Error: No empty values allow!");

        else if (Convert.ToString(result) == "Already exists!")
            return BadRequest("Error: Email exists already!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: User not added!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [Authorize(Roles = "User")]
    [HttpPut("Edit")]
    public async Task<IActionResult> Edit(UserDTO userDTO)
    {

        var result = await _userRepository.UpdateAsync(userDTO);

        if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: User does not exist!");

        if (Convert.ToString(result) == "Already exists!")
            return BadRequest("Error: Email does not exist!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: User not updated!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [AllowAnonymous]
    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(UserDTO userDTO)
    {
        var result = await _userRepository.SendMailForgotPassword(userDTO.Email);

        if (Convert.ToString(result) == "No empty allow!")
            return BadRequest("Error: No empty values allow!");

        else if (Convert.ToString(result) == "No Exists!")
            return BadRequest("Error: Email does not exist!");

        else if (Convert.ToString(result) == "Socket error!")
            return BadRequest("Error: An issue with the Socket!");

        else if (Convert.ToString(result) == "Smtp command error!")
            return BadRequest("Error: An issue with the Smtp commands!");

        else if (Convert.ToString(result) == "Smtp protocol error!")
            return BadRequest("Error: An issue connecting with Smtp server!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [AllowAnonymous]
    [HttpPost("RestorePassword")]
    public async Task<IActionResult> RestorePassword(PassRestoreRequest req)
    {

        var result = await _userRepository.RestorePassword(req.Email, req.Password, req.Code);

        if (Convert.ToString(result) == "No empty allow!")
            return BadRequest("Error: No empty values allow!");

        else if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: Email does not exist!");

        else if (Convert.ToString(result) == "Wrong code!")
            return BadRequest("Error: Recovery code is wrong!");

        else if (Convert.ToString(result) == "Code not match!!")
            return BadRequest("Error: Code do not match with user!");

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

