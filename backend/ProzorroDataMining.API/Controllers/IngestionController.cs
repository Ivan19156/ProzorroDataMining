namespace ProzorroDataMining.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProzorroDataMining.Application.Services;

[ApiController]
[Route("api/[controller]")]
public sealed class IngestionController : ControllerBase
{
    private readonly ITenderIngestionService _ingestionService;

    public IngestionController(ITenderIngestionService ingestionService)
    {
        _ingestionService = ingestionService;
    }

    [HttpPost("run")]
    public async Task<IActionResult> Run(CancellationToken ct)
    {
        await _ingestionService.RunAsync(ct);
        return Ok("Ingestion completed");
    }
}