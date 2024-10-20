using DungeonGame.Common;

namespace DungeonGame;

public class Program
{
    public static void Main(string[] args)
    {
        var level = 1;
        while (true)
        {
            var dungeon = new Dungeon(level);
            dungeon.Initialize();

            Console.WriteLine("Welcome to the Dungeon!");
            Console.WriteLine("Enter W, A, S, D to move North/South/East/West! Enter 0 to Reset the dungeon.");
            Console.WriteLine("One room has a GOLDEN APPLE in it at random. Find that room, and you win the game!");
            Console.WriteLine("Level: " + level);
            while (true)
            {
                Console.WriteLine("You are in Room: " + dungeon.CurrentRoom.Name);
                Console.WriteLine("Total Rooms Created: " + dungeon.AllRooms.Count);
                Console.WriteLine("Doors availble to enter: " + string.Join(",", dungeon.CurrentRoom.DoorsAvailable));
                Console.WriteLine(dungeon.CurrentRoom.Description);
                dungeon.PrintMap();

                var playerMove = Console.ReadKey();
                Console.WriteLine();
                if (playerMove.KeyChar == '0')
                {
                    break;
                }
                var availbleDoors = dungeon.CurrentRoom.DoorsAvailable.Union(dungeon.CurrentRoom.DoorsAvailable.Select(x => x.ToLower()));
                if (/*playerMove.Length != 1 ||*/ !availbleDoors.Contains(playerMove.KeyChar.ToString()))
                {
                    Console.WriteLine("INVALID INPUT. Enter valid input");
                    Console.WriteLine();
                    continue;
                }

                var wins = dungeon.PlayerMove(ConvertToPlayerMovement(playerMove.KeyChar.ToString()));
                if (wins)
                {
                    Console.WriteLine("CONGRADULATIONS!! You found the apple!");
                    Console.WriteLine("Final Map:");
                    dungeon.PrintMap();

                    while (true)
                    {
                        Console.WriteLine("Go to next level? W for yes.");
                        var input = Console.ReadKey();
                        Console.WriteLine();
                        if (input.KeyChar == 'W' || input.KeyChar == 'w')
                        {
                            level++;
                            break;
                        }
                    }
                    break;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    public static PlayerMovement ConvertToPlayerMovement(string direction)
    {
        switch (direction.ToUpper())
        {
            case "W":
                return PlayerMovement.North;
            case "S":
                return PlayerMovement.South;
            case "D":
                return PlayerMovement.East;
            case "A":
                return PlayerMovement.West;
            default: //impossble
                throw new ArgumentException("Invalid direction. Please enter W, S, A, or D.");
        }
    }
}
