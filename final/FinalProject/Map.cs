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

    public Dictionary<int, Dictionary<int, Tile>> GetNearbyTiles(Position position, int radiusX, int radiusY)
    {
        var nearbyTiles = new Dictionary<int, Dictionary<int, Tile>>();

        for (int y = position.Y + radiusY; y >= position.Y - radiusY; y--)
        {
            nearbyTiles.Add(y, new Dictionary<int, Tile>());
            for (int x = position.X - radiusX; x <= position.X + radiusX; x++)
            {
                nearbyTiles[y].Add(x, GetTile(new Position(x, y)));
            }
        }

        return nearbyTiles;
    }

    private Tile GetTile(Position position)
    {
        Tile foundTile;

        // This is absolutely terrible efficiency.
        // TODO: try to improve lookup times:
        // https://gamedev.stackexchange.com/questions/176198/recommended-data-structure-for-storage-and-fast-access-of-infinite-chunk-based-h
        int tileIndex = _tiles.FindIndex((tile) => tile.Position.X == position.X && tile.Position.Y == position.Y);
        if (tileIndex != -1)
        {
            foundTile = _tiles[tileIndex].Tile;
        }
        else
        {
            // Generate the tile if it doesn't already exist.
            foundTile = GenerateTile(position.X, position.Y);
            _tiles.Add(new TileAndPosition(foundTile, new Position(position.X, position.Y)));
        }

        return foundTile;
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
