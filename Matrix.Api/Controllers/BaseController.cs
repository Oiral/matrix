using Microsoft.AspNetCore.Mvc;

namespace Matrix.Api.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    
}