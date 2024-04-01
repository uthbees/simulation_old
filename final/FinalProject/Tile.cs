using OpenTK.Mathematics;

public abstract class Tile
{
    public abstract Vector3 GetColor();
    public abstract bool IsWalkable();
}
