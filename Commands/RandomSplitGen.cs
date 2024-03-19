using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace SkinGenBot.Commands;

public class RandomSplitGen : BaseCommandModule
{
    [Command("splitgenrandom")]
    public async Task RandomCommand(CommandContext ctx)
    {
        string hex1 = Colors.ColorToHex(Colors.GenerateRandomHex());
        string hex2 = Colors.ColorToHex(Colors.GenerateRandomHex());
        string path = Program.GetPathSplitGen(hex1, hex2);
        
        await ctx.RespondAsync($"Generating SplitGen With {hex1} and {hex2}...");
        try
        {
            Image.SplitGen(hex1, hex2);
            if (Program.DebugMode) Console.WriteLine("DEBUG: SplitGen image generated.");
            Image.SaveImage(true, hex1, hex2);
            if (Program.DebugMode) Console.WriteLine($"DEBUG: Skin image saved to {path}.");
        }
        catch (Exception e)
        {
            await ctx.RespondAsync($"Input Error! Hex {hex1} or {hex2} is an unsupported format!");
        }

        FileStream file = File.Open(path, FileMode.Open);
        DiscordMessageBuilder builder = new();
        builder.WithFile($"SplitGen_{hex1}_{hex2}.png", file);
        builder.WithContent($"Result of hex {hex1} and {hex2} SplitGen:");

        await ctx.RespondAsync(builder);
        if (Program.DebugMode) Console.WriteLine("DEBUG: SplitGen image successfully sent.");
        
        file.Close();
        File.Delete(path);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: SplitGen image successfully deleted.\nPath: {path}");
    }

}