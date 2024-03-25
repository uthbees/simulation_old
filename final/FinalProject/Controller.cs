public class Controller
{
    private int _posX;
    private int _posY;

    public Controller(int posX, int posY)
    {
        _posX = posX;
        _posY = posY;
    }

    public Controller()
    {
        _posX = 0;
        _posY = 0;
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
        }
    }
}
