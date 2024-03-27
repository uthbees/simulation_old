using System;

public class BeachTile : Tile
{
    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.Write(' ');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
