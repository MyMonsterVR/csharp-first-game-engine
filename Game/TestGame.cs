using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;
using Point = System.Drawing.Point;
using SystemFonts = System.Drawing.SystemFonts;

namespace TestEngine;

public class TestGame : TestEngine
{
    private Player _player;
    private Animate _walkanimup, _walkanimdown, _walkanimleft, _walkanimright;

    private float _oldSpeed = 0;

    private bool _canMove = true;
    private bool isWalking;
    int desiredX = 0, desiredY = 0;

    private bool canMoveRight = true, canMoveLeft = true, canMoveUp = true, canMoveDown = true;

    bool paused = false;

    public TestGame() : base(new Vector(800, 500), "Test Engine")
    {
    }
    
    Bitmap tree = new Bitmap(
        resourceFolder + @"\tree1.png");

    public override void OnClick(object sender, EventArgs e)
    {
    }

    public override void OnLoad()
    {
        string[,] map = new string[10, 16]
        {
            { "w", "w", "w", "w", "w", ".", ".", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", ".", ".", ".", ".", ".", "e", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", ".", ".", ".", "p", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", "w", "w", ".", ".", ".", ".", ".", ".", ".", "w", "w", "w", "w" },
            { "w", ".", "w", "w", "w", "g", "g", "g", "g", ".", ".", ".", "w", "w", "w", "w" },
            { "w", ".", "w", "w", "w", "g", "g", "g", "g", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", "e", ".", ".", ".", ".", ".", "w", "w", "w", "w", "w", "w", "w" },
            { "w", ".", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
        };
        
        Room.AddRoom(map);
        
        // Add the player to the room
        
        foreach (Vector? i in Room.GetTiles("g"))
            //new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Quad);
            new Shape(i, new Vector(40, 40), Color.LawnGreen, "Floor", Type.Sprite,
                new Bitmap(
                    resourceFolder + @"\PE_Grass.png"));
        
        foreach (Vector? i in Room.GetTiles("w"))
        {
            Thread.Sleep(10);
            // new Shape(i, new Vector(50, 50), Color.DarkGreen, "Wall", Type.Quad);
            new Shape(i, new Vector(50, 50), Color.DarkGreen, "Wall", Type.Sprite,
                tree);
        }
        
        foreach (Vector? i in Room.GetTiles("e"))
        {
            new Shape(i, new Vector(40, 40), Color.Red, "Enemy", Type.Sprite,
                new Bitmap(
                    resourceFolder + @"\player_5.png"));
        }
        
        foreach (Vector? i in Room.GetTiles("p"))
        {
            // _player = new Player(i, new Vector(40, 40), Color.Gold, "Player", Type.Quad, null);
            _player = new Player(i, new Vector(40, 40), Color.Blue, "Player", Type.Sprite,
                new Bitmap(
                    resourceFolder + @"\player_1.png"));
        }
        
        _oldSpeed = _player.Speed;
        
        _canMove = true;
        canUpdate = true;
        
        Debug.Log("Just Loaded");
        
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
    }

    public override void OnUpdate()
    {
        
        Movement();
        OnCollision();
    }
    
    
    void Movement()
    {
        if (Keyboard.IsKeyDown(Key.LeftShift))
        {
            _player.Speed = _oldSpeed + 2;
        }
        else
        {
            _player.Speed = _oldSpeed;
        }

        if (A && canMoveLeft)
        {
            _player.image = _walkanimleft.PlayOneFrame();
            desiredX = -1;
        }

        if (D && canMoveRight)
        {
            _player.image = _walkanimright.PlayOneFrame();
            desiredX = 1;
        }

        if (W && canMoveUp)
        {
            _player.image = _walkanimup.PlayOneFrame();
            desiredY = -1;
        }

        if (S && canMoveDown)
        {
            _player.image = _walkanimdown.PlayOneFrame();
            desiredY = 1;
        }

        // Is player Idle
        if (!W && !S && !A && !D)
        {
            isWalking = false;
        }

        if (_canMove)
        {
            _player.Position.Y += desiredY * _player.Speed;
            _player.Position.X += desiredX * _player.Speed;
            desiredX = 0;
            desiredY = 0;
        }
    }

    void OnCollision()
    {
        if (!_player.IsCollided(_player, "Wall") && !paused)
        {
            _canMove = true;
            canUpdate = true;
            canMoveRight = true;
            canMoveLeft = true;
            canMoveUp = true;
            canMoveDown = true;
        }

        if (_player.IsCollided(_player, "Wall"))
        {
            //canUpdate = false;
            // Get colliding wall
            Shape wall = Vector.GetClosestShape(_player.Position, "Wall", _player);
            //Debug.Log(wall.GetCenter().X + " " + wall.GetCenter().Y);

            // Find out which side of the wall the player is facing
            if (_player.Position.X > wall.GetCenter().X)
            {
                // Player is facing left of wall
                canMoveLeft = false;
                canMoveRight = true;
            }
            else
            {
                // Player is facing right of wall
                canMoveRight = false;
                canMoveLeft = true;
            }

            if (_player.Position.Y > wall.GetCenter().Y)
            {
                // Player is facing up of wall
                canMoveUp = false;
                canMoveDown = true;
            }
            else
            {
                // Player is facing down of wall
                canMoveDown = false;
                canMoveUp = true;
            }
        }
    }
}


/*
if (_player.IsCollided(_player, "Wall"))
        {
            // Get colliding wall
            Shape wall = Vector.GetClosestShape(_player.Position, "Wall", _player);
            //Debug.Log(wall.GetCenter().X + " " + wall.GetCenter().Y);

            // Find out which side of the wall the player is facing
            if (_player.Position.X > wall.GetCenter().X)
            {
                // Player is facing left of wall
                canMoveLeft = false;
                canMoveRight = true;
            }
            else
            {
                // Player is facing right of wall
                canMoveRight = false;
                canMoveLeft = true;
            }
            
            if (_player.Position.Y > wall.GetCenter().Y)
            {
                // Player is facing up of wall
                canMoveUp = false;
                canMoveDown = true;
            }
            else
            {
                // Player is facing down of wall
                canMoveDown = false;
                canMoveUp = true;
            }

        }*/