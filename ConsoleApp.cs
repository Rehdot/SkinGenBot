namespace SkinGenBot;

public class ConsoleApp
{
    
    public static void Run()
    {
        if (!Program.ConsoleMode) return;
        string prompt = "(C) Custom Hex\n(R) Random Hex\n(SC) SplitGen Custom\n(SR) SplitGen Random\n(E) End\n-> ";
        string input = Input($"Console Mode Enabled.\n{prompt}").ToLower();
        string hex = String.Empty;
        string hex2 = String.Empty;
        
        while (input != "e")
        {
            switch (input)
            {
                default:
                    hex = Input("Input Custom Hex: ");
                    Image.SolidGen(hex);
                    Image.SaveImage(false, hex);
                    break;
                case "r":
                    hex = Colors.ColorToHex(Colors.GenerateRandomHex());
                    Image.SolidGen(hex);
                    Image.SaveImage(false, hex);
                    break;
                case "sc":
                    hex = Input("Input SplitGen Hex 1: ");
                    hex2 = Input("Input SplitGen Hex 2: ");
                    Image.SplitGen(hex, hex2);
                    Image.SaveImage(true, hex, hex2);
                    break;
                case "sr":
                    hex = Colors.ColorToHex(Colors.GenerateRandomHex());
                    hex2 = Colors.ColorToHex(Colors.GenerateRandomHex());
                    Image.SplitGen(hex, hex2);
                    Image.SaveImage(true, hex, hex2);
                    break;
            }

            input = Input(prompt).ToLower();
        }
        
        Console.WriteLine("Console Operation ended.\nPress any key to close.");
        Program.EndMainThread = true;
        Console.ReadKey();
    }
    
    public static string Input(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
    
}