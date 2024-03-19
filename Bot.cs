using System.Reflection;
using DisCatSharp;
using DisCatSharp.CommandsNext;
using DisCatSharp.Enums;
using Newtonsoft.Json;

namespace SkinGenBot;

public class Bot
{
    
    private static string _botToken = string.Empty;
    
    public static async Task MainAsync()
    {
        var discord = new DiscordClient(new DiscordConfiguration()
        {
            Token = _botToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContent
        });

        var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new List<string> { "?" }
        });
        
        RegisterAllCommands(commands);

        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
    
    // There's probably a better way to do this
    public static void InitJson()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "BotToken.json");

        if (!File.Exists(filePath))
        {
            string token = ConsoleApp.Input("BotToken.json file not found. Creating a new one...\nEnter your bot's token below:\n");
            var botConfig = new { Token = token };
            
            string jsonContent = JsonConvert.SerializeObject(botConfig, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
            Console.WriteLine("BotToken.json was created with your token.");
        }
        
        string fileContent = File.ReadAllText(filePath);
        dynamic botConfigObj = JsonConvert.DeserializeObject(fileContent);
        _botToken = botConfigObj.Token;
    }
    
    private static void RegisterAllCommands(CommandsNextExtension commands)
    {
        var commandClasses = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(BaseCommandModule))).ToList();

        foreach (var commandClass in commandClasses)
        {
            commands.RegisterCommands(commandClass);
        }
    }

}