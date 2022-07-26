﻿using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using Application = System.Windows.Forms.Application;
using Size = System.Drawing.Size;

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

    public static Canvas Window = null;
    private Thread gameLoopThread = null;

    protected static Debug Debug = new();

    protected static List<Shape> ShapeRenderStack = new();
    private static List<Text2D> TextRenderStack = new();
    public static bool W, A, S, D;

    protected static bool canUpdate = true;

    protected static Vector camPos = Vector.Zero();
    protected static Vector camZoom = new Vector(1, 1);
    protected static float camRot = 0;

    private static Spritebatch _spritebatch;

    private static readonly string AssetsFolder = Path.GetFullPath(@"..\..\..\Assets\");
    public static readonly string ResourceFolder = Path.Combine(AssetsFolder, "Resources");

    protected Input Input = new();

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
        Window.FormClosed += OnClose;
        gameLoopThread = new Thread(GameLoop);
        gameLoopThread.SetApartmentState(ApartmentState.STA);
        gameLoopThread.IsBackground = true;
        gameLoopThread.Start();
        Application.Run(Window);
    }

    public static void UpdateRender()
    {
        Window.Refresh();
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
        global::TestEngine.Debug.Log("Removed Shape");
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

    private static IEnumerable<System.Type> GetAllTypesThatImplementInterface<T>()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface);
    }

    private void GameLoop()
    {
        foreach (var type in GetAllTypesThatImplementInterface<MonoBehaviour>())
        {
            var instance = (MonoBehaviour)Activator.CreateInstance(type);
            instance.Start();
        }
        OnLoad();
        while (true)
        {
            try
            {
                if (Window.isInFocus)
                {
                    foreach (var type in GetAllTypesThatImplementInterface<MonoBehaviour>())
                    {
                        var instance = (MonoBehaviour)Activator.CreateInstance(type);
                        instance.Update();
                    }

                    if (canUpdate)
                        Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                    OnUpdate();
                }

                Thread.Sleep(1000 / 60);
            }
            catch (Exception e)
            {
                Debug.Error(e.StackTrace?.ToString());
                return;
            }
        }
        
    }
    
    public static void SetCamera(Vector pos, Vector zoom, float rot)
    {
        camPos = pos;
        camZoom = zoom;
        camRot = rot;
    }

    private static List<Shape> _shapeRender;
    private static List<Text2D> _textRender;

    private static void Renderer(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        _spritebatch = new Spritebatch(Window.Size, g);
        _spritebatch.Begin(Color.Green);
        g.TranslateTransform((float)camPos.X, (float)camPos.Y);
        g.ScaleTransform((float)camZoom.X, (float)camZoom.Y);
        g.RotateTransform(camRot);

        _shapeRender = new List<Shape>(ShapeRenderStack);
        foreach (Shape s in _shapeRender)
        {
            if (s.type == Type.Quad)
            {
                _spritebatch.drawRectangle(s);
            }

            if (s.type == Type.Sprite)
            {
                _spritebatch.Draw(s);
            }
        }

        _spritebatch.End();

        _textRender = new List<Text2D>(TextRenderStack);
        foreach (Text2D t in _textRender)
        {
            Font f = new Font(t.Font, t.FontSize);
            g.DrawString(t.Text, f, new SolidBrush(t.Color), (float)t.Position.X, (float)t.Position.Y,
                StringFormat.GenericDefault);
        }
    }

    protected virtual void OnClose(object? sender, FormClosedEventArgs e)
    {
        Environment.Exit(0);
    }

    protected virtual void OnClick(object sender, EventArgs e)
    {
    }

    protected abstract void OnLoad();
    protected abstract void OnUpdate();
    protected abstract void LoadMap(int nextRoom = -1);
    protected abstract void PlaceSprites();
}