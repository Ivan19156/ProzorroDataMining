using Microsoft.EntityFrameworkCore;
using ProzorroDataMining.Application.Http;
using ProzorroDataMining.Application.Services;
using ProzorroDataMining.Data;
using ProzorroDataMining.Data.Repositories;
using ProzorroDataMining.Domain.Interfaces;
using ProzorroDataMining.Shared.Constants;
using ProzorroDataMining.Worker;
using Microsoft.Extensions.Http.Resilience;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITenderRepository, TenderRepository>();
builder.Services.AddScoped<ITenderIngestionService, TenderIngestionService>();

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

builder.Services.AddHostedService<IngestionBackgroundService>();

var host = builder.Build();
host.Run();