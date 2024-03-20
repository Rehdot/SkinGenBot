using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace SkinGenBot.Commands;

public class SolidGen : BaseCommandModule
{
    [Command("skin")]
    public async Task SkinCommand(CommandContext ctx, string hex)
    {
        hex = Colors.HexCheck(hex);
        string path = Program.GetPath(hex);
        await ctx.RespondAsync($"Generating skin from hex {hex}...");

        try
        {
            Image.SolidGen(hex);
            if (Program.DebugMode) Console.WriteLine("DEBUG: Skin image generated.");
            Image.SaveImage(hex);
            if (Program.DebugMode) Console.WriteLine($"DEBUG: Skin image saved to {path}.");
        }
        catch (Exception e)
        {
            await ctx.RespondAsync($"Input Error! {hex} is an unsupported format!");
        }

        FileStream file = File.Open(path, FileMode.Open);
        DiscordMessageBuilder builder = new();
        builder.WithFile($"SkinGen_{hex}.png", file);
        builder.WithContent($"Result of hex {hex} skin generation:");
        
        await ctx.RespondAsync(builder);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully sent.");
        
        file.Close();
        File.Delete(path);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully deleted.");
    }

}