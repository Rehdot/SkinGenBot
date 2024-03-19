using System.Runtime.InteropServices;
using System.Drawing;

namespace SkinGenBot;

public class Image
{
    private static readonly Bitmap
        NewSkin = new(64, 64),
        SolidImage = new(GetImageReference("SkinGenBot.Graphics.BaseSkin.png")),
        SplitImage = new(GetImageReference("SkinGenBot.Graphics.SplitGen.png"));
    private static readonly List<int> 
        HatCoords1 = new() { 0, 7, 8, 31, 1, 6, 9, 30, 2, 5, 10, 29, 3, 4, 11, 28 }, 
        HatCoords2 = new() { 15, 16, 23, 24, 14, 17, 22, 25, 13, 18, 21, 26, 12, 19, 20, 27 };
    
    
    private static Stream GetImageReference(string resourcePath)
    {
        Stream stream = typeof(Program).Assembly.GetManifestResourceStream(resourcePath);
        if (stream == null) throw new Exception($"Resource '{resourcePath}' not found.");
        return stream;
    }
    
    public static void SaveImage(bool splitGen, string input, [Optional] string input2)
    {
        if (splitGen)
        {
            Console.WriteLine($"SplitGen of {Colors.HexCheck(input)} & {Colors.HexCheck(input2)} generated. (Runtime generation #{Program.Counter})");
            NewSkin.Save(Program.GetPathSplitGen(input, input2), System.Drawing.Imaging.ImageFormat.Png);
            if (Program.ConsoleMode) Console.WriteLine($"Skin saved to {Program.GetPathSplitGen(input, input2)}");
        }
        else
        {
            Console.WriteLine($"Hex code {input} generated. (Runtime generation #{Program.Counter})");
            NewSkin.Save(Program.GetPath(input), System.Drawing.Imaging.ImageFormat.Png);
            if (Program.ConsoleMode) Console.WriteLine($"Skin saved to {Program.GetPath(input)}");
        }
        
        Program.Counter++;
    }

    public static void ReplaceColorValue(Color oldColor, Color newColor)
    {
        for (int y = 0; y < NewSkin.Height; y++)
        {
            for (int x = 0; x < NewSkin.Width; x++)
            {
                if (NewSkin.GetPixel(x, y) == oldColor)
                    NewSkin.SetPixel(x, y, newColor);
            }
                
        }
        
    }

    public static void ReplaceCoord(int x, int y, Color newColor)
    {
        NewSkin.SetPixel(x, y, newColor);
    }

    public static void SplitGen(string hex1, string hex2)
    {
        DrawNewImage(SplitImage);
        
        Color leftColor = Colors.HexToColor(Colors.HexCheck(hex1));
        Color rightColor = Colors.HexToColor(Colors.HexCheck(hex2));

        if (!IsTooDark(leftColor))
            ReplaceAllColors(Colors.LeftColorList, leftColor, 0.85f);
        else
        {
            ReplaceColorValue(Colors.LeftColorList[0], leftColor);
            ReplaceHatColors(true, false, leftColor, rightColor);
        }
        
        if (!IsTooDark(rightColor))
            ReplaceAllColors(Colors.RightColorList, rightColor, 0.85f);
        else
        {
            ReplaceColorValue(Colors.RightColorList[0], rightColor);
            ReplaceHatColors(false, true, leftColor, rightColor);
        }
    }

    public static void SolidGen(string hex)
    {
        Color newColor = Colors.HexToColor(Colors.HexCheck(hex));
        DrawNewImage(SolidImage);
        
        if (!IsTooDark(newColor))
        {
            ReplaceAllColors(Colors.SolidColorList, newColor, 0.85f);
        }
        else
        {
            ReplaceColorValue(Colors.SolidColorList[0], newColor);
            ReplaceHatColors(true, true, newColor, newColor);
        }
        
    }

    private static void ReplaceHatColors(bool leftSide, bool rightSide, Color leftColor, Color rightColor)
    {
        if (leftSide)
        {
            ReplaceColorValue(Colors.LeftColorList[2], Colors.LightenColor(leftColor, 0.15f));
            ReplaceHatCoords(HatCoords1, leftColor, 0.05f);
        }
        if (rightSide)
        {
            ReplaceColorValue(Colors.RightColorList[2], Colors.LightenColor(rightColor, 0.15f));
            ReplaceHatCoords(HatCoords2, rightColor, 0.05f);
        }
    }

    private static void ReplaceAllColors(List<Color> list, Color newColor, float factor)
    {
        for (int i = 0; i < 4; i++)
        {

            if (i < 1)
            {
                ReplaceColorValue(list[i], newColor);
                continue;
            }
            
            ReplaceColorValue(list[i], Colors.DarkenColor(newColor, factor));
            factor -= 0.10f;
        }
    }

    private static void ReplaceHatCoords(List<int> list, Color color, float factor)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if ((i + 1) % 4 == 0 && i < 16)
                factor += 0.10f;
            
            if (i < 4)
            {
                ReplaceCoord(list[i], 10, color);
                continue;
            }
            
            ReplaceCoord(list[i], 10, Colors.LightenColor(color, factor));
        }
    }
    
    private static bool IsTooDark(Color color)
    {
        return Equals(Colors.DarkenColor(color, 0.65f), Colors.DarkenColor(color, 0.75f));
    }
    
    public static void DrawNewImage(Bitmap baseImage)
    {
        using (Graphics graphics = Graphics.FromImage(NewSkin))
        {
            try
            {
                graphics.DrawImage(baseImage, new Rectangle(0, 0, 64, 64));
                if (Program.DebugMode) Console.WriteLine("DEBUG: Internal graphic found.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Internal graphic not found.\n{e}");
            }
        }
    }
}