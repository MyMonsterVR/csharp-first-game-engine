using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using TestEngine.Scripts;

namespace TestEngine.Game.Scripts;

public class MapContainer
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Layer { get; set; }
    public string Tile { get; set; }
}

public class MapLoader
{

    // Load map should return a 2d array of the map data
    // Draw map should take the 2d array and draw the map to the screen

    // Constructor for the map loader

    private int Width { get; set; }
    private int Height { get; set; }
    private int Layers { get; set; }

    // data holder for the map
    // containing x, y and layer
    private List<MapContainer> mapData = new();

    public MapLoader(string path)
    {
        // load the map data
        LoadMap(path);
    }

    private void LoadMap(string path)
    {
        // load the json file
        var json = File.ReadAllText(path);
        // parse the json file
        var map = JObject.Parse(json);
        // get the options
        var options = map["mapData"]["options"];
        // get the layers
        Layers = (Int32)options[0]["totalLayers"];
        // get the height
        Height = (Int32)options[0]["rows"];
        // get the width
        Width = (Int32)options[0]["totalColumns"] * Layers;
        // create the map data array

        // loop through the layers

        int layers = Layers;
        int x = 0;
        int y = 0;
        for (int i = 0; i < layers; i++)
        {
            y = 0;
            x = 0;
            // get the layer
            var layer = map["mapData"]["layer" + i];
            // get the map design
            var mapDesign = layer["mapDesign"];
            foreach (var field in mapDesign)
            {
                foreach (var tile in field)
                {
                    // add the tile to the map data
                    mapData.Add(new MapContainer
                    {
                        X = x,
                        Y = y,
                        Layer = i,
                        Tile = tile.ToString()
                    });
                    x++;
                }

                y++;
                x = 0;
            }
        }
    }

    public void DrawMap(int xSize = 44, int ySize = 44, int spacing = 6)
    {
        int oldX = 0;
        int oldY = 0;
        int oldLayer = 0;
        int layer = 0;
        // loop through the map data
        foreach (var tile in mapData)
        {
            var tiles = FindTiles(layer);
            layer++;

            if(oldLayer != tile.Layer)
            {
                oldX = 0;
                oldY = 0;
                oldLayer = tile.Layer;
            }

            foreach (var t in tiles)
            {
                oldX = t.X * spacing;
                oldY = t.Y * spacing;
                switch (t.Tile)
                {
                    case "F":
                        new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                            new Vector(xSize, ySize), Color.Green, "Floor", TestEngine.Type.Quad);
                        break;
                    case "W":
                        new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                            new Vector(xSize, ySize), Color.Green, "Wall", TestEngine.Type.Sprite,
                            TestGame.treeSprite);
                        break;
                    case "P":
                        Movement._player = new Player(new Vector(oldX * spacing, oldY * spacing),
                            new Vector(xSize, ySize),
                            new Vector(xSize, ySize), Color.Green, "Player", TestEngine.Type.Sprite,
                            TestGame.playerSprite);
                        break;
                    case "D":
                        new Shape(new Vector(oldX * spacing, oldY * spacing), new Vector(xSize, ySize),
                            new Vector(xSize, ySize), Color.Black, "Door", TestEngine.Type.Quad);
                        break;
                    default:
                        continue;
                }
            }
        }
    }

    // find every tile with a given layer
    public List<MapContainer> FindTiles(int layer)
    {
        return mapData.FindAll(x => x.Layer == layer);
    }
}