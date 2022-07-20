using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace TestEngine;

public sealed class Canvas : Form
{
    public bool isInFocus = false;
    
    public Canvas()
    {
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
    }
    
    protected override void OnActivated(EventArgs e)
    {
        Debug.Log("Got Focus");
        isInFocus = true;
        base.OnActivated(e);
    }


    protected override void OnDeactivate(EventArgs e)
    {
        Debug.Log("Lost Focus");
        isInFocus = false;
        base.OnDeactivate(e);
    }
}

public abstract class TestEngine
{
    public static Vector ScreenSize = new(1920, 1080);
    private string Title = "TestEngine";

    private Canvas Window = null;
    private Thread gameLoopThread = null;

    protected Debug Debug = new();

    private static List<Shape> ShapeRenderStack = new();
    private static List<Text2D> TextRenderStack = new();
    public static bool W, A, S, D;

    protected bool canUpdate = true;
    
    protected Vector camPos = Vector.Zero();

    private Spritebatch _spritebatch;
    
    public static string assetsFolder = Path.GetFullPath(@"..\..\..\Assets\");
    public static string resourceFolder = Path.Combine(assetsFolder, "Resources");

    public enum Type
    {
        Quad,
        Sprite
    }

    public TestEngine(Vector screenSize, string title)
    {
        ScreenSize = screenSize;
        Title = title;
        
        Window = new Canvas();
        Window.Size = new Size((int)ScreenSize.X, (int)ScreenSize.Y);
        Window.Text = Title;
        Window.Paint += Renderer;
        Window.Click += OnClick;
        
        gameLoopThread = new Thread(GameLoop);
        gameLoopThread.SetApartmentState(ApartmentState.STA);
        gameLoopThread.IsBackground = true;
        gameLoopThread.Start();
        Application.Run(Window);
    }

    public static void RegisterText(Text2D t) => TextRenderStack.Add(t);
    
    public static void RemoveText(Text2D t) => TextRenderStack.Remove(t);
    
    public static void RegisterShape(Shape s)
    {
        ShapeRenderStack.Add(s);
    }

    public static void RemoveShape(Shape s)
    {
        ShapeRenderStack.Remove(s);
    }

    public static List<Shape> GetShapes(string tag)
    {
        List<Shape> found = new List<Shape>();
        foreach (Shape s in ShapeRenderStack)
        {
            if (s.Tag == tag)
            {
                found.Add(s);
            }
        }

        return found;
    }

    private void GameLoop()
    {
        OnLoad();
        while (true)
        {
            try
            {
                if (Window.isInFocus)
                {
                    W = (Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0;
                    A = (Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0;
                    S = (Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0;
                    D = (Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0;
                }
                else
                {
                    W = false;
                    A = false;
                    S = false;
                    D = false;
                }

                if(canUpdate)
                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                OnUpdate();
                Thread.Sleep(1000 / 60);
                
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
            }
        }
    }

    private List<Shape> _shapeRender;
    private List<Text2D> _textRender;

    private void Renderer(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        _spritebatch = new Spritebatch(Window.Size, g);
        _spritebatch.Begin(Color.Green);
        g.TranslateTransform((float)camPos.X,(float)camPos.Y);
        _shapeRender = new List<Shape>(ShapeRenderStack);
        foreach (Shape s in _shapeRender)
        {
            if (s.type == Type.Quad)
            {
                _spritebatch.drawRectangle(s);
                /*g.FillRectangle(new SolidBrush(s.color), (int)s.Position!.X, (int)s.Position.Y, (int)s.Size.X,
                    (int)s.Size.Y);*/
            }

            if (s.type == Type.Sprite)
            {
                _spritebatch.Draw(s);
                /*g.DrawImage(s.image, new Rectangle((int)s.Position!.X, (int)s.Position.Y,
                    (int)s.Size.X, (int)s.Size.Y));*/
            }
        }
        
        _spritebatch.End();
        
        _textRender = new List<Text2D>(TextRenderStack);
        foreach (Text2D t in _textRender)
        {
            Font f = new Font(t.Font, t.FontSize);
            g.DrawString(t.Text, f, new SolidBrush(t.Color), (float)t.Position.X, (float)t.Position.Y, StringFormat.GenericDefault);
        }
        
    }

    public virtual void OnClick(object sender, EventArgs e) {}

    public abstract void OnLoad();
    public abstract void OnUpdate();
}