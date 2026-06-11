namespace GrupaA.DTOs;

public class ProjectDto
{
    public int ProjectId { get; set; }
    public string Objective  { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ArtifactDto Artifact { get; set; }
    public List<StaffDto> StaffAssignments { get; set; }
}