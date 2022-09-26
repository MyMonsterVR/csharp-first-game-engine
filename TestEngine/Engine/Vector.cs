namespace TestEngine;

public class Vector
{
    public double X { get; set; }
    public double Y { get; set; }

    public Vector(double X, double Y)
    {
        this.X = X;
        this.Y = Y;
    }

    // Get the middle location of a shape
    public static Vector GetMiddleLocation(Vector Position, Vector Scale)
    {
        return new Vector((int)Position.X + (int)Scale.X / 2, (int)Position.Y + (int)Scale.Y / 2);
    }
    
    //this will get the direction to the shape
    public static Vector GetDirection(Vector tooPos, Vector fromPos)
    {
        return new Vector(tooPos.X - fromPos.X, tooPos.Y - fromPos.Y);
    }
    
    // this will get the closest shape
    public static Shape GetClosestShape(Vector Position, string tag, Shape Exceptions)
    {
        List<Shape> shapes = TestEngine.GetShapes(tag);
        if (shapes.Count == 0) return null;
        Shape currentClosest = shapes[0];
        foreach (Shape s in shapes)
        {
            if (s != Exceptions)
            {
                if(GetDistance(s.Position, Position) < GetDistance(currentClosest.Position, Position))
                {
                    currentClosest = s;
                }
            }
        }

        return currentClosest;

    }
    
    // this will get the distance between two shapes
    public static double GetDistance(Vector? point1, Vector? point2)
    {
        if (point1 != null && point2 != null)
        {
            double x = Math.Abs(point1.X - point2.X);
            double y = Math.Abs(point1.Y - point2.Y);
            return Math.Sqrt((x * x) + (y * y));
        }

        return 0;
    }
    
    public static double Magnitude(Vector? vector)
    {
        if (vector != null)
        {
            return Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));
        }

        return 0;
    }
    
    public static Vector Normalize(Vector? vector)
    {
        if (vector != null)
        {
            double magnitude = Magnitude(vector);
            return new Vector(vector.X / magnitude, vector.Y / magnitude);
        }

        return new Vector(0, 0);
    }
    
    // Get angle
    public static double GetAngle(Vector pos1, Vector pos2)
    {
        double x = pos2.X - pos1.X;
        double y = pos2.Y - pos1.Y;
        return Math.Atan2(y, x);
    }
    
    // Get Corner of shape
    public static Vector GetCorner(Vector Position, Vector Scale)
    {
        return new Vector(Position.X + Scale.X, Position.Y + Scale.Y);
    }
    
    // Get the side of a shape
    public static Vector GetSide(Vector Position, Vector Scale)
    {
        return new Vector(Position.X + Scale.X, Position.Y);
    }

    // Get Vector.Zero
    public static Vector Zero()
    {
        return new Vector(0, 0);
    }
}