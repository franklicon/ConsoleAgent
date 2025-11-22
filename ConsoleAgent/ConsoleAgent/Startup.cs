using ConsoleAgent.Services;
using GeminiDotnet;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using GeminiDotnet.Extensions.AI;

namespace ConsoleAgent;

public static class Startup
{
    public static void ConfigureServices(HostApplicationBuilder builder,
        string provider, string model)
    {
        builder.Services.AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Information));
        builder.Services.AddSingleton<ILoggerFactory>(sp =>
            LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Information)));

        builder.Services.AddSingleton<IChatClient>(sp =>
        {
            ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var client = provider switch
            {
                "openai" => new OpenAI.Chat.ChatClient(model, Environment.GetEnvironmentVariable("OPENAI_API_KEY"))
                    .AsIChatClient(),
                
                "gemini" => new GeminiChatClient(new GeminiClientOptions
                {
                    ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")!,
                    ModelId = model,
                    ApiVersion = GeminiApiVersions.V1Beta
                }),
                
                _ => throw new ArgumentException($"Unknown provider: {provider}")
            };

            return new ChatClientBuilder(client).UseLogging(loggerFactory)
                .UseFunctionInvocation(loggerFactory, c =>
                {
                    c.IncludeDetailedErrors = true;
                }).Build(sp);
        });
        
        builder.Services.AddTransient<ChatOptions>(sp => new ChatOptions
        {   
            ModelId = model,
            Temperature = 1,
            MaxOutputTokens = 5000,
            Tools = [.. sp.GetTools()]
        });

        builder.Services.AddSingleton<WeatherService>(_ =>
        {
            string weatherKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY")!;
            return new WeatherService(weatherKey);
        });
    }
}