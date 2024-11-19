using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.Entities;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(ILogger<IOrderRepository> logger, IOrderRepository repository) : ControllerBase
{
    private readonly ILogger<IOrderRepository> _logger = logger;
    private readonly IOrderRepository _repository = repository;

    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        try
        {
            var result = await _repository.GetOrderById(orderId);
            if (result == Guid.Empty) return NotFound();
            await _repository.DeleteOrder(orderId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Delete Order by id {@orderId}: {@ex}", orderId, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<OrderResponse>> SaveOrder(SaveOrder order)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _repository.SaveOrder(order);
            if (result == null) return BadRequest();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Save Order: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpPost("UpdateDelivery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<OrderGood>>> UpdateDelivery(string barcode)
    {
        try
        {
            if (barcode.Length < 13)
            {
                _logger.LogError("Failed to UpdateDelivery: {@barcode} بارکد اشتباه است!", barcode);
                return BadRequest(new Response("بارکد اشتباه است!", false,  null));
            }
            var result = await _repository.UpdateDelivery(barcode);
            return result.Match<ActionResult<List<OrderGood>>>(orderGoods =>
             Ok(new Response("عملیات با موفقیت انجام شد.", true, orderGoods))
            , error => BadRequest(new Response(error, false, null)));
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to UpdateDelivery: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new Response("Internal Server Error. Please Try Agian Later!", false, null));
        }
    }
    [HttpGet("GetLastCashTransaction")]
    public async Task<IActionResult> GetLastCashTransaction([FromQuery] string pcName)
    {
        try
        {
            _logger.LogInformation("Fetching the last cash transaction for PC Name: {PcName}", pcName);
            var result = await _repository.GetLastCashTransaction(pcName);
            if (result == null)
            {
                _logger.LogWarning("No cash transaction found for PC Name: {PcName}", pcName);
                return NotFound("تراکنش نقدی برای دستگاه وارد شده یافت نشد");
            }
            _logger.LogInformation("Successfully fetched the last cash transaction for PC Name: {PcName}", pcName);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the last cash transaction for PC Name: {PcName}", pcName);
            return StatusCode(500, "خطایی در سمت سرور در اجرای کوئری مربوط به یافتن آخرین تراکنش نقدی رخ داده است");
        }
    }
}
