using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionController : ControllerBase
{

    public VersionController()
    {

    }

    [HttpGet]
    public async Task<ActionResult<string>> GetVersion()
    {

        return Ok("1.1.0");

    }
}
