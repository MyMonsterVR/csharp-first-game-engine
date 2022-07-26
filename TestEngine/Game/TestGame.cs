﻿using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TestEngine.Game.Scripts;
using TestEngine.Scripts;

namespace TestEngine;

public class TestGame : TestEngine
{

    private Animate _walkanimup, _walkanimdown, _walkanimleft, _walkanimright;

    private float _oldSpeed = 0;

    int desiredX = 0, desiredY = 0;

    private static bool _canMove = true;
    private bool canMoveRight = true, canMoveLeft = true, canMoveUp = true, canMoveDown = true;
    bool paused = false;
    
    private Vector lastPos = Vector.Zero();

    public static Bitmap treeSprite = new Bitmap(ResourceFolder + @"\tree1.png");

    public static Bitmap grassSprite = new Bitmap(ResourceFolder + @"\PE_Grass.png");

    public static Bitmap enemySprite = new Bitmap(ResourceFolder + @"\player_5.png");

    public static Bitmap playerSprite = new Bitmap(ResourceFolder + @"\player_1.png");

    private static Text2D test;

    private Shape[] enemies = new Shape[10];

    private int enemyCount = 0;

    public static bool loadingMap;

    private static readonly string mapPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\map.json";

    public TestGame() : base(new Vector(815, 555), "Test Engine")
    {
    }

    protected override void OnClick(object sender, EventArgs e)
    {
        Debug.Log("Clicked");
    }

    private Map mapData;
    
    protected override void OnLoad()
    {
        Console.WriteLine("Loading map...");
        var map = new MapLoader(mapPath);
        map.DrawMap();
        //t.PrintMapData();
        //mapData.LoadJson(mapPath);

        /*NewMap(0);
        test = new Text2D(new Vector(0, 0), 16, $"Level: {Room.CurrentRoom}", "Arial Black", Color.White);

        LoadMap();

        _oldSpeed = Movement._player.Speed;
        Movement._playerPos = Movement._player.Position;
        _canMove = true;
        canUpdate = true;

        #region DOWN ANIMATION

        _walkanimdown = new Animate();
        _walkanimdown.AddFrame(
            new Bitmap(ResourceFolder + @"\player_1.png"), Movement._player);
        _walkanimdown.AddFrame(
            new Bitmap(ResourceFolder + @"\player_2.png"), Movement._player);
        _walkanimdown.AddFrame(
            new Bitmap(ResourceFolder + @"\player_3.png"), Movement._player);
        _walkanimdown.AddFrame(
            new Bitmap(ResourceFolder + @"\player_4.png"), Movement._player);

        #endregion

        #region LEFT ANIMATION

        _walkanimleft = new Animate();
        _walkanimleft.AddFrame(
            new Bitmap(ResourceFolder + @"\player_5.png"), Movement._player);
        _walkanimleft.AddFrame(
            new Bitmap(ResourceFolder + @"\player_6.png"), Movement._player);
        _walkanimleft.AddFrame(
            new Bitmap(ResourceFolder + @"\player_7.png"), Movement._player);
        _walkanimleft.AddFrame(
            new Bitmap(ResourceFolder + @"\player_8.png"), Movement._player);

        #endregion

        #region RIGHT ANIMATION

        _walkanimright = new Animate();
        _walkanimright.AddFrame(
            new Bitmap(ResourceFolder + @"\player_9.png"), Movement._player);
        _walkanimright.AddFrame(
            new Bitmap(ResourceFolder + @"\player_10.png"), Movement._player);
        _walkanimright.AddFrame(
            new Bitmap(ResourceFolder + @"\player_11.png"), Movement._player);
        _walkanimright.AddFrame(
            new Bitmap(ResourceFolder + @"\player_12.png"), Movement._player);

        #endregion

        #region UP ANIMATION

        _walkanimup = new Animate();
        _walkanimup.AddFrame(
            new Bitmap(ResourceFolder + @"\player_13.png"), Movement._player);
        _walkanimup.AddFrame(
            new Bitmap(ResourceFolder + @"\player_14.png"), Movement._player);
        _walkanimup.AddFrame(
            new Bitmap(ResourceFolder + @"\player_15.png"), Movement._player);
        _walkanimup.AddFrame(
            new Bitmap(ResourceFolder + @"\player_16.png"), Movement._player);

        #endregion

        Debug.Log("Finished Loading");*/
    }

    protected override void LoadMap(int nextRoom = -1)
    {
        ShapeRenderStack.Clear();
        if (nextRoom == -1)
        {
            NewMap(Room.CurrentRoom);
        }
        else
        {
            NewMap(nextRoom);
        }

        Room.NextRoom();
        canUpdate = false;
        _canMove = false;

        //PlaceSprites();
        Debug.Log("Loaded map");

        loadingMap = false;
        test.Text = $"Level: {Room.CurrentRoom}";
        Movement._playerPos = Movement._player.Position;
        _canMove = true;
        canUpdate = true;

        UpdateRender();
    }

