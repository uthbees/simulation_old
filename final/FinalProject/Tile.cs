public abstract class Tile
{
    public readonly int X;
    public readonly int Y;

    protected Tile(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public abstract void Display();
    public abstract bool IsWalkable();
}
