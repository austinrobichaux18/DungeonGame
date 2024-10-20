using DungeonGame.Common;

namespace DungeonGame;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            var dungeon = new Dungeon();
            dungeon.Initialize();

            Console.WriteLine("Welcome to the Dungeon!");
            Console.WriteLine("Enter N, S, E, W to move North/South/East/West! Enter 0 to Reset the dungeon.");
            Console.WriteLine("One room has a GOLDEN APPLE in it at random. Find that room, and you win the game!");
            while (true)
            {
                Console.WriteLine("You are in Room: " + dungeon.CurrentRoom.Name);
                Console.WriteLine("Total Rooms Created: " + dungeon.AllRooms.Count);
                Console.WriteLine("Doors availble to enter: " + string.Join(",", dungeon.CurrentRoom.DoorsAvailable));
                Console.WriteLine(dungeon.CurrentRoom.Description);
                dungeon.PrintMap();

                var playerMove = Console.ReadLine();

                if (playerMove == "0")
                {
                    break;
                }
                if (playerMove.Length != 1 || !dungeon.CurrentRoom.DoorsAvailable.Contains(playerMove))
                {
                    Console.WriteLine("INVALID INPUT. Enter valid input");
                    Console.WriteLine();
                    continue;
                }

                var wins = dungeon.PlayerMove(ConvertToPlayerMovement(playerMove));
                if (wins)
                {
                    Console.WriteLine("CONGRADULATIONS!! You found the apple!");
                    Console.WriteLine("Final Map:");
                    dungeon.PrintMap();

                    Console.WriteLine("Play Again? 1 for yes");
                    var input = Console.ReadLine();
                    if (input == "1")
                    {
                        break;
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
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
