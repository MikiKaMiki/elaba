using ABAValidatorAPI.Models;
using ABAValidatorAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABAValidatorAPI.Controllers.V2;

[ApiController]
[Route("/api/v2/[controller]")]
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
        return Ok("Hello, I am ABA Validator v2");
    }
    
    [HttpPost("validate-file")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateAbaFile(AbaValidationRequest request)
    {
        _logger.LogInformation("Validating file {fileName}", request.FileName);

        try
        {
            var fileConent = Convert.FromBase64String(request.Base64Content!);

            AbaValidationResponse result = await _abaService
                .ValidateAbaFileAsync(fileConent);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger
                .LogError(
                    "Internal server error. Exception: {ex}. Exception Message: {message}", 
                    ex, 
                    ex.Message);


            return StatusCode(500, "Internal server error");
        }
    }
}
