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
                "You are a helpful CLI assistant. Use the provided functions when appropriate.")
        ];

        Console.WriteLine("Ask me anything (empty = exit)");

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
        }
    }
}