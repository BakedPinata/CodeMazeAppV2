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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
