public enum Direction
{
    North,
    East,
    South,
    West
}

public class Controller
{
    private readonly Position _position;
    private readonly Map _map;

    public Controller(Map map)
    {
        _position = new Position(0, 0);
        _map = map;
    }

    public Position GetPosition()
    {
        return _position;
    }

    public void AttemptMove(Direction direction)
    {
        var attemptedTarget = new Position(_position);
        attemptedTarget.Move(direction);

        if (_map.PositionIsWalkable(attemptedTarget))
        {
            _position.Move(direction);
        }
    }
}
