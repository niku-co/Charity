using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Diagnostics.Metrics;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StoreController : ControllerBase
{
    private readonly ILogger<IStoreRepository> _logger;
    private readonly IStoreRepository _repository;
    public StoreController(ILogger<IStoreRepository> logger, IStoreRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("/api/[controller]/OrderSetting")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> GetOrderSetting()
    {
        try
        {
            var result = await _repository.GetOrderSetting();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Order Setting: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet("/api/[controller]/StoreId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> GetStoreId()
    {
        try
        {
            var result = await _repository.GetStoreId();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get StoreId: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet("/api/[controller]/StoreIds")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> GetStoreIds()
    {
        try
        {
            var result = await _repository.GetStoreIds();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get StoreIds: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }


    [HttpGet("/api/[controller]/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Store>> GetStore()
    {
        try
        {
            var result = await _repository.GetStore();
            if (result == null) return NotFound();
            var holidayMode = BitConverter.ToInt64(result.HolidayMode, 0);
            var normalMode = BitConverter.ToInt64(result.NormalMode, 0);
            if (result.Mode == 1)
            {
                result.ModeResult.Personal = -(holidayMode >> 0) & 0x1;
                result.ModeResult.Away = -(holidayMode >> 1) & 0x1;
                result.ModeResult.Courier = -(holidayMode >> 2) & 0x1;
                result.ModeResult.Post = -(holidayMode >> 3) & 0x1;
            }
            else
            {
                result.ModeResult.Personal = -(normalMode >> 0) & 0x1;
                result.ModeResult.Away = -(normalMode >> 1) & 0x1;
                result.ModeResult.Courier = -(normalMode >> 2) & 0x1;
                result.ModeResult.Post = -(normalMode >> 3) & 0x1;
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Store: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
