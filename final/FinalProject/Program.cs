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

class Program
{
    static void Main(string[] args)
    {
        Map map = new();
        Controller controller = new();

        string lastInput = "";
        while (lastInput != "Q")
        {
            Console.Clear();
            map.Display(controller.GetPosition());
            Console.WriteLine("Use arrow keys or WASD to move. Press q to quit.");

            lastInput = Console.ReadKey(true).Key.ToString();
Console.WriteLine(lastInput);
            switch (lastInput)
            {
                case "W":
                case "UpArrow":
                    controller.Move(Direction.North);
                    break;
                case "A":
                case "LeftArrow":
                    controller.Move(Direction.West);
                    break;
                case "S":
                case "DownArrow":
                    controller.Move(Direction.South);
                    break;
                case "D":
                case "RightArrow":
                    controller.Move(Direction.West);
                    break;
            }
        }
    }
}
