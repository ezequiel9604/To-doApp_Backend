
namespace Domain.DTOs;

public class PassRecoveryDTO
{

    public int Id { get; set; }

    public string? Code { get; set; }

    public DateTime? Date { get; set; }

    public bool Used { get; set; }


    public int UserId { get; set; }

}

