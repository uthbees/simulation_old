public class MountainTile : Tile
{
    public MountainTile(int x, int y) : base(x, y)
    {
    }

    public override void Display()
    {
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
