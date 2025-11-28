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

        DictionaryService dictionaryService = serviceProvider.GetRequiredService<DictionaryService>();
        MethodInfo getDefinitionsFunction = typeof(DictionaryService)
            .GetMethod(nameof(DictionaryService.GetDefinitionsForWord), [typeof(string), typeof(CancellationToken)])!;

        yield return AIFunctionFactory.Create(getDefinitionsFunction, dictionaryService,
            new AIFunctionFactoryOptions
            {
                Name = "get_definitions",
                Description = "Get multiple definitions for a specified word."
            });
    }
}