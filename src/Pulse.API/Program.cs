using Pulse.Core.Extensions;
using Pulse.Core.Data;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

// Add services to the container.
builder.Services.AddPulseByMirthSystems();
builder.Services.AddPulseByMirthSystemsAuthentication(builder.Configuration);
builder.Services.AddPulseByMirthSystemsAuthorization();

// Add database
builder.AddNpgsqlDbContext<ApplicationDbContext>("pulsedb");

builder.Services.AddCors(options =>
{
    var clientOriginUrl = builder.Configuration.GetValue<string>("CLIENT_ORIGIN_URL");
    if (!string.IsNullOrEmpty(clientOriginUrl))
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(clientOriginUrl)
                .WithHeaders(new string[] {
                    HeaderNames.ContentType,
                    HeaderNames.Authorization,
                })
                .WithMethods("GET")
                .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
        });
    }
});

builder.Services.AddControllers();

var app = builder.Build();

var requiredVars =
    new string[] {
          "PORT",
          "CLIENT_ORIGIN_URL",
          "AUTH0_DOMAIN",
          "AUTH0_AUDIENCE",
    };

foreach (var key in requiredVars)
{
    var value = app.Configuration.GetValue<string>(key);

    if (value == "" || value == null)
    {
        throw new Exception($"Config variable missing: {key}.");
    }
}

app.Urls.Add($"http://+:{app.Configuration.GetValue<string>("PORT")}");

app.UsePulseByMirthSystemsMiddleware();
app.MapControllers();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
