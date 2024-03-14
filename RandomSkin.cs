using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace SkinGenBot;

public class RandomSkin : BaseCommandModule
{
    [Command("random")]
    public async Task RandomCommand(CommandContext ctx)
    {

        string hex = Program.GenerateRandomHex();
        
        await ctx.RespondAsync($"Generating random hex...");

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

        DiscordMessageBuilder builder = new DiscordMessageBuilder();

        builder.WithFile($"SkinGen_{hex}.png", file);
        builder.WithContent($"Result of random hex {hex}:");

        await ctx.RespondAsync(builder);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully sent.");
        
        file.Close();
        
        File.Delete(Program.GetPath(hex));
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully deleted.");

    }

}