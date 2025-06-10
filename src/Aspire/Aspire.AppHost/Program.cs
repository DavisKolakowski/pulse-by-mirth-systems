using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database with PostGIS extension
var postgres = builder.AddPostgres("postgres")
    .WithImage("postgis/postgis")
    .WithImageTag("16-3.4")
    .WithPgWeb()
    .WithPgAdmin();

var pulseDb = postgres.AddDatabase("pulsedb");

// Add Auth0 configuration parameters
var auth0Domain = builder.AddParameter("auth0-domain");
var auth0ClientId = builder.AddParameter("auth0-client-id", secret: true);
var auth0Audience = builder.AddParameter("auth0-audience");

// Add the database migration service - runs before API starts
var migrationService = builder.AddProject<Pulse_Services_DatabaseMigrations>("pulse-migrations")
    .WithReference(pulseDb)
    .WaitFor(postgres);

// Add the API project - using port 6060 as per .env.example
var apiService = builder.AddProject<Pulse_API>("pulse-api")
    .WithReference(pulseDb)
    .WithHttpEndpoint(port: 6060, name: "api-endpoint")
    .WithEnvironment("CLIENT_ORIGIN_URL", "http://localhost:4040")
    .WithEnvironment("AUTH0_DOMAIN", auth0Domain)
    .WithEnvironment("AUTH0_AUDIENCE", auth0Audience)
    .WaitFor(migrationService); // Wait for migrations to complete

// Add the React SPA client - using port 4040 to match Auth0 configuration
var webClient = builder.AddNpmApp("pulse-web-client", @"..\Pulse.Clients.Web")
    .WithReference(apiService)
    .WithHttpEndpoint(port: 4040, env: "PORT")
    .WithEnvironment("VITE_API_SERVER_URL", "http://localhost:6060")
    .WithEnvironment("VITE_AUTH0_DOMAIN", auth0Domain)
    .WithEnvironment("VITE_AUTH0_CLIENT_ID", auth0ClientId)
    .WithEnvironment("VITE_AUTH0_AUDIENCE", auth0Audience)
    .WithEnvironment("VITE_AUTH0_CALLBACK_URL", "http://localhost:4040/callback");

builder.Build().Run();
