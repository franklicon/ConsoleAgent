# ConsoleAgent

A lightweight CLI AI agent built with **.NET 9** that utilizes `Microsoft.Extensions.AI` to orchestrate conversations and function calling across multiple providers (OpenAI and Google Gemini).

## Features

- **Multi-Provider**: Native support for **OpenAI** and **Google Gemini** (via `GeminiDotnet`).
- **Tooling**: Automatic function calling for local services (Weather, Dictionary).
- **Architecture**: Clean Dependency Injection (DI) setup using the generic Host pattern.

## Setup

1. **Clone** the repository.
2. **Configure Environment**: Create a `.env` file in the project root:
   ```env
   OPENAI_API_KEY=...
   GEMINI_API_KEY=...
   WEATHER_API_KEY=...
   ```

## Usage

Run the application via CLI, specifying the provider and model if needed (defaults to `openai` and `gpt-5`).

Example: 
```bash
dotnet run --provider openai --model gtp-5-mini
```


## Capabilities

The agent is pre-configured with the following tools:

| Tool | Function | Description |
| :--- | :--- | :--- |
| **Weather** | `get_weather` | Fetches current weather for a city. |
| **Dictionary** | `get_definitions` | Gets multiple definitions for a specified word. |

## Project Structure

- **Program.cs**: CLI argument parsing and Host builder.
- **Startup.cs**: Service registration (`IChatClient`, `ChatOptions`, `WeatherService`, `DictionaryService`).
- **FunctionRegistry.cs**: Maps C# methods to AI tools.
- **ChatAgent.cs**: Handles the main chat loop and history.