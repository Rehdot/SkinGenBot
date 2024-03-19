namespace SkinGenBot;

class Program
{
    
    public static int Counter = 1;
    public static bool DebugMode, ConsoleMode, EndMainThread;
    
    public static void Main()
    {
        Init();
        ConsoleApp.Run();
        if (EndMainThread) return;
        Bot.MainAsync().GetAwaiter().GetResult();
    }
    
    private static void Init()
    {
        switch (ConsoleApp.Input("One Of Us Skin Generator v1.0.1\n(D) Debug Bot Mode\n(C) Console Mode\n(B) Bot Mode\n-> ").ToLower())
        {
            case "d":
                DebugMode = true;
                break;
            case "c":
                ConsoleMode = true;
                return;
        }

        Bot.InitJson();
    }

    public static string GetPath(string hex)
    {
        string tempPath = Environment.CurrentDirectory + $"\\SkinGen_{hex}.png";
        if (DebugMode) Console.WriteLine($"DEBUG: GetPath called, directory: {tempPath}");
        return tempPath;
    }
    
    public static string GetPathSplitGen(string hex1, string hex2)
    {
        string tempPath = Environment.CurrentDirectory + $"\\SplitGen_{hex1}_{hex2}.png";
        if (DebugMode) Console.WriteLine($"DEBUG: GetPathSplitGen called, directory: {tempPath}");
        return tempPath;
    }

}