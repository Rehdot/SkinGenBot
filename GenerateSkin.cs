using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace SkinGenBot;

public class GenerateSkin : BaseCommandModule
{
    [Command("skin")]
    public async Task SkinCommand(CommandContext ctx, string hex)
    {

        hex = Program.PrependPound(hex);

        await ctx.RespondAsync($"Generating skin from hex {hex}...");

        try
        {
            Program.ReplaceOldImage(hex);
            if (Program.DebugMode) Console.WriteLine("DEBUG: Skin image generated.");
            Program.SaveImage(hex);
            if (Program.DebugMode) Console.WriteLine($"DEBUG: Skin image saved to {Program.GetPath(hex)}.");
        }
        catch (Exception e)
        {
            await ctx.RespondAsync($"Input Error! {hex} is an unsupported format!");
        }

        FileStream file = File.Open(Program.GetPath(hex), FileMode.Open);
        
        DiscordMessageBuilder builder = new();
        
        builder.WithFile($"SkinGen_{hex}.png", file);
        builder.WithContent($"Result of hex {hex} skin generation:");

        await ctx.RespondAsync(builder);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully sent.");
        
        file.Close();
        
        File.Delete(Program.GetPath(hex));
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully deleted.");
        
    }

}