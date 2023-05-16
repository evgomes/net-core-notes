using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add service dependencies.
builder.Services.AddApiDependencies(builder.Configuration);
builder.Services.AddDatabaseDependencies(builder.Configuration);
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// Configure app middlewares pipeline.
var app = builder.Build();
app.ApplyPendingMigrations();
app.UseMiddlewares();

// Run app.
app.Run();
