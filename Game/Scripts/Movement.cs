using System.Windows.Input;
using TestEngine.Engine;

namespace TestEngine.Scripts;

public class Movement : MonoBehaviour
{
    public static Player player = null;
    public float _oldSpeed = 4;
    public static bool _canMove = true;

    public static Vector _playerPos;

    private Animate _walkanimup, _walkanimdown, _walkanimleft, _walkanimright, _idleanimup, _idleanimdown, _idleanimleft, _idleanimright;

    public void Update()
    {
        Move();
    }

    void MovePosition(int desiredX, int desiredY, float speed)
    {
        _playerPos = player.Position;
        _playerPos.Y += desiredY * speed;
        _playerPos.X += desiredX * speed;
    }

    void Move()
    {
        if (!_canMove) return;

        player.Speed = Keyboard.IsKeyDown(Key.LeftShift) ? _oldSpeed + 2 : _oldSpeed;

        var yAxis = Input.GetInput("Vertical");
        var xAxis = Input.GetInput("Horizontal");

        MovePosition(xAxis, yAxis, player.Speed);
    }
}