public enum Direction
{
    North,
    East,
    South,
    West
}

public class Map
{
    private List<Tile> _tiles = new();
    private int seed;

    private const int HALF_WIDTH = 5;
    private const int HALF_HEIGHT = 5;

    public void Display(int centerX, int centerY)
    {
        // Generate the tiles in view if they don't already exist
        // This is absolutely terrible efficiency, but that doesn't matter for this project
        for (int x = centerX - HALF_WIDTH; x <= centerX + HALF_WIDTH; x++)
        {
            for (int y = centerY - HALF_HEIGHT; y <= centerY + HALF_HEIGHT; y++)
            {
                Tile foundTile;

                int tileIndex = _tiles.FindIndex((tile) => tile.X == x && tile.Y == y);
                if (tileIndex != -1)
                {
                    foundTile = _tiles[tileIndex];
                }
                else
                {
                    Tile newTile = GenerateTile(x, y);
                    foundTile = newTile;
                    _tiles.Add(newTile);
                }

                // TODO: actually display something
            }
        }
    }

    public bool DirectionIsWalkable(Direction direction)
    {
        // TODO
        return true;
    }

    private Tile GenerateTile(int x, int y)
    {
        // TODO
        return new GroundTile(x, y);
    }
}
