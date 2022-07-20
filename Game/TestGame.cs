using System.Numerics;
using System.Windows;
using System.Windows.Input;

namespace TestEngine;

public class TestGame : TestEngine
{
    private Player _player;
    private Animate _walkanimup, _walkanimdown, _walkanimleft, _walkanimright;

    private float _oldSpeed = 0;

    int desiredX = 0, desiredY = 0;

    private bool _canMove = true;
    private bool isWalking;
    private bool canMoveRight = true, canMoveLeft = true, canMoveUp = true, canMoveDown = true;
    bool paused = false;

    private Vector playerPos = Vector.Zero();
    private Vector lastPos = Vector.Zero();

    private Bitmap treeSprite = new Bitmap(resourceFolder + @"\tree1.png");

    private Bitmap grassSprite = new Bitmap(resourceFolder +@"\PE_Grass.png");
    
    private Bitmap enemySprite = new Bitmap(resourceFolder +@"\player_5.png");

    public TestGame() : base(new Vector(800, 550), "Test Engine")
    {
    }

    protected override void OnClick(object sender, EventArgs e)
    {
        Console.WriteLine("You clicked");
    }

    protected override void OnLoad()
    {
        string[,] map =
        {
            { "w", "w", "w", "w", "w", ".", ".", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
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
        
        string[,] map2 =
        {
            { "w", "w", "w", "w", "w", ".", "p", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", ".", ".", ".", ".", ".", "e", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", ".", ".", "w", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", ".", ".", "w", ".", ".", ".", "w", "w", "w", ".", ".", "w", "w", "w" },
            { "w", ".", ".", "w", "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w" },
            { "w", ".", ".", "w", "w", "g", "g", "g", "g", ".", ".", ".", ".", "w", "w", "w" },
            { "w", ".", ".", "w", "w", "g", "g", "g", "g", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", ".", "e", "w", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
        };
        
        Room.AddRoom(map2);
        
        // Add the player to the room
        
        // Grass
        foreach (Vector? i in Room.GetTiles("g"))
            //new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Quad);
            new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Sprite,
                grassSprite);
        
        // Wall
        foreach (Vector? i in Room.GetTiles("w"))
        {
            // new Shape(i, new Vector(50, 50), Color.DarkGreen, "Wall", Type.Quad);
            new Shape(i, new Vector(50, 50), Color.DarkGreen, "Wall", Type.Sprite,
                treeSprite);
        }
        
        // Enemy
        foreach (Vector? i in Room.GetTiles("e"))
        {
            new Shape(i, new Vector(40, 40), Color.Red, "Enemy", Type.Sprite,
                enemySprite);
        }
        
        // Player
        foreach (Vector? i in Room.GetTiles("p"))
        {
            // _player = new Player(i, new Vector(40, 40), Color.Gold, "Player", Type.Quad, null);
            _player = new Player(i, new Vector(40, 40), Color.Blue, "Player", Type.Sprite,
                new Bitmap(resourceFolder + @"\player_1.png"));
        }
        
        _oldSpeed = _player.Speed;
        
        _canMove = true;
        canUpdate = true;
        
        playerPos = _player.Position;

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

        new Text2D(new Vector(0, 0), 16, "Hello World", "Arial Black", Color.White);
        
        Debug.Log("Finished Loading");
    }

    protected override void OnUpdate()
    {
        Movement();
        OnCollision();
        if (Keyboard.IsKeyDown(Key.R))
        {
            Room.NextRoom();
            UpdateRender();
        }
    }


    void Movement()
    {
        if (_player == null) return;
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
            _player.Sprite = _walkanimleft.PlayOneFrame();
            desiredX = -1;
        }

        if (D)
        {
            _player.Sprite = _walkanimright.PlayOneFrame();
            desiredX = 1;
        }

        if (W)
        {
            _player.Sprite = _walkanimup.PlayOneFrame();
            desiredY = -1;
        }

        if (S)
        {
            _player.Sprite = _walkanimdown.PlayOneFrame();
            desiredY = 1;
        }

        // Is player Idle
        if (!W && !S && !A && !D)
        {
            isWalking = false;
        }

        if (_canMove)
        {
            playerPos.Y += desiredY * _player.Speed;
            playerPos.X += desiredX * _player.Speed;
            desiredX = 0;
            desiredY = 0;
        }
    }

    void OnCollision()
    {
        if (_player.IsCollided(_player, "Wall") != null)
        {
            playerPos.X = lastPos.X;
            playerPos.Y = lastPos.Y;
        }
        else
        {
            lastPos.X = playerPos.X;
            lastPos.Y = playerPos.Y;
        }
    }
}