
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ChangeOfPassword
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime? ChangedDate { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

}

