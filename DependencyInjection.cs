using System;

public static class DependencyInjection
{
    public static void CriarInjecao(this IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
             .Build();
    }
}
