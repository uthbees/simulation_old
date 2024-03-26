public class MountainTile : Tile
{
    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write('^');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
    
