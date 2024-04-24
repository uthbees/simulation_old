using OpenTK.Mathematics;

public class MountainTile : Tile
{
    private readonly Vector3 _color = new(0.5f, 0.5f, 0.5f);

    public override Vector3 GetColor()
    {
        return _color;
    }

    public override bool IsWalkable()
    {
        return false;
    }
}
