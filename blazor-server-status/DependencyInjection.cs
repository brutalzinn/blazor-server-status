using blazor_server_status;
using blazor_server_status.Data;
using ConfigurationSubstitution;

public static class DependencyInjection
{
    public static IConfiguration Configuration;
    public static void InjectConfiguration(this IServiceCollection services)
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .EnableSubstitutions("%", "%")
            .Build();



        services.Configure<ApiConfig>(options => Configuration.GetSection("ApiConfig").Bind(options));
    }

    public static void InjectServices(this IServiceCollection services)
    {
        services.AddSingleton<RedisService>();
        services.AddSingleton<ServerPingService>();
        services.AddHostedService<StatusBackgroundService>();
    }

    public static void InjectCacheStorage(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = ObterRedisContext();
        });

        string ObterRedisContext()
        {
            var redisContextUrl = Configuration.GetConnectionString("Redis");
            Uri redisUrl;
            bool isRedisUrl = Uri.TryCreate(redisContextUrl, UriKind.Absolute, out redisUrl);
            if (isRedisUrl)
            {
                redisContextUrl = string.Format("{0}:{1},password={2}", redisUrl.Host, redisUrl.Port, redisUrl.UserInfo.Split(':')[1]);
            }
            return redisContextUrl;
        }
    }
}
