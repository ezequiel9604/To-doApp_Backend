
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Task
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public TimeOnly? TimeOfDay { get; set; }
}
