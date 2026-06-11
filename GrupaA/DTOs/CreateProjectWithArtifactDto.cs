using System.ComponentModel.DataAnnotations;

namespace GrupaA.DTOs;

public class CreateProjectWithArtifactDto
{
    [Required]
    public CreateArtifactDto Artifact { get; set; }
    [Required]
    public CreateProjectDto Project { get; set; }
}