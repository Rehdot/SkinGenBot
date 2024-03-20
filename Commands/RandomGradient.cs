using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace SkinGenBot.Commands;

public class RandomGradient : BaseCommandModule
{
    [Command("gradientrandom")]
    public async Task GradientRandomCommand(CommandContext ctx)
    {
        string hex1 = Colors.ColorToHex(Colors.GenerateRandomHex());
        string hex2 = Colors.ColorToHex(Colors.GenerateRandomHex());
        string path = Program.GetPathGradient(hex1, hex2);
        
        await ctx.RespondAsync($"Generating random Gradient...");
        try
        {
            Image.GradientGen(hex1, hex2);
            if (Program.DebugMode) Console.WriteLine("DEBUG: Random Gradient image generated.");
            Image.SaveImageGradient(hex1, hex2);
            if (Program.DebugMode) Console.WriteLine($"DEBUG: Skin image saved to {path}.");
        }
        catch (Exception e)
        {
            await ctx.RespondAsync($"Input Error! Hex {hex1} or {hex2} is an unsupported format!");
        }

        FileStream file = File.Open(path, FileMode.Open);
        DiscordMessageBuilder builder = new();
        builder.WithFile($"Gradient_{hex1}_{hex2}.png", file);
        builder.WithContent($"Result of hex {hex1} and {hex2} Gradient:");
        
        await ctx.RespondAsync(builder);
        if (Program.DebugMode) Console.WriteLine("DEBUG: Gradient image successfully sent.");
        file.Close();
        File.Delete(path);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Gradient image successfully deleted.\nPath: {path}");
    }

}