namespace DungeonGame.Common;

public class Dungeon
{
    //True if room created, false if not
    public bool[,] Rooms { get; set; }
    public List<Room> AllRooms { get; set; } = new List<Room>();
    public Room CurrentRoom { get; set; }
    public (int x, int y) WinningRoom { get; private set; }

    public static int MaxSize = 10;
    public void Initialize()
    {
        Rooms = new bool[MaxSize, MaxSize];

        for (var i = 0; i < MaxSize; i++)
        {
            for (var j = 0; j < MaxSize; j++)
            {
                Rooms[i, j] = false;
            }
        }
        CurrentRoom = new Room(MaxSize / 2, MaxSize / 2);

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
    public bool PlayerMove(PlayerMovement move)
    {
        var coord_x = CurrentRoom.Coord_x;
        var coord_y = CurrentRoom.Coord_y;
        if (move == PlayerMovement.North)
        {
            coord_y++;
        }
        else if (move == PlayerMovement.South)
        {
            coord_y--;
        }
        else if (move == PlayerMovement.East)
        {
            coord_x++;
        }
        else if (move == PlayerMovement.West)
        {
            coord_x--;
        }

        if (Rooms[coord_x, coord_y])
        {
            CurrentRoom = AllRooms.First(x => x.Coord_x == coord_x && x.Coord_y == coord_y);
        }
        else
        {
            CurrentRoom = new Room(coord_x, coord_y);

            Rooms[coord_x, coord_y] = true;
            AllRooms.Add(CurrentRoom);
        }

        return CurrentRoom.Coord_x == WinningRoom.x && CurrentRoom.Coord_y == WinningRoom.y;
    }
}
public class Room
{
    public Room(int coord_x, int coord_y)
    {
        Coord_x = coord_x;
        Coord_y = coord_y;

        if (Coord_y != Dungeon.MaxSize - 1)
        {
            DoorsAvailable.Add("N");
        }
        if (Coord_y != 0)
        {
            DoorsAvailable.Add("S");
        }
        if (Coord_x != Dungeon.MaxSize - 1)
        {
            DoorsAvailable.Add("E");
        }
        if (Coord_x != 0)
        {
            DoorsAvailable.Add("W");
        }
    }

    public int Coord_x { get; set; }
    public int Coord_y { get; set; }
    public List<string> DoorsAvailable { get; set; } = new List<string>();
    public string Name => Coord_x + "." + Coord_y;
    public string Content { get; set; } //layout, mobs, etc...
}
public enum PlayerMovement
{
    North,
    South,
    East,
    West
}
