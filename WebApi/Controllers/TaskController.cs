
using Domain.DTOs;
using Domain.Repositories;
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

    [HttpGet("GetAll")]
    public async Task<ActionResult<List<UTaskDTO>>> GetAll()
    {
        var tasks = await _taskRepository.GetAllAsync();

        return tasks;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add(UTaskDTO uTaskDTO)
    {   
        
        var result = await _taskRepository.CreateAsync(uTaskDTO);

        if (Convert.ToString(result) == "No empty allow!")
            return BadRequest("Error: There are empty values, No empty values allow!");

        else if (Convert.ToString(result) == "No action!")
            return BadRequest("Error: task not added!");

        else if (Convert.ToString(result) == "Database error!")
            return BadRequest("Error: Request to database failed!");

        return Ok(result);

    }

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

    [HttpPut("Delete")]
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

