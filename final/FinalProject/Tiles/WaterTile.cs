public class WaterTile : Tile
{
    public WaterTile(int x, int y) : base(x, y)
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
