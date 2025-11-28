using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAgent;

public static class ChatAgent
{
    public static async Task RunAsync(IServiceProvider serviceProvider)
    {
        IChatClient client = serviceProvider.GetRequiredService<IChatClient>();
        ChatOptions chatOptions = serviceProvider.GetRequiredService<ChatOptions>();

        List<ChatMessage> history = [
            new ChatMessage (ChatRole.System, 
                "You are a helpful CLI assistant. Use the provided functions when appropriate." +
                "If a tool call fails due to some invalid arguments, then make an attempt to fix the arguments yourself" +
                "using your best judgment, then try to call the tool again.")
        ];

        Console.WriteLine("Ask me anything (empty = exit)");

        int turnsSinceLastSummary = 0;
        const int summaryInterval = 5;

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                break;
            }

            Console.ResetColor();

            history.Add(new ChatMessage(ChatRole.User, input));

            ChatResponse response = await client.GetResponseAsync(history, chatOptions);
            Console.WriteLine(response.Text);
            history.AddRange(response.Messages);
            turnsSinceLastSummary++;

            if (turnsSinceLastSummary >= summaryInterval)
            {
                string summary = await SummarizeHistory(history, client, chatOptions);
                history = [history[0], new ChatMessage(ChatRole.System, summary)];
                turnsSinceLastSummary = 0;
            }
        }
    }
    
    private static async Task<string> SummarizeHistory(List<ChatMessage> history,
        IChatClient client, ChatOptions chatOptions)
    {
        string summaryPrompt = "Summarize the following conversation in a few sentences:\n\n";
        foreach (ChatMessage message in history)
        {
            summaryPrompt += $"{message.Role}: {message.Text}\n";
        }

        List<ChatMessage> summaryHistory = [new ChatMessage(ChatRole.System, summaryPrompt)];
        ChatResponse summaryResponse = await client.GetResponseAsync(summaryHistory, chatOptions);

        return summaryResponse.Text;
    }
}