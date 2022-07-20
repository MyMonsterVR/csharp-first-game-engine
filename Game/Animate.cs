namespace TestEngine;

public class Animate
{
    List<Image> frames = new List<Image>();
    private int increment = 0;
    private int DelayCount = 0;
    private int delay = 10;

    private Vector size;
    
    public void AddFrame(Bitmap frame, Shape s)
    {
        frames.Add(frame);
        size = s.Size;
    }
    
    public void ClearFrames()
    {
        frames.Clear();
    }

    public Image PlayOneFrame()
    {
        DelayCount++;
        if (DelayCount % delay == 0)
        {
            increment = (increment == frames.Count) ? 0 : increment + 1;
        }

        return new Bitmap(frames[increment - 1], (int)size.X, (int)size.Y);
    }
}