using OpenTK.Windowing.GraphicsLibraryFramework;

class Program
{
    static void Main(string[] args)
    {
        const int windowTileRadius = 10;

        Map map = new();
        Controller controller = new(map);
        using ProjectWindow projectWindow = new(windowTileRadius);

        projectWindow.RegisterKeyAction(Keys.W, () => controller.AttemptMove(Direction.North));
        projectWindow.RegisterKeyAction(Keys.Up, () => controller.AttemptMove(Direction.North));
        projectWindow.RegisterKeyAction(Keys.A, () => controller.AttemptMove(Direction.West));
        projectWindow.RegisterKeyAction(Keys.Left, () => controller.AttemptMove(Direction.West));
        projectWindow.RegisterKeyAction(Keys.S, () => controller.AttemptMove(Direction.South));
        projectWindow.RegisterKeyAction(Keys.Down, () => controller.AttemptMove(Direction.South));
        projectWindow.RegisterKeyAction(Keys.D, () => controller.AttemptMove(Direction.East));
        projectWindow.RegisterKeyAction(Keys.Right, () => controller.AttemptMove(Direction.East));

        while (!projectWindow.WindowShouldClose())
        {
            projectWindow.RenderFrame(map, controller.GetPosition());
            ProjectWindow.WaitForNextInput();
        }
    }
}
