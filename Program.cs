using System.Drawing;
using DisCatSharp;
using DisCatSharp.CommandsNext;
using DisCatSharp.Enums;
using Newtonsoft.Json;

namespace SkinGenBot;

class Program
{
    
    private static int _counter = 1;
    private static string _botToken = string.Empty;
    public static bool DebugMode, ConsoleMode, EndMainThread;
    private static readonly Color 
        OldColor = ColorTranslator.FromHtml("#1520a7"),
        OldColorHat1 = ColorTranslator.FromHtml("#000081"),
        OldColorHat2 = ColorTranslator.FromHtml("#000a8d"),
        OldColorHat3 = ColorTranslator.FromHtml("#08149a");
    private static readonly Bitmap
        NewSkin = new Bitmap(64, 64),
        OldImage = new Bitmap(GetImageReference());
    
    
    public static void Main()
    {
        
        Init();
        ConsoleOperation();
        if (EndMainThread) return;
        MainAsync().GetAwaiter().GetResult();
        
    }

    static async Task MainAsync()
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

        commands.RegisterCommands<GenerateSkin>();
        commands.RegisterCommands<RandomSkin>();

        await discord.ConnectAsync();
        await Task.Delay(-1);

    }
    
    private static void Init()
    {
        
        var imageStream = typeof(Program).Assembly.GetManifestResourceStream("BaseSkin.png");

        switch (Input("One Of Us Skin Generator v1.0.0\n(D) Debug Bot Mode\n(C) Console Mode\n(B) Bot Mode\n-> ").ToLower())
        {
            case "d":
                DebugMode = true;
                break;
            case "c":
                ConsoleMode = true;
                return;
        }

        InitJson();

    }

    // There's probably a better way to do this
    public static void InitJson()
    {
        string filePath = Path.Combine(Environment.CurrentDirectory, "BotToken.json");

        if (!File.Exists(filePath))
        {
            string token = Input("BotToken.json file not found. Creating a new one...\nEnter your bot's token below:\n");
            var botConfig = new { Token = token };
            
            string jsonContent = JsonConvert.SerializeObject(botConfig, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);
            Console.WriteLine("BotToken.json was created with your token.");
        }
        
        string fileContent = File.ReadAllText(filePath);
        dynamic botConfigObj = JsonConvert.DeserializeObject(fileContent);
        _botToken = botConfigObj.Token;
    }

    private static Stream GetImageReference()
    {
        string resourcePath = "SkinGenBot.Graphics.BaseSkin.png";
        Stream stream = typeof(Program).Assembly.GetManifestResourceStream(resourcePath);
        if (stream == null)
        {
            throw new Exception($"Resource '{resourcePath}' not found.");
        }
        return stream;
    }

    private static void ConsoleOperation()
    {

        if (!ConsoleMode) return;

        string input = Input("Console Mode Enabled.\n(C) Custom Hex Input\n(R) Random Hex Generator\n(E) End\n-> ").ToLower();
        string hex = String.Empty;
        
        while (input != "e")
        {
            switch (input)
            {
                default:
                    hex = Input("Input Custom Hex: ");
                    break;
                case "r":
                    hex = GenerateRandomHex();
                    break;
            }

            try
            {
                ReplaceOldImage(hex);
                SaveImage(hex);
            }
            catch (Exception e)
            {
                Console.WriteLine("Input error! Hex format unsupported.");
            }

            input = Input("(C) Custom Hex Input\n(R) Random Hex Generator\n(E) End\n-> ").ToLower();

        }
        
        Console.WriteLine("Console Operation ended.\nPress any key to close.");
        EndMainThread = true;
        Console.ReadKey();

    }

    public static string GenerateRandomHex()
    {
        Random random = new();
        Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        string hex = ColorTranslator.ToHtml(color);
        return PrependPound(hex);
    }
    
    public static void SaveImage(string input)
    {
        input = PrependPound(input);
        NewSkin.Save(GetPath(input), System.Drawing.Imaging.ImageFormat.Png);
        Console.WriteLine($"Hex code {input} generated. (Runtime generation #{_counter})");
        if (ConsoleMode) Console.WriteLine($"Skin saved to {GetPath(input)}");
        _counter++;
    }

    private static Color DarkenColor(string hex, float factor)
    {
        
        Color color = ColorTranslator.FromHtml(hex);
        
        int red = (int)(color.R * factor);
        int green = (int)(color.G * factor);
        int blue = (int)(color.B * factor);
        
        red = Math.Max(0, Math.Min(255, red));
        green = Math.Max(0, Math.Min(255, green));
        blue = Math.Max(0, Math.Min(255, blue));

        return Color.FromArgb(red, green, blue);
        
    }

    public static string GetPath(string hex)
    {
        string tempPath = Environment.CurrentDirectory + $"\\SkinGen_{hex}.png";
        
        if (DebugMode) Console.WriteLine($"DEBUG: GetPath called, directory: {tempPath}");
        
        return tempPath;
    }

    public static string PrependPound(string hex)
    {
        return hex.StartsWith("#") ? hex : "#" + hex;
    }

    public static void ReplaceOldImage(string input)
    {
        
        using (Graphics graphics = Graphics.FromImage(NewSkin))
        {
            try
            {
                graphics.DrawImage(OldImage, new Rectangle(0, 0, 64, 64));
                if (DebugMode) Console.WriteLine($"DEBUG: Internal graphic FOUND");
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Internal graphic NOT FOUND\n{e}");
                return;
            }
            
            
            
            for (int y = 0; y < NewSkin.Height; y++)
            {
                
                for (int x = 0; x < NewSkin.Width; x++)
                {

                    switch (NewSkin.GetPixel(x, y))
                    {
                        
                        case var value when value == OldColor:
                            NewSkin.SetPixel(x, y, ColorTranslator.FromHtml(input));
                            break;
                        case var value when value == OldColorHat1:
                            NewSkin.SetPixel(x, y, DarkenColor(input, 0.65f));
                            break;
                        case var value when value == OldColorHat2:
                            NewSkin.SetPixel(x, y, DarkenColor(input, 0.75f));
                            break;
                        case var value when value == OldColorHat3:
                            NewSkin.SetPixel(x, y, DarkenColor(input, 0.85f));
                            break;
                      
                    }
                    
                }
                
            }
            
        }
        
    }

    public static string Input(string prompt)
    {
        
        Console.Write(prompt);
        return Console.ReadLine();

    }

}