// API/Controllers/AnalyticsController.cs
namespace ProzorroDataMining.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProzorroDataMining.Application.Services;

[ApiController]
[Route("api/[controller]")]
public sealed class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("savings")]
    public async Task<IActionResult> GetBudgetSavings(CancellationToken ct)
    {
        var result = await _analyticsService.GetBudgetSavingsAsync(ct);
        return Ok(result);
    }

    [HttpGet("top-buyers")]
    public async Task<IActionResult> GetTopBuyers(
        [FromQuery] int top = 5,
        CancellationToken ct = default)
    {
        var result = await _analyticsService.GetTopBuyersAsync(top, ct);
        return Ok(result);
    }

    [HttpGet("top-suppliers")]
    public async Task<IActionResult> GetTopSuppliersAsync(
        [FromQuery] int top = 5,
        CancellationToken ct = default)
    {
        var result = await _analyticsService.GetTopSuppliersAsync(top, ct);
        return Ok(result);
    }
}