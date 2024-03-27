using System;

public class GroundTile: Tile
{
    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write('"');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
