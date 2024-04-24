using System;

public class Position
{
    // Note to grader: These are technically public (at least, they use the public keyword), but in practice, it's the
    // same thing as being private with getter functions and this way is nicer to use.
    public int X { get; private set; }
    public int Y { get; private set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Position(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    public void Move(Direction direction, int distance = 1)
    {
        switch (direction)
        {
            case Direction.North:
                Y += distance;
                break;
            case Direction.East:
                X += distance;
                break;
            case Direction.South:
                Y -= distance;
                break;
            case Direction.West:
                X -= distance;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
