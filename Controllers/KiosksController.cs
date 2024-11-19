using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class KiosksController : ControllerBase
{
    private readonly ILogger<IKioskRepository> _logger;
    private readonly IKioskRepository _repository;
    public KiosksController(ILogger<IKioskRepository> logger, IKioskRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> GetKioskId([FromQuery] string ipAddress="", string pcName="")
    {
        try
        {
            var result = await _repository.GetKioskId(ipAddress, pcName);
            if (result == 0) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get kiosk id: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
