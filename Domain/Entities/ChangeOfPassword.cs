
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ChangeOfPassword
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime? ChangedDate { get; set; }

    [Required]
    public int Counter { get; set; }

}

