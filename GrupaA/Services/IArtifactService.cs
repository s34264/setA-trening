namespace GrupaA.Services;
using GrupaA.DTOs;

public interface IArtifactService
{
    public Task AddArtifactWithProjectAsync(CreateProjectWithArtifactDto dto);
}