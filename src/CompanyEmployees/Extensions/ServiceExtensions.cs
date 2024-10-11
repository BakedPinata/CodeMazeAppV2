namespace CompanyEmployees.Extensions;

/// <summary>
/// A place to put all application service extension methods.
/// This will reduce the clutter in the main <see cref="Program.cs"/> class.
/// </summary>
public static class ServiceExtensions
{

  /// <summary>
  /// Configure Cross-Origin Resource Sharing (CORS).
  /// <para>
  /// CORS is a mechanism to give or restrict access rights to applications from different domains.
  /// Mandatory if we want to send requests from different domains to our application.
  /// </para>
  /// </summary>
  /// <param name="services"></param>
  public static void ConfigureCors(this IServiceCollection services) =>
    services.AddCors(options =>
    {
      options.AddPolicy("CorsPolicy", builder =>
        // Okay for now, but should be as restrictive as possible in production
        builder.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

        // An example of a more restrictive policy
        /*builder.WithOrigins("https://example.com")
          .WithMethods("POST", "GET")
          .WithHeaders("accept", "content-type"));*/
    });

  /// <summary>
  /// Configure an IIS integration to help with deployment to IIS.
  /// <para>
  /// For now we are not configuring any options and will use the defaults. For details see <see href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iisoptions?view=aspnetcore-8.0#properties">the docs</see>.
  /// </para>
  /// </summary>
  /// <param name="services"></param>
  public static void ConfigureIISIntegration(this IServiceCollection services) =>
    services.Configure<IISOptions>(options =>
    {
      /*options.AuthenticationDisplayName = null;
      options.AutomaticAuthentication = true;
      options.ForwardClientCertificate = true;*/
    });
}
