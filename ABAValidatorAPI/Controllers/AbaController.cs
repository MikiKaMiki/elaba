using ABAValidatorAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABAValidatorAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AbaController : ControllerBase
{
    #region fields

    private readonly ILogger<AbaController> _logger;
    private readonly AbaService _abaService;

    #endregion fields

    #region Constructor

    public AbaController(ILogger<AbaController> logger, AbaService abaService)
    {
        _logger = logger;
        _abaService = abaService;
    }

    #endregion Constructor


    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello, I am ABA Validator");
    }
    
    [HttpPost("validate-stream")]
    public async Task ValidateAbaStream()
    {
        using var reader = new StreamReader(Request.Body);

        await WriteResponseAsync(reader);
    }

    #region private

    private async Task WriteResponseAsync(StreamReader reader)
    {
        Response.ContentType = "application/json";

        await Response.WriteAsync("[\n");

        bool isFirstRecord = true;

        await foreach (var result in _abaService.ValidateAbaStreamAsync(reader))
        {
            if (!isFirstRecord)
            {
                await Response.WriteAsync(",\n");
            }

            var json = System.Text.Json.JsonSerializer.Serialize(result);

            await Response.WriteAsync(json);

            isFirstRecord = false;
        }

        await Response.WriteAsync("\n]");
    }

    #endregion private
}
