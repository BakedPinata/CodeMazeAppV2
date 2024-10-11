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
/// A WebApplication we can use to add middleware components to the application's request
/// pipeline.
/// </summary>
var app = builder.Build();

/// <summary>
/// Configure the HTTP request pipeline.
/// </summary>
#region Configure Middleware

if (app.Environment.IsDevelopment())
  app.UseDeveloperExceptionPage(); 
else
  // Adds the Strict-Transport-Security header
  app.UseHsts();

app.UseHttpsRedirection();
// Enables static files - No path specified so the default wwwroot is used
app.UseStaticFiles();
// Forward proxy headers to the current request - Helps during application deployment
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
  ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
