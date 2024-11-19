using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.Entities;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GoodTypesController : ControllerBase
{
    private readonly IGoodTypeRepository _repository;
    private readonly ILogger<GoodTypesController> _logger;

    public GoodTypesController(IGoodTypeRepository repository, ILogger<GoodTypesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet("{kioskId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<GoodType>>> GetAll(string kioskId)
    {
        try
        {
            var parseResult = int.TryParse(kioskId, out int kioskIdint);
            if (!parseResult) return BadRequest("kioskId is Not valid!");

            var goodTypes = await _repository.GetAll();
            if (goodTypes == null) return NotFound("هیچ دسته‌ای تعریف نشده است!");

            var goodTypeList = goodTypes.ToList();
            var visitedList = new List<GoodType>();
            foreach (GoodType goodType in goodTypeList)
            {
                goodType.LayOutID ??= 1;
                if (string.IsNullOrWhiteSpace(goodType.PageManagement)) return BadRequest("مقدار فیلد PageManagement نامعتبر است!");
                if (goodType.KioskIDs is not null)
                {
                    if (goodType.KioskIDs.Length > 0)
                    {
                        goodType.Visited = goodType.KioskIDs.Any(id => id == kioskIdint);
                    }
                    //if (goodType.KioskIDs.Length == 0)
                    //    goodType.Visited = true;
                    //else
                    //goodType.Visited = goodType.KioskIDs.Any(id => id == kioskIdint);
                }
                //if (!goodType.Visited) list.RemoveAt(i); 
                if (goodType.Visited) visitedList.Add(goodType);
            }

            if (visitedList.Count == 0) return NotFound("هیچ دسته‌ای برای کیوسک تعریف نشده است!");

            visitedList = [.. visitedList.OrderBy(i => i.LayOutIndex)];

            return Ok(visitedList);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get GoodTypes: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }


    //[HttpGet("/api/[controller]/GetImage/{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //public async Task<ActionResult<GoodTypeImage>> GetImage(int id)
    //{
    //    try
    //    {
    //        var iamge = await _repository.GetGoodTypeImageById(id);
    //        if (iamge == null) return NotFound();

    //        return Ok(iamge);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("Failed to Get Good images: {@ex}", ex);
    //        return StatusCode(StatusCodes.Status500InternalServerError,
    //                "Internal Server Error. Please Try Agian Later!");
    //    }
    //}

    [HttpGet("/api/[controller]/GetGoodTypeIma12/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGoodTypeIma12(int id)
    {
        try
        {
            var image = await _repository.GetGoodTypeIma12ById(id);
            if (image == null) return NotFound();

            return File(image, "image/png");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Good images: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet("/api/[controller]/GetGoodTypeGif/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGoodTypeGif(int id)
    {
        try
        {
            var image = await _repository.GetGoodTypeGifById(id);
            if (image == null) return NotFound();

            return File(image, "image/gif");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Good images: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet("/api/[controller]/GetUpdatedGoodTypesId/{lastImageUpdate}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<int>>> GetUpdatedGoodTypesId(string lastImageUpdate)
    {
        try
        {
            var ids = await _repository.GetUpdatedGoodTypesId(lastImageUpdate);
            if (ids == null) return NotFound();

            return Ok(ids);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Good images: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
