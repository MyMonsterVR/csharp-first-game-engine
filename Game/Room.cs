namespace TestEngine;

public class Room : TestGame
{
    private static List<string[,]> _rooms = new List<string[,]>();
    public static int CurrentRoom = 0;
    
    public static void AddRoom(string[,] room)
    {
        _rooms.Add(room);
    }

    public static string[,] GetCurrentRooms()
    {
        return _rooms[CurrentRoom];
    }
    
    public static void NextRoom()
    {
        if (!loadingMap)
        {
            CurrentRoom++;
        }
    }

    public static List<Vector?> GetTiles(string tile)
    {
        List<Vector?> v = new List<Vector?>();

        for (int x = 0; x < GetCurrentRooms().GetLength(1); x++)
        {
            for (int y = 0; y < GetCurrentRooms().GetLength(0); y++)
            {
                if (GetCurrentRooms()[y, x] == tile)
                {
                    v.Add(new Vector(x*50, y*50));
                }
            }
        }
        
        return v;
    }
}