
using Domain.DTOs;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{

    private readonly IUTaskRepository _taskRepository;

    public TaskController(IUTaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [AllowAnonymous]
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<UTaskDTO>>> GetAll()
    {
        var tasks = await _taskRepository.GetAllAsync();

        return tasks;
    }

    [Authorize(Roles = "User")]
    [HttpGet("GetByUserId/{id}")]
    public async Task<ActionResult<List<UTaskDTO>>> GetByUserId(int id)
    {
        var tasks = await _taskRepository.GetByUserId(id);

        return tasks;
    }

    [Authorize(Roles = "User")]
    [HttpPost("Add")]
    public async Task<IActionResult> Add(UTaskDTO uTaskDTO)
    {   
        
        var result = await _taskRepository.CreateAsync(uTaskDTO);

        if (Convert.ToString(result) == "No empty values allow!")
            return BadRequest("Error: No empty values allow!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: task not added!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [Authorize(Roles = "User")]
    [HttpPut("Edit")]
    public async Task<IActionResult> Edit(UTaskDTO uTaskDTO)
    {

        var result = await _taskRepository.UpdateAsync(uTaskDTO);

        if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: There are empty values, No empty values allow!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: task not added!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

    [Authorize(Roles = "User")]
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(UTaskDTO uTaskDTO)
    {

        var result = await _taskRepository.DeleteAsync(uTaskDTO);

        if (Convert.ToString(result) == "No exists!")
            return BadRequest("Error: There are empty values, No empty values allow!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: task not added!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

}

