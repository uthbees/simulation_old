using System;

public struct Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}

public class Controller
{
    private int _posX;
    private int _posY;

    public Controller(int posX, int posY)
    {
        _posX = posX;
        _posY = posY;
    }

    public Position GetPosition()
    {
        return new Position(_posX, _posY);
    }

    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                _posY++;
                break;
            case Direction.East:
                _posX++;
                break;
            case Direction.South:
                _posY--;
                break;
            case Direction.West:
                _posX--;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
