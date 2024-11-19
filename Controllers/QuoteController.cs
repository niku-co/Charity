using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NikuAPI.Entities;
using NikuAPI.IRepository;

namespace NikuAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class QuoteController : ControllerBase
{
    private readonly ILogger<IQuoteRepository> _logger;
    private readonly IQuoteRepository _repository;

    public QuoteController(ILogger<IQuoteRepository> logger, IQuoteRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("/api/[controller]/GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Quote>>> GetAllQuotes()
    {
        try
        {
            var result = await _repository.GetAllQuotes();
            if (result== null || !result.Any()) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Quotes: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Quote>> GetQuote(string quoteType)
    {
        try
        {
            var result = await _repository.GetQuote(quoteType);
            if (result == null || !result.Any()) return NotFound();
            Random rand = new();
            var idx = rand.Next(result.Count());
            return Ok(result.ToList()[idx]);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to Get Quote: {@ex}", ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                    "Internal Server Error. Please Try Agian Later!");
        }
    }
}

