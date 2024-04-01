using OpenTK.Mathematics;

public class GroundTile : Tile
{
    private readonly Vector3 _color = new(0.2f, 0.5f, 0.2f);

    public override Vector3 GetColor()
    {
        return _color;
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
