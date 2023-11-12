
using BackendAPI.Core;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value is not null) return Ok(result.Value);
        if (result.IsSuccess && result.Value is null) return NotFound();
        return BadRequest(result.Error);
    }
}