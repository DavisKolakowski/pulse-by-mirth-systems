var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .AddDatabase("pulsedb");

// Add the API project - using port 6060 as per .env.example
var apiService = builder.AddProject("pulse-api", @"..\..\Pulse.API\Pulse.API.csproj")
    .WithReference(postgres)
    .WithHttpEndpoint(port: 6060, name: "api-endpoint")
    .WithEnvironment("CLIENT_ORIGIN_URL", "http://localhost:4040");

// Add the React SPA client - using port 4040 to match Auth0 configuration
var webClient = builder.AddNpmApp("pulse-web-client", @"..\Pulse.Clients.Web")
    .WithReference(apiService)
    .WithHttpEndpoint(port: 4040, env: "PORT")
    .WithEnvironment("VITE_API_SERVER_URL", "http://localhost:6060");

builder.Build().Run();
