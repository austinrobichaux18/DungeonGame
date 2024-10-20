using Newtonsoft.Json;
using System.Reflection;

namespace DungeonGame.Common;

public class Dungeon
{
    //True if room created, false if not
    public bool[,] Rooms { get; set; }
    public List<Room> AllRooms { get; set; } = new List<Room>();
    public Room CurrentRoom { get; set; }
    public (int x, int y) WinningRoom { get; private set; }
    public List<string>? AllDescriptions { get; private set; }

    public static int MaxSize = 10;
    public void Initialize()
    {
        SetDescriptions();

        Rooms = new bool[MaxSize, MaxSize];

        for (var i = 0; i < MaxSize; i++)
        {
            for (var j = 0; j < MaxSize; j++)
            {
                Rooms[i, j] = false;
            }
        }
        CurrentRoom = new Room(MaxSize / 2, MaxSize / 2, AllDescriptions[new Random().Next(AllDescriptions.Count) - 1]);

        Rooms[MaxSize / 2, MaxSize / 2] = true;
        AllRooms.Add(CurrentRoom);

        var x = new Random().Next(MaxSize - 1);
        var y = new Random().Next(MaxSize - 1);
        while (x == MaxSize / 2 && y == MaxSize / 2)
        {
            x = new Random().Next(MaxSize - 1);
            y = new Random().Next(MaxSize - 1);
        }
        WinningRoom = (x, y);
    }
    public void PrintMap()
    {
        int rows = Rooms.GetLength(0);
        int cols = Rooms.GetLength(1);

        // Print top border
        Console.WriteLine(" " + new string('-', cols * 2 + 1));

        // Iterate through the rows of the 2D array
        for (int i = 0; i < rows; i++)
        {
            Console.Write("|"); // Start each row with a vertical bar
                                // Iterate through the columns of the 2D array
            for (int j = 0; j < cols; j++)
            {
                if (i == CurrentRoom.Coord_x && j == CurrentRoom.Coord_y)
                {
                    if (CurrentRoom.Coord_x == WinningRoom.x && CurrentRoom.Coord_y == WinningRoom.y)
                    {
                        Console.Write("A ");
                    }
                    else
                    {
                        Console.Write("O ");
                    }
                }
                else
                {
                    // If the value is true, print 'X', else print an empty space
                    Console.Write(Rooms[i, j] ? "X " : "  ");
                }
            }
            Console.WriteLine("|"); // End each row with a vertical bar
        }

        // Print bottom border
        Console.WriteLine(" " + new string('-', cols * 2 + 1));
    }
    private void SetDescriptions()
    {
        var assemblyPath = Assembly.GetExecutingAssembly().Location;
        var directoryPath = Path.GetDirectoryName(assemblyPath);
        var jsonFilePath = Path.Combine(directoryPath, @$"..\..\..\RoomDescriptions.json");
        var jsonString = File.ReadAllText(jsonFilePath);

        AllDescriptions = JsonConvert.DeserializeObject<List<string>>(jsonString);
    }

    public bool PlayerMove(PlayerMovement move)
    {
        var coord_x = CurrentRoom.Coord_x;
        var coord_y = CurrentRoom.Coord_y;
        if (move == PlayerMovement.North)
        {
            coord_x--;
        }
        else if (move == PlayerMovement.South)
        {
            coord_x++;
        }
        else if (move == PlayerMovement.East)
        {
            coord_y++;
        }
        else if (move == PlayerMovement.West)
        {
            coord_y--;
        }

        if (Rooms[coord_x, coord_y])
        {
            CurrentRoom = AllRooms.First(x => x.Coord_x == coord_x && x.Coord_y == coord_y);
        }
        else
        {
            CurrentRoom = new Room(coord_x, coord_y, AllDescriptions[new Random().Next(AllDescriptions.Count - 1)]);

            Rooms[coord_x, coord_y] = true;
            AllRooms.Add(CurrentRoom);
        }

        return CurrentRoom.Coord_x == WinningRoom.x && CurrentRoom.Coord_y == WinningRoom.y;
    }
}
public class Room
{
    public Room(int coord_x, int coord_y, string description)
    {
        Coord_x = coord_x;
        Coord_y = coord_y;

        if (Coord_x != 0)
        {
            DoorsAvailable.Add("W");
            DoorsAvailable.Add("w");
        }
        if (Coord_x != Dungeon.MaxSize - 1)
        {
            DoorsAvailable.Add("S");
            DoorsAvailable.Add("s");
        }
        if (Coord_y != Dungeon.MaxSize - 1)
        {
            DoorsAvailable.Add("D");
            DoorsAvailable.Add("d");
        }
        if (Coord_y != 0)
        {
            DoorsAvailable.Add("A");
            DoorsAvailable.Add("a");
        }
        Description = description;
    }

    public int Coord_x { get; set; }
    public int Coord_y { get; set; }
    public List<string> DoorsAvailable { get; set; } = new List<string>();
    public string Name => Coord_x + "." + Coord_y;
    public string Content { get; set; } //layout, mobs, etc...
    public string Description { get; set; }
}
public enum PlayerMovement
{
    North,
    South,
    East,
    West
}
