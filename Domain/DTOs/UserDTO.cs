﻿
namespace Domain.DTOs;

public class UserDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public List<UTaskDTO>? UTasks { get; set; }

    public List<ChangeOfPasswordDTO>? ChangeOfPasswords { get; set; }

}
