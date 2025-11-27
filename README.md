# ConsoleAgent

A .NET 9 console AI agent demonstrating `Microsoft.Extensions.AI` for orchestrating function calls with multiple AI providers (OpenAI and Google Gemini). The agent can converse and invoke local tools for weather, inventory, and email tasks.

## Setup

1. Clone the repository.
2. Create a `.env` file in the `ConsoleAgent` directory with your API keys:

## Usage

Run the application via CLI, specifying the provider and model if needed (defaults to `openai` and `gpt-5`).

Example: 
```bash
dotnet run --provider openai --model gtp-5-mini
```

## Available Tools

The agent utilizes the following automatically registered functions:

- **Weather**: `get_weather` - Checks current weather in a city.
- **Wardrobe**: `get_clothing_from_wardrobe` - Lists available clothing items.
- **Email**: `email_friend` - Simulates sending an email to a friend.