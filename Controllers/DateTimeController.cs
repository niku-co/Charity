using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DateTimeController : ControllerBase
{
    private readonly ILogger<IDateTimeRepository> _logger;
    private readonly IDateTimeRepository _repository;

    public DateTimeController(ILogger<IDateTimeRepository> logger, IDateTimeRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> GetCurrentDateTime()
    {
        try
        {
            var result = await _repository.GetCurrentDateTime();
            if (result == null) return BadRequest();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Current DateTime : {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
