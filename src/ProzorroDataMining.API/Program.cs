using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly.Extensions.Http;
using ProzorroDataMining.Application.Http;
using ProzorroDataMining.Application.Services;
using ProzorroDataMining.Data;
using ProzorroDataMining.Data.Repositories;
using ProzorroDataMining.Domain.Interfaces;
using ProzorroDataMining.Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IProzorroClient, ProzorroClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["Prozorro:BaseUrl"]
        ?? ProzorroConstants.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddStandardResilienceHandler(options =>
{
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromSeconds(2);
    options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;
});

builder.Services.AddScoped<ITenderRepository, TenderRepository>();
builder.Services.AddScoped<ITenderIngestionService, TenderIngestionService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

