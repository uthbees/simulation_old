using System;
using System.Collections.Generic;

readonly struct TileAndPosition
{
    public Tile Tile { get; }
    public Position Position { get; }

    public TileAndPosition(Tile tile, Position position)
    {
        Tile = tile;
        Position = position;
    }
}

public class Map
{
    private readonly List<TileAndPosition> _tiles = new();
    private int seed;

    private const int HalfWidth = 20;
    private const int HalfHeight = 5;

    public void Display(Position position)
    {
        // This is absolutely terrible efficiency.
        // TODO: try to improve lookup times:
        // https://gamedev.stackexchange.com/questions/176198/recommended-data-structure-for-storage-and-fast-access-of-infinite-chunk-based-h
        for (int y = position.Y + HalfHeight; y >= position.Y - HalfHeight; y--)
        {
            for (int x = position.X - HalfWidth; x <= position.X + HalfWidth; x++)
            {
                TileAndPosition foundTile;

                int tileIndex = _tiles.FindIndex((tile) => tile.Position.X == x && tile.Position.Y == y);
                if (tileIndex != -1)
                {
                    foundTile = _tiles[tileIndex];
                }
                else
                {
                    // Generate the tile if it doesn't already exist.
                    var newTile = new TileAndPosition(GenerateTile(x, y), new Position(x, y));
                    foundTile = newTile;
                    _tiles.Add(newTile);
                }

                // TODO: If I need more things for scope, I can expand this into more of a rendering engine and add
                // support for rendering the character and other things on top of tiles.
                foundTile.Tile.Display();
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

    private static Tile GenerateTile(int x, int y)
    {
        // TODO: use noise
        if (Math.Sqrt(x * x + y * y) < 4)
        {
            return new WaterTile();
        }
        if (Math.Sqrt(x * x + y * y) < 10)
        {
            return new BeachTile();
        }

        if (x % 2 == 0)
        {
            return new GroundTile();
        }

        return new MountainTile();
    }
}
