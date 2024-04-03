using OpenTK.Mathematics;

public class WaterTile : Tile
{
    private readonly Vector3 _color = new(0, 0, 1);

    public override Vector3 GetColor()
    {
        return _color;
    }

    public override bool IsWalkable()
    {
        return false;
    }
}
