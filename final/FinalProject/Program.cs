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

/*
To do list:
- Smooth (fractional) movement
- Improve tile lookup efficiency
    - This might be helpful: https://gamedev.stackexchange.com/questions/176198/recommended-data-structure-for-storage-and-fast-access-of-infinite-chunk-based-h

- Click on water to place landfill?
- Improve terrain generation?
- Scroll to zoom?
- Send tile positions as vertex attributes or something instead of a uniform? (Updating uniforms is relatively slow.)
 */
