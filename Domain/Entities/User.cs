﻿
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public byte[]? PasswordHash { get; set; }

    [Required]
    public byte[]? PasswordSalt { get; set; }


    // Foreign keys

    public List<UTask>? UTasks { get; set; }

    public List<ChangeOfPassword>? ChangeOfPasswords { get; set; }

    public List<PassRecovery>? PassRecoveries { get; set; }

}

