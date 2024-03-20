using System.Drawing;

namespace SkinGenBot;

public class Colors
{

    public static readonly List<Color>
        SolidColorList = new() { HexToColor("1520a7"), HexToColor("08149a"), HexToColor("000a8d"), HexToColor("000081") }, 
        LeftColorList = new() { HexToColor("2a2a2a"), HexToColor("171717"), HexToColor("0d0d0d"), HexToColor("000000") }, 
        RightColorList = new() { HexToColor("fdbfe7"), HexToColor("d7a2c4"), HexToColor("bd8fad"), HexToColor("a47c96") }, 
        GradientColorList = new() { HexToColor("9000ff"), HexToColor("9400ff"), HexToColor("9900ff"), HexToColor("9d00ff"), 
            HexToColor("a100ff"), HexToColor("a500ff"), HexToColor("aa00ff"), HexToColor("ae00ff"), HexToColor("b200ff"), 
            HexToColor("b600ff"), HexToColor("bb00ff"), HexToColor("bf00ff"), HexToColor("c300ff"), HexToColor("c700ff"), 
            HexToColor("cc00ff"), HexToColor("d000ff"), HexToColor("d400ff"), HexToColor("d800ff"), HexToColor("dd00ff"), 
            HexToColor("e100ff"), HexToColor("e500ff"), HexToColor("e900ff"), HexToColor("ee00ff"), HexToColor("f200ff"), 
            HexToColor("f600ff"), HexToColor("fa00ff"), HexToColor("ff00ff"), HexToColor("ff00fa") };
    
    public static Color GenerateRandomHex()
    {
        Random random = new();
        Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        string hex = ColorTranslator.ToHtml(color);
        return HexToColor(HexCheck(hex));
    }

    public static Color HexToColor(string hex)
    {
        return ColorTranslator.FromHtml(HexCheck(hex));
    }

    public static string ColorToHex(Color color)
    {
        return ColorTranslator.ToHtml(color);
    }

    public static Color MixColors(Color color1, Color color2, float factor)
    {
        factor = Math.Max(0f, Math.Min(1f, factor));
    
        int red = (int)(color1.R * (1 - factor) + color2.R * factor);
        int green = (int)(color1.G * (1 - factor) + color2.G * factor);
        int blue = (int)(color1.B * (1 - factor) + color2.B * factor);
    
        red = Math.Max(0, Math.Min(255, red));
        green = Math.Max(0, Math.Min(255, green));
        blue = Math.Max(0, Math.Min(255, blue));

        return Color.FromArgb(red, green, blue);
    }
    
    public static Color DarkenColor(Color color, float factor)
    {
        int red = (int)(color.R * factor);
        int green = (int)(color.G * factor);
        int blue = (int)(color.B * factor);
        
        red = Math.Max(0, Math.Min(255, red));
        green = Math.Max(0, Math.Min(255, green));
        blue = Math.Max(0, Math.Min(255, blue));

        return Color.FromArgb(red, green, blue);
    }

    public static Color LightenColor(Color color, float factor)
    {
        int red = (int)(color.R + (255 - color.R) * factor);
        int green = (int)(color.G + (255 - color.G) * factor);
        int blue = (int)(color.B + (255 - color.B) * factor);
    
        red = Math.Max(0, Math.Min(255, red));
        green = Math.Max(0, Math.Min(255, green));
        blue = Math.Max(0, Math.Min(255, blue));

        return Color.FromArgb(red, green, blue);
    }
    
    public static string HexCheck(string hex)
    {
        return hex.StartsWith("#") ? hex : "#" + hex;
    }
    
}