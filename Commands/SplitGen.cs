using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace SkinGenBot.Commands;

public class SplitGen : BaseCommandModule
{
    [Command("splitgen")]
    public async Task SplitGenCommand(CommandContext ctx, string hex1, string hex2)
    {
        hex1 = Colors.HexCheck(hex1);
        hex2 = Colors.HexCheck(hex2);
        string path = Program.GetPathSplitGen(hex1, hex2);
        
        await ctx.RespondAsync($"Generating random SplitGen from {hex1} and {hex2}...");
        try
        {
            Image.SplitGen(hex1, hex2);
            if (Program.DebugMode) Console.WriteLine("DEBUG: SplitGen image generated.");
            Image.SaveImageSplitGen(hex1, hex2);
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