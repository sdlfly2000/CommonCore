using Common.Core.Shared.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Core.ConsoleApp.Test;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<MainActivity>();
        serviceCollection.AddScoped<IMemoryCacheService, MemoryCacheService>();

        serviceCollection.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = "homeserver2:6379,password=sdl@1215";
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mainActivity = serviceProvider.GetRequiredService<MainActivity>();

        mainActivity.Print(new Model { Name = "hello", Age = 20 }, CancellationToken.None).GetAwaiter().GetResult();
    }
}