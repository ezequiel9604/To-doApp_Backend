
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class UTask
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public TimeOnly? Hour { get; set; }


    public int UserId { get; set; }
    public User? User { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int FrequencyId { get; set; }
    public Frequency? Frequency { get; set; }

}
