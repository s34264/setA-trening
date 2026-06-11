using GrupaA.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GrupaA.Controllers;

[ApiController]
[Route("api/artifacts")]
public class ArtifactsController(IArtifactService service) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddArtifact([FromBody] int CreateArtifactDto)
    {
        try
        {
            return Ok(await service.GetProjectByIdAsync(id));
        }
        catch (NotFoundExcpetion e)
        {
            return NotFound(e.Message);
        }
    }
    
}