using ABAValidatorAPI.Middleware;
using ABAValidatorAPI.Services;
using ABAValidatorAPI.Services.Rules;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("/app/logs/aba_validator_log-.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] RequestId: {RequestId} TraceIdentifier: {TraceIdentifier} {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddScoped<AbaService>();
builder.Services.AddSingleton<RecordRules>();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".aba"] = "application/octet-stream";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.UseMiddleware<CorrelationIdResponseMiddleware>();

app.Run();
