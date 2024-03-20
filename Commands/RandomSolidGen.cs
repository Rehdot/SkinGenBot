using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace SkinGenBot.Commands;

public class RandomSolidGen : BaseCommandModule
{
    [Command("random")]
    public async Task RandomCommand(CommandContext ctx)
    {
        string hex = Colors.ColorToHex(Colors.GenerateRandomHex());
        string path = Program.GetPath(hex);
        await ctx.RespondAsync($"Generating random hex...");
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
        DiscordMessageBuilder builder = new DiscordMessageBuilder();
        builder.WithFile($"SkinGen_{hex}.png", file);
        builder.WithContent($"Result of random hex {hex}:");

        await ctx.RespondAsync(builder);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully sent.");
        
        file.Close();
        File.Delete(path);
        if (Program.DebugMode) Console.WriteLine($"DEBUG: Hex {hex} image successfully deleted.");
    }

}