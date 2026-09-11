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
    public IActionResult Run(CancellationToken ct)
    {
        _ = Task.Run(() => _ingestionService.RunAsync(ct), ct);
        return Accepted(new { message = "Ingestion started" });
    }
}