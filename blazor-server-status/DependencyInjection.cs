using blazor_server_status;
using System;

public static class DependencyInjection
{
    public static void InjectConfiguration(this IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
             .Build();


        services.Configure<ApiConfig>(options => config.GetSection("ApiConfig").Bind(options));

    }
}