    protected override void PlaceSprites()
    {
        // Grass
        foreach (Vector? i in Room.GetTiles("g"))
        {
            //new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Quad);
            new Shape(i, new Vector(40, 40), new Vector(40, 40), Color.LawnGreen, "Floor", Type.Sprite,
                grassSprite);
        }

        // Wall
        foreach (Vector? i in Room.GetTiles("w"))
        {
            // new Shape(i, new Vector(50, 50), Color.DarkGreen, "Wall", Type.Quad);
            new Shape(i, new Vector(50, 50), new Vector(40, 40), Color.DarkGreen, "Wall", Type.Sprite,
                treeSprite);
        }

        // Enemy
        foreach (Vector? i in Room.GetTiles("e"))
        {
            enemies[enemyCount] = new Shape(i, new Vector(40, 40), new Vector(40, 40), Color.Red, "Enemy", Type.Sprite,
                enemySprite);

            Debug.Log(enemies[enemyCount].Position.X + " " + enemies[enemyCount].Position.Y);
            enemyCount++;
        }

        // Player
        foreach (Vector? i in Room.GetTiles("p"))
        {
            // Movement._player = new Player(i, new Vector(40, 40), new Vector(0,0), Color.Gold, "Player", Type.Quad, new Bitmap(resourceFolder + @"\player_1.png"));
            Movement._player = new Player(i, new Vector(40, 40), new Vector(20, 20), Color.Blue, "Player", Type.Sprite,
                new Bitmap(ResourceFolder + @"\player_1.png"));
        }

        // Door
        foreach (Vector? i in Room.GetTiles("d"))
        {
            //new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Quad);
            new Shape(i, new Vector(50, 50), new Vector(40, 40), Color.Black, "Door", Type.Quad);
        }

        // Previous Door
        foreach (Vector? i in Room.GetTiles("pd"))
        {
            //new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Quad);
            new Shape(i, new Vector(50, 50), new Vector(40, 40), Color.Black, "PrevDoor", Type.Quad);
        }

        // Second Door
        foreach (Vector? i in Room.GetTiles("sd"))
        {
            //new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Quad);
            new Shape(i, new Vector(50, 50), new Vector(40, 40), Color.Black, "Door2", Type.Quad);
        }
    }

    protected override void OnUpdate()
    {
        if (paused) return;
        OnCollision();
    }

    static void NewMap(int level)
    {
        switch (level)
        {
            case 0:
            {
                string[,] map =
                {
                    { "w", "w", "w", "w", "w", "d", "d", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", ".", ".", ".", ".", ".", "e", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", ".", ".", ".", "p", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", ".", ".", ".", ".", ".", "w", "w", "w", ".", ".", "w", "w", "w" },
                    { "w", ".", "w", "w", "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", "w", "w", "w", "g", "g", "g", "g", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", "w", "w", "w", "g", "g", "g", "g", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", "e", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                };

                Room.AddRoom(map);
                break;
            }

            case 1:
            {
                string[,] map =
                {
                    { "w", "w", "w", "w", "w", "d", "d", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", ".", "p", ".", ".", ".", "e", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", "w", ".", ".", ".", "w", "w", "w", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", "e", "w", "p", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", "w", "w", "pd", "pd", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                };

                Room.AddRoom(map);
                break;
            }

            case 2:
            {
                string[,] map =
                {
                    { "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", ".", ".", ".", ".", ".", "e", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", "w", ".", ".", ".", "w", "w", "w", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", "g", "g", "g", "g", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", "g", "g", "g", "g", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", "p", ".", "e", "w", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", "pd", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                };

                Room.AddRoom(map);
                break;
            }
            default:
                Debug.Log($"No map found at level: {level}");
                break;
        }

        Debug.Log("Added new map");
    }

    void OnCollision()
    {
        if (Movement._player.IsCollided(Movement._player, "Wall") != null || Movement._player.IsCollided(Movement._player, "Enemy") != null)
        {
            Movement._playerPos.X = lastPos.X;
            Movement._playerPos.Y = lastPos.Y;
        }
        else
        {
            lastPos.X = Movement._playerPos.X;
            lastPos.Y = Movement._playerPos.Y;
        }

        List<string> tagList = new List<string>() { "Door", "PrevDoor", "SecondDoor", "ThirdDoor" };

        foreach (var tagsToCheck in tagList)
        {
            if (Movement._player.IsCollided(Movement._player, tagsToCheck) != null)
            {
                Debug.Log("Collided with " + tagsToCheck);
                var tag = tagsToCheck;

                // get player collision tag
                var roomToGo = 0;

                if (tag == "PrevDoor")
                {
                    Room.CurrentRoom -= 2;
                    Debug.Log("Room to go back to: " + roomToGo);
                }
                else if (tag == "SecondDoor")
                {
                    Room.CurrentRoom += 2;
                    Console.WriteLine("Room to go: " + roomToGo);
                }

                LoadMap(Room.CurrentRoom);
            }
        }
    }
}