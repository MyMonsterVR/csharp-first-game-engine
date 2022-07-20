namespace TestEngine;

public class Spritebatch
{
    private Graphics Gfx;
    private BufferedGraphics bfgfx;
    private BufferedGraphicsContext cntxt = BufferedGraphicsManager.Current;

    public Spritebatch(Size clientsize, Graphics gfx)
    {
        cntxt.MaximumBuffer = new Size(clientsize.Width + 1, clientsize.Height + 1);
        bfgfx = cntxt.Allocate(gfx, new Rectangle(Point.Empty, clientsize));
        Gfx = gfx;
    }

    public void Begin()
    {
        bfgfx.Graphics.Clear(Color.Black);
    }
    
    public void Begin(Color c)
    {
        bfgfx.Graphics.Clear(c);
    }

    public void Draw(Shape s)
    {
        bfgfx.Graphics.DrawImageUnscaled(s.image, new Rectangle((int)s.Position!.X, (int)s.Position.Y,
            (int)s.Size.X, (int)s.Size.Y));
    }

    public void drawImage(Bitmap b, Rectangle rec)
    {
        bfgfx.Graphics.DrawImageUnscaled(b, rec);
    }

    public void drawImageClipped(Bitmap b, Rectangle rec)
    {
        bfgfx.Graphics.DrawImageUnscaledAndClipped(b, rec);
    }

    public void drawRectangle(Shape s)
    {
        bfgfx.Graphics.FillRectangle(new SolidBrush(s.color), (int)s.Position!.X, (int)s.Position.Y, (int)s.Size.X,
            (int)s.Size.Y);
    }

    public void End()
    {
        bfgfx.Render(Gfx);
    }
}