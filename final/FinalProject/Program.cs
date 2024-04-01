using OpenTK.Windowing.GraphicsLibraryFramework;

class Program
{
    static void Main(string[] args)
    {
        Map map = new();
        Controller controller = new(0, 0);
        using ProjectWindow projectWindow = new();

        projectWindow.RegisterKeyAction(Keys.W, () => controller.Move(Direction.North));
        projectWindow.RegisterKeyAction(Keys.Up, () => controller.Move(Direction.North));
        projectWindow.RegisterKeyAction(Keys.A, () => controller.Move(Direction.West));
        projectWindow.RegisterKeyAction(Keys.Left, () => controller.Move(Direction.West));
        projectWindow.RegisterKeyAction(Keys.S, () => controller.Move(Direction.South));
        projectWindow.RegisterKeyAction(Keys.Down, () => controller.Move(Direction.South));
        projectWindow.RegisterKeyAction(Keys.D, () => controller.Move(Direction.East));
        projectWindow.RegisterKeyAction(Keys.Right, () => controller.Move(Direction.East));

        while (!projectWindow.WindowShouldClose())
        {
            projectWindow.RenderFrame(map.GetNearbyTiles(controller.GetPosition(), 7, 7));
            ProjectWindow.WaitForNextInput();
        }
    }
}
