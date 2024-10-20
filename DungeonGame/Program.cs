﻿using DungeonGame.Common;

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
            case "N":
                return PlayerMovement.North;
            case "S":
                return PlayerMovement.South;
            case "E":
                return PlayerMovement.East;
            case "W":
                return PlayerMovement.West;
            default: //impossble
                throw new ArgumentException("Invalid direction. Please enter N, S, E, or W.");
        }
    }
}
