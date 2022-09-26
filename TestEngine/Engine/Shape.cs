namespace TestEngine;

public class Shape
{
    public Vector Position { get; set; }
    public Vector Size { get; set; }
    public Color color = Color.Aqua;
    public string? Tag;
    public TestEngine.Type type;
    public Image Sprite;
    public Vector CollisionSize { get; set; }

    public Shape(Vector? position, Vector size, Vector collisionSize, Color color, string? tag, TestEngine.Type type, Image sprite)
    {
        Position = position;
        Size = size;
        this.color = color;
        Tag = tag;
        this.type = type;
        CollisionSize = collisionSize;

        Bitmap b = new Bitmap((int)size.X, (int)size.Y);

        using (Graphics g = Graphics.FromImage(b))
        {
            g.DrawImage(sprite, 0, 0, (int)size.X, (int)size.Y);
        }

        Sprite = b;

        TestEngine.RegisterShape(this);
    }
    
    public Shape(Vector? position, Vector size, Vector collisionSize, Color color, string? tag, TestEngine.Type type)
    {
        Position = position;
        Size = size;
        this.color = color;
        Tag = tag;
        this.type = type;
        CollisionSize = collisionSize;

        TestEngine.RegisterShape(this);
    }


    public void Destroy()
    {
        Position = null;
        Size = null;
        Tag = null;

        TestEngine.RemoveShape(this);
    }

    public Shape IsCollided(Shape shape, string tag)
    {
        List<Shape> p = TestEngine.GetShapes(tag);
        foreach (Shape s in p)
        {
            // check if the shapes are colliding
            if (s.Position.X < shape.Position.X + shape.CollisionSize.X &&
                s.Position.X + s.CollisionSize.X > shape.Position.X &&
                s.Position.Y < shape.Position.Y + shape.CollisionSize.Y &&
                s.Position.Y + s.CollisionSize.Y > shape.Position.Y)
            {
                return s;
            }

            /*if (s.Position.Y + s.Size.Y > shape.Position.Y && shape.Position.Y + shape.Size.Y > s.Position.Y &&
                s.Position.X + s.Size.X > shape.Position.X && shape.Position.X + shape.Size.X > s.Position.X)
            {
                return true;
            }*/
        }

        return null;
    }
    
    public Shape IsCollided(Shape shape, List<string> tag)
    {
        foreach (var tags in tag)
        {
            List<Shape> p = TestEngine.GetShapes(tags);
            foreach (Shape s in p)
            {
                // check if the shapes are colliding
                if (s.Position.X < shape.Position.X + shape.CollisionSize.X &&
                    s.Position.X + s.CollisionSize.X > shape.Position.X &&
                    s.Position.Y < shape.Position.Y + shape.CollisionSize.Y &&
                    s.Position.Y + s.CollisionSize.Y > shape.Position.Y)
                {
                    return s;
                }

                /*if (s.Position.Y + s.Size.Y > shape.Position.Y && shape.Position.Y + shape.Size.Y > s.Position.Y &&
                    s.Position.X + s.Size.X > shape.Position.X && shape.Position.X + shape.Size.X > s.Position.X)
                {
                    return true;
                }*/
            }

            return null;   
        }

        return null;
    }

    // Get the center of the shape
    public Vector GetCenter()
    {
        return new Vector(Position.X + Size.X / 2, Position.Y + Size.Y / 2);
    }

    // Get shape's bounding box
    public Rectangle GetBoundingBox()
    {
        return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
    }

    // Get radius of the shape
    public float GetRadius()
    {
        return (float)Size.X / 2;
    }

    // Get Width
    public float GetWidth()
    {
        return (float)Size.X;
    }

    // Get Height
    public float GetHeight()
    {
        return (float)Size.Y;
    }
}