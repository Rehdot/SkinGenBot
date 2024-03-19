using System.Drawing;

namespace SkinGenBot;

public class Colors
{

    public static readonly List<Color>
        SolidColorList = new() { HexToColor("#1520a7"), HexToColor("#08149a"), HexToColor("#000a8d"), HexToColor("#000081") },
        LeftColorList = new() { HexToColor("#2a2a2a"), HexToColor("#171717"), HexToColor("#0d0d0d"), HexToColor("#000000") },
        RightColorList = new() { HexToColor("#fdbfe7"), HexToColor("#d7a2c4"), HexToColor("#bd8fad"), HexToColor("#a47c96") };
    
    public static Color GenerateRandomHex()
    {
        Random random = new();
        Color color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        string hex = ColorTranslator.ToHtml(color);
        return HexToColor(HexCheck(hex));
    }

    public static Color HexToColor(string hex)
    {
        return ColorTranslator.FromHtml(hex);
    }

    public static string ColorToHex(Color color)
    {
        return ColorTranslator.ToHtml(color);
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