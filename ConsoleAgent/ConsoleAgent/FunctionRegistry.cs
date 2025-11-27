using System.Reflection;
using ConsoleAgent.Services;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAgent;

public static class FunctionRegistry
{
    public static IEnumerable<AITool> GetTools(this IServiceProvider serviceProvider)
    {
        WeatherService weatherService = serviceProvider.GetRequiredService<WeatherService>();
        MethodInfo getWeatherFunction = typeof(WeatherService)
            .GetMethod(nameof(WeatherService.GetWeatherInCity), [typeof(string), typeof(CancellationToken)])!;

        yield return AIFunctionFactory.Create(getWeatherFunction, weatherService,
            new AIFunctionFactoryOptions
            {
                Name = "get_weather",
                Description = "Get the current weather descriptions in a specified city"
            });

        WardrobeService wardrobeService = serviceProvider.GetRequiredService<WardrobeService>();
        MethodInfo getWardrobeFunction = typeof(WardrobeService)
            .GetMethod(nameof(WardrobeService.ListClothing), [])!;

        yield return AIFunctionFactory.Create(getWardrobeFunction, wardrobeService,
            new AIFunctionFactoryOptions
            {
                Name = "get_clothing_from_wardrobe",
                Description = "List all the clothing I have in my wardrobe"
            });

        EmailService emailService = serviceProvider.GetRequiredService<EmailService>();
        MethodInfo getEmailFunction = typeof(EmailService)
            .GetMethod(nameof(EmailService.EmailFriend), [typeof(string), typeof(string)])!;

        yield return AIFunctionFactory.Create(getEmailFunction, emailService,
            new AIFunctionFactoryOptions
            {
                Name = "email_friend",
                Description = "Sends an email to my friend with this name"
            });
    }
}