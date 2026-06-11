using GrupaA.DTOs;
using GrupaA.Exceptions;
using GrupaA.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrupaA.Controllers;

[ApiController]
[Route("api/artifacts")]
public class ArtifactsController(IArtifactService service) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddArtifact([FromBody] CreateProjectWithArtifactDto dto)
    {
        try
        {
            await service.AddArtifactWithProjectAsync(dto);
            return Created();
        }
        catch (Exception  e)
        {
            return BadRequest(e.Message);
        }
    }
    
}