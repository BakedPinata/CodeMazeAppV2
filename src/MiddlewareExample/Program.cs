var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

// Example middleware that assigns an anonymous method to the Run method. Since the Run method is
// always terminal (end), any component added with this method will terminate the pipeline.
// This is why the response from the controller is not returned.
app.Run(async context =>
{
  await context.Response.WriteAsync("Hello from the middleware component.");
});

app.MapControllers();

app.Run();
