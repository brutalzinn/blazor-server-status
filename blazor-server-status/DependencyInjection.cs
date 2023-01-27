using Blazor.Status.Backend.Configs;
using Blazor.Status.Backend.Services;
using ConfigurationSubstitution;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Discord;

public static class DependencyInjection
{
    public static IConfiguration Configuration;

    public static void InjectSeriLog(this WebApplicationBuilder builder)
    {
        var discordWebhookUrl = Environment.GetEnvironmentVariable("DISCORD_WEBHOOK_URL");
        Uri redisUrl;
        ulong webHookId = 0;
        string webhookToken = "";
        bool isDiscordUrl = Uri.TryCreate(discordWebhookUrl, UriKind.Absolute, out redisUrl);
        if (isDiscordUrl)
        {
            var discordIdSegment = redisUrl.Segments[3].TrimEnd('/');
            var discordTokenSegment = redisUrl.Segments[4];
            ulong.TryParse(discordIdSegment, out webHookId);
            webhookToken = discordTokenSegment;
        }
        builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Discord(webHookId, webhookToken, restrictedToMinimumLevel: LogEventLevel.Warning));

    }
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
            options.Configuration = GetRedisContext();
        });

        string GetRedisContext()
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
