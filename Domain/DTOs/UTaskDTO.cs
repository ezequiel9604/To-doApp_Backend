

namespace Domain.DTOs;

public class UTaskDTO
{

    public int Id { get; set; }

    public int Hour { get; set; }

    public int Minute { get; set; }

    public int Day { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public string? Description { get; set; }


    public int UserId { get; set; }

    public string? Category { get; set; }

    public string? Frequency { get; set; }

}

