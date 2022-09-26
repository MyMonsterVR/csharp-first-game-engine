using System.Windows.Input;

namespace TestEngine.Scripts;

public class Movement : MonoBehaviour
{
    public static Player _player = null;
    public float _oldSpeed = 0;
    public static bool _canMove = true;

    public static Vector _playerPos;

    public void Update()
    {
        Move();
    }
    
    void MovePosition(int desiredX, int desiredY, float speed)
    {
        _playerPos = _player.Position;
        _playerPos.Y += desiredY * speed;
        _playerPos.X += desiredX * speed;
    }
    
    void Move()
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
        

        var yAxis = Input.GetInput("Vertical");
        var xAxis = Input.GetInput("Horizontal");

        MovePosition(xAxis, yAxis, 4);


        /*if (A & !W & !D & !S)
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
        }*/
    }
}