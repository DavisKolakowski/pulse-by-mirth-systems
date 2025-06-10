using Pulse.Core.Data;
using Pulse.Services.DatabaseMigrations;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add OpenTelemetry tracing for the migration service
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

// Add database context using Aspire PostgreSQL integration
builder.AddNpgsqlDbContext<ApplicationDbContext>("pulsedb", configureDbContextOptions: options =>
{
    options.UseNpgsql(npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly(typeof(Worker).Assembly.FullName);
        npgsqlOptions.UseNodaTime();
        npgsqlOptions.UseNetTopologySuite();
    });
    // Configure snake_case naming convention for PostgreSQL
    options.UseSnakeCaseNamingConvention();
});

// Add the database initializer
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// Run the host
await host.RunAsync();