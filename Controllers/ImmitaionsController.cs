using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.Entities;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImmitaionsController : ControllerBase
{
    private readonly ILogger<IImitationRepository> _logger;
    private readonly IImitationRepository _repository;
    public ImmitaionsController(ILogger<IImitationRepository> logger, IImitationRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Immitation>>> GetAll()
    {
        try 
        {
            var result = await _repository.GetAll();
            if (result == null || !result.Any()) return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Immitations: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
