using System.Numerics;
using System.Windows;
using System.Windows.Input;

namespace TestEngine;

public class TestGame : TestEngine
{
    private static Player _player = null;
    private Animate _walkanimup, _walkanimdown, _walkanimleft, _walkanimright;

    private float _oldSpeed = 0;

    int desiredX = 0, desiredY = 0;

    private static bool _canMove = true;
    private bool _isWalking = false;
    private bool canMoveRight = true, canMoveLeft = true, canMoveUp = true, canMoveDown = true;
    bool paused = false;

    private Vector _playerPos = Vector.Zero();
    private Vector lastPos = Vector.Zero();

    private static Bitmap treeSprite = new Bitmap(resourceFolder + @"\tree1.png");

    private static Bitmap grassSprite = new Bitmap(resourceFolder + @"\PE_Grass.png");

    private static Bitmap enemySprite = new Bitmap(resourceFolder + @"\player_5.png");

    private static Text2D test;

    public static bool loadingMap;

    public TestGame() : base(new Vector(800, 550), "Test Engine")
    {
    }

    protected override void OnClick(object sender, EventArgs e)
    {
        Debug.Log("Clicked");

    }

    protected override void OnLoad()
    {
        NewMap(0);
        test = new Text2D(new Vector(0, 0), 16, $"Level: {Room.CurrentRoom}", "Arial Black", Color.White);

        LoadMap();

        _oldSpeed = _player.Speed;
        _playerPos = _player.Position;
        _canMove = true;
        canUpdate = true;

        #region DOWN ANIMATION

        _walkanimdown = new Animate();
        _walkanimdown.AddFrame(
            new Bitmap(resourceFolder + @"\player_1.png"), _player);
        _walkanimdown.AddFrame(
            new Bitmap(resourceFolder + @"\player_2.png"), _player);
        _walkanimdown.AddFrame(
            new Bitmap(resourceFolder + @"\player_3.png"), _player);
        _walkanimdown.AddFrame(
            new Bitmap(resourceFolder + @"\player_4.png"), _player);

        #endregion

        #region LEFT ANIMATION

        _walkanimleft = new Animate();
        _walkanimleft.AddFrame(
            new Bitmap(resourceFolder + @"\player_5.png"), _player);
        _walkanimleft.AddFrame(
            new Bitmap(resourceFolder + @"\player_6.png"), _player);
        _walkanimleft.AddFrame(
            new Bitmap(resourceFolder + @"\player_7.png"), _player);
        _walkanimleft.AddFrame(
            new Bitmap(resourceFolder + @"\player_8.png"), _player);

        #endregion

        #region RIGHT ANIMATION

        _walkanimright = new Animate();
        _walkanimright.AddFrame(
            new Bitmap(resourceFolder + @"\player_9.png"), _player);
        _walkanimright.AddFrame(
            new Bitmap(resourceFolder + @"\player_10.png"), _player);
        _walkanimright.AddFrame(
            new Bitmap(resourceFolder + @"\player_11.png"), _player);
        _walkanimright.AddFrame(
            new Bitmap(resourceFolder + @"\player_12.png"), _player);

        #endregion

        #region UP ANIMATION

        _walkanimup = new Animate();
        _walkanimup.AddFrame(
            new Bitmap(resourceFolder + @"\player_13.png"), _player);
        _walkanimup.AddFrame(
            new Bitmap(resourceFolder + @"\player_14.png"), _player);
        _walkanimup.AddFrame(
            new Bitmap(resourceFolder + @"\player_15.png"), _player);
        _walkanimup.AddFrame(
            new Bitmap(resourceFolder + @"\player_16.png"), _player);

        #endregion

        Debug.Log("Finished Loading");
    }

    protected override void LoadMap()
    {
        ShapeRenderStack.Clear();
        NewMap(Room.CurrentRoom);
        Room.NextRoom();
        canUpdate = false;
        _canMove = false;

        PlaceSprites();
        Debug.Log("Loaded map");

        loadingMap = false;
        test.Text = $"Level: {Room.CurrentRoom}";
        _playerPos = _player.Position;
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
            new Shape(i, new Vector(40, 40), new Vector(40,40), Color.LawnGreen, "Floor", Type.Sprite,
                grassSprite);
        }

        // Wall
        foreach (Vector? i in Room.GetTiles("w"))
        {
            // new Shape(i, new Vector(50, 50), Color.DarkGreen, "Wall", Type.Quad);
            new Shape(i, new Vector(50, 50), new Vector(40,40), Color.DarkGreen, "Wall", Type.Sprite,
                treeSprite);
        }
        
        // Player
        foreach (Vector? i in Room.GetTiles("p"))
        {
            // _player = new Player(i, new Vector(40, 40), new Vector(0,0), Color.Gold, "Player", Type.Quad, new Bitmap(resourceFolder + @"\player_1.png"));
            _player = new Player(i, new Vector(40, 40), new Vector(20,20), Color.Blue, "Player", Type.Sprite,
                new Bitmap(resourceFolder + @"\player_1.png"));
        }

        // Enemy
        foreach (Vector? i in Room.GetTiles("e"))
        {
            new Shape(i, new Vector(40, 40), new Vector(40,40), Color.Red, "Enemy", Type.Sprite,
                enemySprite);
        }

        // Door
        foreach (Vector? i in Room.GetTiles("d"))
        {
            //new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Quad);
            new Shape(i, new Vector(50, 50),  new Vector(40,40), Color.Black, "Door", Type.Quad);
        }
        
    }
    protected override void OnUpdate()
    {
        if (paused) return;
        Movement();
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
                    { "w", "w", "w", "w", "w", ".", ".", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", ".", "p", ".", ".", ".", "e", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", ".", "w", ".", ".", ".", "w", "w", "w", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w" },
                    { "w", ".", ".", "w", "w", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", ".", "e", "w", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", ".", "w", "w", "w", "p", ".", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
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
                    { "w", ".", ".", "e", "w", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
                    { "w", "p", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
                };

                Room.AddRoom(map);
                break;
            }
        }

        Debug.Log("Added new map");
    }

    void Movement()
    {
        if (!_canMove) return;
        if (Keyboard.IsKeyDown(Key.LeftShift))
        {
            _player.Speed = _oldSpeed + 2;
        }
        else
        {
            _player.Speed = _oldSpeed;
        }

        if (A & !W & !D & !S)
        {
            _isWalking = true;
            _player.Sprite = _walkanimleft.PlayOneFrame();
            desiredX = -1;
        }

        if (D)
        {
            _isWalking = true;
            _player.Sprite = _walkanimright.PlayOneFrame();
            desiredX = 1;
        }

        if (W)
        {
            _isWalking = true;
            _player.Sprite = _walkanimup.PlayOneFrame();
            desiredY = -1;
        }

        if (S)
        {
            _isWalking = true;
            _player.Sprite = _walkanimdown.PlayOneFrame();
            desiredY = 1;
        }

        // Is player Idle
        if (!W && !S && !A && !D)
        {
            _isWalking = false;
        }

        if (_canMove && _isWalking)
        {
            _playerPos.Y += desiredY * _player.Speed;
            _playerPos.X += desiredX * _player.Speed;
            desiredX = 0;
            desiredY = 0;
        }
    }

    void OnCollision()
    {
        if (_player.IsCollided(_player, "Wall") != null)
        {
            _playerPos.X = lastPos.X;
            _playerPos.Y = lastPos.Y;
        }
        else
        {
            lastPos.X = _playerPos.X;
            lastPos.Y = _playerPos.Y;
        }

        if (_player.IsCollided(_player, "Door") != null)
        {
            LoadMap();
        }
    }
}