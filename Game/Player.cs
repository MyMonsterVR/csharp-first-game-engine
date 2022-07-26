namespace TestEngine;

public class Player : Shape
{
    public float Speed = 4f;
    
    public Player(Vector? position, Vector size, Vector collisionSize, Color color, string? tag, TestEngine.Type type, Image image) : base(position, size, collisionSize, color, tag, type, image)
    {
    }
}