using GrupaA.Exceptions;
using GrupaA.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrupaA.Controllers;

[ApiController]
[Route("api/projects")]
public class CustomersController(IProjectService service) : ControllerBase
{

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetProjectById([FromRoute] int id)
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