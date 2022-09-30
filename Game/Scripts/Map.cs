using TestEngine.Scripts;

namespace TestEngine.Game.Scripts;

public class Map
{
    // Map size
    public int Width { get; set; }
    public int Height { get; set; }

    // Map data
    public int[,] Data { get; set; }

    // Map constructor
    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        Data = new int[width, height];
    }

    // Map constructor with file
    public Map(string file)
    {
        var size = LoadSize(file);
        Width = (int)size.X;
        Height = (int)size.Y;
        Data = new int[Width, Height];
    }

    // Get map data
    public int Get(int x, int y)
    {
        return Data[x, y];
    }

    // Set map data
    public void Set(int x, int y, int value)
    {
        Data[x, y] = value;
    }

    // Generate map data
    // 0 = empty
    // 1 = wall
    /* example */
    // 1 1 1 1 1 1 1 1 1 1
    // 1 0 0 0 0 0 0 0 0 1
    // 1 0 0 0 0 0 0 0 0 1
    // 1 0 0 0 0 0 0 0 0 1
    // 1 1 1 1 1 1 1 1 1 1
    // in a 10x5 map
    public void Generate()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                {
                    Set(x, y, 1);
                }
                else
                {
                    Set(x, y, 0);
                }
            }
        }
    }

    // Print map data
    public void Print()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(Get(x, y));
            }

            Console.WriteLine();
        }
    }

    // Save map data to file
    public void Save(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    writer.Write(Get(x, y));
                }

                writer.WriteLine();
            }
        }
    }

    // Load map data from file
    public void Load(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            int y = 0;
            while ((line = reader.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    Set(x, y, int.Parse(line[x].ToString()));
                }

                y++;
            }
        }
    }

    // load map data length and total rows from file and save as a Vector
    private static Vector LoadSize(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            int length = 0;
            int y = 0;
            while ((line = reader.ReadLine()) != null)
            {
                y++;
                if(line.Length > length)
                    length = line.Length;
            }
            
            return new Vector(length, y);
        }
    }
    

    // Draw map
    // 0 = empty
    // 1 = wall

    /*public void Draw(string mapPath, int xSize = 44, int ySize=44, int spacing = 6)
    {
        // Load map data from file
        Load(mapPath);
        // save old position
        int oldX = 0;
        int oldY = 0;

        // Draw map
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                oldX = x * spacing;
                oldY = y * spacing;
                // first draw all the empty spaces
                if (Get(x, y) == 0)
                {
                    // Draw empty with new Shape()
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xSize, ySize), Color.LawnGreen, "Floor", TestEngine.Type.Quad);
                }

                if (Get(x, y) == 1)
                {
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xSize, ySize), Color.Green, "Wall", TestEngine.Type.Sprite, TestGame.treeSprite);
                }
            }
        }

        // console output
        TestEngine.UpdateRender();
        Console.WriteLine("Map loaded");
        
    }*/
    
    // Draw map
    // first draw all the empty spaces
    // then draw all the walls
    
    public void Draw(string mapPath, int xSize = 44, int ySize=44, int xColSize = 44, int yColSize = 44, int spacing = 6)
    {
        Load(mapPath);
        // save old position
        int oldX = 0;
        int oldY = 0;

        // Draw map
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                oldX = x * spacing;
                oldY = y * spacing;
                // first draw all the empty spaces
                if (Get(x, y) == 0)
                {
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xColSize, yColSize), Color.Green, "Floor", TestEngine.Type.Quad);
                }
            }
        }

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                oldX = x * spacing;
                oldY = y * spacing;
                if (Get(x, y) == 2)
                {
                    Movement.player = new Player(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xColSize/2, xColSize/2), Color.Blue, "Player", TestEngine.Type.Sprite, new Bitmap(TestEngine.ResourceFolder + @"\player_1.png"));
                }
            }
        }

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                oldX = x * spacing;
                oldY = y * spacing;
                if (Get(x, y) == 1)
                {
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xColSize, yColSize), Color.DarkGreen, "Wall", TestEngine.Type.Sprite, TestGame.treeSprite);
                }
            }
        }
        
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                oldX = x * spacing;
                oldY = y * spacing;
                if (Get(x, y) == 3)
                {
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xColSize, yColSize), Color.Black, "Door", TestEngine.Type.Quad);
                }
            }
        }

        // console output
        TestEngine.UpdateRender();
        Console.WriteLine("Map loaded");
        
    }
    
    
}