using ABAValidatorAPI.Models;
using ABAValidatorAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABAValidatorAPI.Controllers.V3;

[ApiController]
[Route("/api/v3/[controller]")]
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
        return Ok("Hello, I am ABA Validator v3");
    }
    
    [HttpPost("validate-file")]
    public async Task<IActionResult> ValidateAbaFile(AbaValidationRequest request)
    {
        _logger.LogInformation("Validating file {fileName}", request.FileName);

        try
        {
            var fileContent = Convert.FromBase64String(request.Base64Content!);

            var result = await _abaService.ValidateFileAsync(fileContent);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger
                .LogError(
                    "Internal server error. Exception Message: {message}. Exception Message: {ex}", 
                    ex.Message,
                    ex);

            throw;
        }
    }
}
