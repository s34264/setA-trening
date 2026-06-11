using System.ComponentModel.DataAnnotations;

namespace GrupaA.DTOs;

public class CreateArtifactDto
{
    [Required]
    public string Name { get; set; } = String.Empty;
    [Required]
    public DateTime OriginDate { get; set; }
    [Required]
    public int InstitutionId { get; set; }
}