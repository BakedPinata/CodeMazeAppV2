using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

/// <summary>
/// A WebApplicationBuilder that is responsible for:
/// - Adding Configuration to the project by using the builder.Configuration property
/// - Registering services in our app with the builder.Services property
/// - Logging configuration with the builder.Logging property
/// - Other IHostBuilder and IWebHostBuilder configuration
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Add services to the container.
/// A service is a reusable part of the code that adds some functionality to our application.
/// </summary>
#region Register Services

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

builder.Services.AddControllers();

#endregion

/// <summary>
/// A WebApplication we can use to add middleware components to the application's request pipeline.
/// WebApplication implements multiple interfaces like:
/// - IHost - Can be used to start and stop the host
/// - IApplicationBuilder - Used to build the middleware pipeline
/// - IEndpointRouteBuilder - Used to add endpoints to the app
/// </summary>
var app = builder.Build();

/// <summary>
/// Configure the HTTP request pipeline. Note that the order is important.
/// Each middleware component in the pipeline can:
/// - Pass the request to the next middleware component in the pipeline and also
/// - It can execute some work before and after the next component in the pipeline
/// Pipeline is built using request delegates which handle each request.
/// To configure delegates, the Run, Map and Use extension methods are used.
/// </summary>
#region Configure Middleware

if (app.Environment.IsDevelopment())
  app.UseDeveloperExceptionPage(); 
else
  // Adds the Strict-Transport-Security header
  app.UseHsts();

// Add middleware responsible for forwarding from HTTP to HTTPS
app.UseHttpsRedirection();
// Enables static files - No path specified so the default wwwroot is used
app.UseStaticFiles();
// Forward proxy headers to the current request - Helps during application deployment
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
  ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");
// Adds middleware to the specified IApplicationBuilder to enable authorization capabilities
app.UseAuthorization();
// Adds enpoints from controller actions to the IEndpointRouteBuilder
app.MapControllers();

#endregion

app.Run();
