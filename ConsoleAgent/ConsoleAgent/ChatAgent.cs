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