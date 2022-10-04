using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestEngine.Scripts;

public class Map
{
    // Map size
    private int Width { get; set; }
    private int Height { get; set; }

    // Map data
    private int[,] Data { get; set; }

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
    private int Get(int x, int y)
    {
        return Data[x, y];
    }

    // Set map data
    private void Set(int x, int y, int value)
    {
        if (x != null && y != null && value != null)
        {
            Data[x, y] = value;
        }
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

    // load map data from json file
    private void LoadJson(string path)
    {
        // read json file
        string json = File.ReadAllText(path);
        // parse json
        JObject mapdata = JObject.Parse(json);
        // use mapdata to access mapData in json file
        // find properties containing "layer" and ends with a number under the "mapData" property
        var layers = mapdata["mapData"].Children().Where(c => c.Path.Contains("layer"));
        // loop through each layer and get "layerData" property
        int x = 0;
        int y = 0;
        int value = 0;
        foreach (var layer in layers.Children())
        {
            var layerData = layer["mapDesign"];
            // loop through each layerData and get the value
            //if (layer.Path.Contains("layer" + loadedLayer))
            foreach (var data in layerData)
            {
                // foreach value in data array
                foreach (var check in data)
                {
                    switch (check.ToString())
                    {
                        case "F":
                            value = 0;
                            break;
                        case "W":
                            value = 1;
                            break;
                        case "P":
                            value = 2;
                            break;
                        case "D":
                            value = 3;
                            break;
                        default:
                            if (string.IsNullOrWhiteSpace(check.ToString()))
                            {
                                value = 9999;
                            }

                            break;
                    }

                    // set map data
                    Set(x, y, (int)value);
                    // increment x
                    x++;
                }

                // increment y
                if (x >= Width)
                {
                    x = 0;
                    y++;
                }
            }

            y = 0;
        }
    }

    // load map data length and total rows from file and save as a Vector
    private static Vector LoadSize(string path)
    {
        string json = File.ReadAllText(path);
        // parse json
        JObject mapdata = JObject.Parse(json);
        // use mapdata to access mapData in json file
        // find properties containing "layer" and ends with a number under the "mapData" property
        var layers = mapdata["mapData"]["options"];

        var rows = Int32.Parse(layers[0]["rows"].ToString());
        var columns = Int32.Parse(layers[0]["totalColumns"].ToString());
        var layerAmount = Int32.Parse(layers[0]["totalLayers"].ToString());
        return new Vector(rows, columns * layerAmount);
    }

    // Draw map
    // first draw all the empty spaces
    // then draw all the walls

    public void Draw(string mapPath, int xSize = 44, int ySize = 44, int spacing = 6)
    {
        LoadJson(mapPath);
        
        // save old position
        int oldX = 0;
        int oldY = 0;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                oldX = x * spacing;
                oldY = y * spacing;

                if (Get(x, y) == 0)
                {
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xSize, ySize), Color.Green, "Floor", TestEngine.Type.Quad);
                }

                else if (Get(x, y) == 1)
                {
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xSize, ySize), Color.Green, "Wall", TestEngine.Type.Sprite,
                        TestGame.treeSprite);
                }
                else if (Get(x, y) == 2)
                {
                    Movement._player = new Player(new Vector(oldX * spacing, oldY * spacing),
                        new Vector(xSize, ySize),
                        new Vector(xSize, ySize), Color.Green, "Player", TestEngine.Type.Sprite,
                        TestGame.playerSprite);
                }
                else if (Get(x, y) == 3)
                {
                    new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                        new Vector(xSize, ySize), Color.Black, "Door", TestEngine.Type.Quad);
                }
            }
        }

        /*for (int x = 0; x < Width; x++)
        {
            oldX = x * spacing;
            oldY = y * spacing;
            if (Get(x, y) == 2)
            {
                Movement._player = new Player(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                    new Vector(xSize, ySize), Color.Blue, "Player", TestEngine.Type.Sprite,
                    new Bitmap(TestEngine.ResourceFolder + @"\player_1.png"));
            }
        }*/

        // console output
        TestEngine.UpdateRender();
        Console.WriteLine("Map loaded");
    }
}