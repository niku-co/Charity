using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.Entities;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ILogger<ICustomerRepository> _logger;
    private readonly ICustomerRepository _repository;
    public CustomersController(ILogger<ICustomerRepository> logger, ICustomerRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("{phoneNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Customer>> GetCustomerByPhoneNumber(string mobile)
    {
        try
        {
            var result = await _repository.GetCustomerByPhoneNumber(mobile);
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Customer by phoneNumber {@phoneNumber}: {@ex}", mobile, ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }

    }

    [HttpGet("/api/[controller]/CheckFullName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> CheckFullName([FromQuery] string mobile, string name, string family)
    {
        try
        {
            var result = await _repository.GetCustomerByPhoneNumber(mobile);
            if (result == null) return NotFound();

            var isEqual = string.IsNullOrEmpty(result.FullName) || result.FullName.Replace("\t", "").Replace(" ", "").Equals((name + family).Replace(" ", ""));
            return Ok(isEqual);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Customer by phoneNumber {@phoneNumber}: {@ex}", mobile, ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }

    }

    [HttpGet("/api/[controller]/OrdersCount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> GetCustomerOrdersCount([FromQuery] CustomerCountDTO customerDTO)
    {
        try
        {
            var result = await _repository.GetCustomerOrdersCount(customerDTO);
            if (result == 0) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Customer's Orders Count by {@customerDTO}: {@ex}", customerDTO, ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }

    }

    [HttpPost("/api/[controller]/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> AddCustomer([FromBody] Customer customer)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _repository.AddCustomer(customer);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Add Customer {@customer}: {@ex}", customer, ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}
