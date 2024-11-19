using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.Entities;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GoodsController : ControllerBase
{
    private readonly IGoodRepository _repository;
    private readonly ILogger<IGoodRepository> _logger;

    public GoodsController(IGoodRepository repository, ILogger<IGoodRepository> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<Good>>> GetAll([FromQuery] int? count = null, int? storeId = null)
    {
        try
        {
            var goods = await _repository.GetAll();
            if (goods == null) return NotFound();

            goods = goods.ToList();
            foreach (var product in goods)
            {
                var ids = product.ChildIDs;

                if (!string.IsNullOrEmpty(ids))
                {
                    var arrIds = ids.Split(',').Select(int.Parse).ToList();
                    for (int i = 0; i < arrIds.Count; i++)
                    {
                        var find = goods.SingleOrDefault(o => o.ProductId == arrIds[i]);
                        if (find is not null)
                        {
                            find.Topic = find.Topic.Replace(product.Topic, "").Trim();
                            product.SubProducts.Add(find);
                            find.Parent = product;
                        }
                    }
                }
                long price = 0;
                string? message = null;
                if (count != null && storeId != null)
                {
                    (price, message) = await _repository.CheckExist(product.ProductId, (int)count, (int)storeId);
                    product.Stock = string.IsNullOrEmpty(message);
                }
            }

            goods = goods.OrderBy(i => i.LayoutIndex);
            return Ok(goods);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Goods: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }


    //[HttpGet("/api/[controller]/GetImageById/{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //public async Task<ActionResult<GoodImage>> GetImage(int id)
    //{
    //    try
    //    {
    //        var image = await _repository.GetGoodImageById(id);
    //        if (image == null) return NotFound();

    //        return Ok(image);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("Failed to Get Goods images: {@ex}", ex);
    //        return StatusCode(StatusCodes.Status500InternalServerError,
    //                "Internal Server Error. Please Try Agian Later!");
    //    }
    //}

    [HttpGet("/api/[controller]/GetGoodIma/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetImage(int id)
    {
        try
        {
            var image = await _repository.GetGoodImaById(id);
            if (image == null) return NotFound();

            return File(image, "image/png");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Goods images: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet("/api/[controller]/GetGoodImaLarge/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGoodImaLargeById(int id)
    {
        try
        {
            var image = await _repository.GetGoodImaLargeById(id);
            if (image == null) return NotFound();

            return File(image, "image/png");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Goods images: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet("/api/[controller]/GetUpdatedGoodsId/{lastUpdatedIamge}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<int>>> GetUpdatedGoodsId(string lastUpdatedIamge)
    {
        try
        {
            var ids = await _repository.GetUpdatedGoodsId(lastUpdatedIamge);
            if (ids == null) return NotFound();

            return Ok(ids);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Goods images: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet("/api/[controller]/CheckExist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<long>> CheckExist([FromQuery] int goodId, int count, int storeId)
    {
        try
        {
            var result = await _repository.CheckExist(goodId, count, storeId);
            if (result.Item1 == -1) return BadRequest(result.Item2);

            return Ok(result.Item1);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Check Exist: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
