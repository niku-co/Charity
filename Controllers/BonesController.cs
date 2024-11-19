using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using NikuAPI.Repository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BonesController : ControllerBase
{
    private readonly ILogger<IBoneRepository> _logger;
    private readonly IBoneRepository _repository;
    public BonesController(ILogger<IBoneRepository> logger, IBoneRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("{nationalCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Bone>> GetBoneById(string nationalCode)
    {
        try
        {
            var result = await _repository.GetBoneById(nationalCode);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Bone by id {@nationalCode}: {@ex}", nationalCode, ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpPut("{nationalCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateBoneById(string nationalCode)
    {
        try
        {
            var result = await _repository.GetBoneById(nationalCode);
            if (result == null) return NotFound();
            if (result.DependantsNumber== 0) return BadRequest("DependantsNumber is zero!");

            var updateResult = await _repository.UpdateBone(nationalCode);
            if (String.IsNullOrEmpty(updateResult)) return BadRequest();
            var dateTime = $"{long.Parse(updateResult):####/##/##-##:##}";
            return Ok(dateTime);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Update Bone by id {@nationalCode}: {@ex}", nationalCode, ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
