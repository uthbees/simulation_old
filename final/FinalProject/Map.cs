public class Map
{
    private readonly List<Tile> _tiles = new();
    private int seed;

    private const int HalfWidth = 20;
    private const int HalfHeight = 5;

    public void Display(Position position)
    {
        // This is absolutely terrible efficiency, but that doesn't matter for this project.
        for (int y = position.Y - HalfHeight; y <= position.Y + HalfHeight; y++)
        {
            for (int x = position.X - HalfWidth; x <= position.X + HalfWidth; x++)
            {
                Tile foundTile;

                int tileIndex = _tiles.FindIndex((tile) => tile.X == x && tile.Y == y);
                if (tileIndex != -1)
                {
                    foundTile = _tiles[tileIndex];
                }
                else
                {
                    // Generate the tile if it doesn't already exist.
                    var newTile = GenerateTile(x, y);
                    foundTile = newTile;
                    _tiles.Add(newTile);
                }

                foundTile.Display();
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }

    public bool DirectionIsWalkable(Direction direction)
    {
        // TODO
        return true;
    }

    private Tile GenerateTile(int x, int y)
    {
        // TODO
        if (x % 2 == 0)
        {
            return new GroundTile(x, y);
        }

        return new MountainTile(x, y);
    }
}
