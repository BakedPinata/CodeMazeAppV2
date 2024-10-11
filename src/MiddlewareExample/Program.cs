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

// Example middleware that assigns an anonymous method to the Map method.
// Map method accepts a pathMatch string parameter it will compare to the start of the request path.
// If they match, the app will execute the branch.
// NOTE: Any component added after the Map method won't be executed even if
// the Run method isn't used in the branch.
app.Map("/usingmapbranch", builder =>
{
  builder.Use(async (context, next) =>
  {
    Console.WriteLine($"A. Map branch logic in the Use Method before the next delegate");
    await next.Invoke();
    Console.WriteLine($"C. Map branch logic in the Use method after the next delegate");
  });
  builder.Run(async context =>
  {
    Console.WriteLine($"B. Map branch response to the client in the run method");
    await context.Response.WriteAsync("Hello from the map branch.");
  });
});

// Example middleware that assigns an anonymous method to the MapWhen method.
// MapWhen method uses the result of a delegate, that accepts HttpContext as a parameter, to
// branch the request pipeline.
app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
{
  builder.Run(async context =>
  {
    await context.Response.WriteAsync("Hello from the MapWhen brach.");
  });
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
