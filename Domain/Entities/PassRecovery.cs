
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PassRecovery
{

    [Key]
    public int Id { get; set; }

    [Required]
    public string? Code { get; set; }

    [Required]
    public DateTime? Date { get; set; }

    [Required]
    public bool Used { get; set; }


    public int UserId { get; set; }
    public User? User { get; set; }

}

