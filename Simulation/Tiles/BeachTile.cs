using OpenTK.Mathematics;

public class BeachTile : Tile
{
    private readonly Vector3 _color = new(1, 0.8f, 0);

    public override Vector3 GetColor()
    {
        return _color;
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
