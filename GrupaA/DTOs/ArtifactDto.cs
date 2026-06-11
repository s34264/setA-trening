namespace GrupaA.DTOs;

public class ArtifactDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime OriginDate { get; set; }
    public InstitutionDto Institution { get; set; }  = new InstitutionDto();
}