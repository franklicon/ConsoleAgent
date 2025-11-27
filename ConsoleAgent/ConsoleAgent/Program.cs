using ConsoleAgent;
using dotenv.net;
using Microsoft.Extensions.Hosting;

DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
string provider = "openai";
string model = "gpt-5";

for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--provider" && i + 1 < args.Length)
    {
        provider = args[i + 1].ToLower();
    }

    if (args[i] == "--model" && i + 1 < args.Length)
    {
        model = args[i + 1].ToLower();
    }
}

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
Startup.ConfigureServices(builder, provider, model);
IHost host = builder.Build();

await ChatAgent.RunAsync(host.Services);