using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Northwind.App.Backend.Models;
using Serilog;

// Configure Serilog bootstrap logger for startup errors
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

try
{
    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

    var builder = WebApplication.CreateBuilder(args);

    // Use Serilog as the logging provider - reads configuration from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    // Entity Framework - SQLite database (read-only in container)
    builder.Services.AddDbContext<NorthwindContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("NorthwindDb")));

    // JWT Authentication
    var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
    var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Northwind.App.Backend";
    var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "Northwind.App.Frontend";

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ClockSkew = TimeSpan.Zero
            };
        });
    builder.Services.AddAuthorization();

    // Health checks
    builder.Services.AddHealthChecks();

    // Problem Details for consistent error responses (RFC 7807)
    builder.Services.AddProblemDetails();

    // Response compression for better performance
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });

    // Add CORS policy to allow all origins
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
                });
    });

    // Add Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Northwind.App.Backend",
            Version = "v1",
            Description = "Northwind.App.Backend API"
        });

        // JWT Bearer authentication in Swagger
        c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });
        c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("bearer", document)] = []
        });
    });

    var app = builder.Build();

    // Global exception handling - returns Problem Details
    app.UseExceptionHandler();
    app.UseStatusCodePages();

    // Response compression
    app.UseResponseCompression();

    // Serilog request logging - logs all HTTP requests
    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRouting();
    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Health check endpoints
    app.MapHealthChecks("/health");

    // Root endpoint - API information page (reads app name from assembly metadata)
    app.MapGet("/", () =>
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var productName = assembly.GetCustomAttribute<System.Reflection.AssemblyProductAttribute>()?.Product ?? "Northwind API Backend";
        var version = assembly.GetName().Version?.ToString() ?? "0.0.0";

        var html = $$"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>{{productName}}</title>
            <style>
                body {
                    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
                    max-width: 800px;
                    margin: 50px auto;
                    padding: 20px;
                    background: #f5f5f5;
                }
                .container {
                    background: white;
                    padding: 2rem;
                    border-radius: 8px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                }
                h1 { color: #0066cc; margin-top: 0; }
                .info { background: #f0f8ff; padding: 15px; border-radius: 5px; margin: 20px 0; }
                .links { margin-top: 20px; }
                .links a { display: inline-block; margin: 5px 10px 5px 0; padding: 8px 16px; background: #0078d4; color: white; text-decoration: none; border-radius: 4px; }
                .links a:hover { background: #106ebe; }
                .version { color: #666; font-size: 0.9em; margin-top: 10px; }
            </style>
        </head>
        <body>
            <div class="container">
                <h1>🚀 {{productName}}</h1>
                <p class="version">Version {{version}}</p>
                <p>This is a <strong>REST API backend</strong> application built with ASP.NET Core.</p>
                
                <div class="info">
                    <p><strong>ℹ️ Information:</strong></p>
                    <p>This is a demo/test API demonstrating modern web API best practices including JWT authentication, Entity Framework Core, and OpenAPI documentation.</p>
                </div>

                <div class="links">
                    <a href="/swagger">📖 API Documentation (Swagger)</a>
                    <a href="/health/live">✅ Health Check</a>
                    <a href="/version">📋 Version</a>
                    <a href="/appname">📛 App Name</a>
                    <a href="/appinfo">ℹ️ App Info</a>
                </div>
            </div>
        </body>
        </html>
        """;

        return Results.Content(html, "text/html");
    });

    Log.Information("Northwind.App.Backend starting...");

    // Log local URL in Development mode
    if (app.Environment.IsDevelopment())
    {
        var urls = app.Configuration["ASPNETCORE_URLS"] ?? app.Configuration["urls"] ?? "http://localhost:5000";
        Log.Information("Running in Development mode - Local URL: {Urls}", urls);
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}