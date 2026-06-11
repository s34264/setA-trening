using System.ComponentModel.DataAnnotations;

namespace GrupaA.DTOs;

public class CreateProjectDto
{
    [Required]
    public string Objective { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public DateTime StartDarte { get; set; }
    public DateTime? EndDarte { get; set; }
}