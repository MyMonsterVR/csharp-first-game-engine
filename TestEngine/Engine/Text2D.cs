namespace TestEngine;

public class Text2D
{
    public Vector Position;
    public int FontSize;
    public string Text;
    public string Font;
    public Color Color;
    
    public Text2D(Vector position, int fontSize, string text, string font, Color color)
    {
        Position = position;
        FontSize = fontSize;
        Text = text;
        Font = font;
        Color = color;
        
        TestEngine.RegisterText(this);
    }
}