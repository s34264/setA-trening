using GrupaA.DTOs;

namespace GrupaA.Services;

public interface IProjectService
{
    public Task<ProjectDto> GetProjectByIdAsync(int id);
}