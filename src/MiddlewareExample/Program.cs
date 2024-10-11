var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

// Example middleware that assigns an anonymous method to the Use method.
// Since the Use method is used to chain multiple request delegates, it also expects a Func delegate
// as a parameter, that we can call using the Invoke method.
app.Use(async (context, next) =>
{
  // Calling the next.Invoke after we send a response to the client will cause an error since
  // properties of the request cannot be set after the response has already been sent.
  //await context.Response.WriteAsync("This will cause a ResponseAlreadyStartedException");

  Console.WriteLine($"1. Logic before executing the next delegate in the Use method.");
  await next.Invoke();
  Console.WriteLine($"3. Logic after executing the next delegate in the Use method.");
});

// Example middleware that assigns an anonymous method to the Run method. Since the Run method is
// always terminal (end), any component added with this method will terminate the pipeline.
// This is why the response from the controller is not returned.
app.Run(async context =>
{
  Console.WriteLine($"2. Writing the response to the client in the Run method.");
  context.Response.StatusCode = 200;
  await context.Response.WriteAsync("Hello from the middleware component.");
});

app.MapControllers();

app.Run();
