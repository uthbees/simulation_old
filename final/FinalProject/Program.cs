using OpenTK.Windowing.GraphicsLibraryFramework;

class Program
{
    static void Main(string[] args)
    {
        const int windowTileRadius = 10;

        Map map = new();
        Controller controller = new(0, 0);
        using ProjectWindow projectWindow = new(windowTileRadius);

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
            projectWindow.RenderFrame(map, controller.GetPosition());
            ProjectWindow.WaitForNextInput();
        }
    }
}

/*
To do list:

* Pre-submission *
- Render "avatar" (a triangle) on the center tile
- Implement terrain collision (even if that means the player will be stuck on an island)

* Post-submission *
- Smooth (fractional) movement
- Improve tile lookup efficiency
    - This might be helpful: https://gamedev.stackexchange.com/questions/176198/recommended-data-structure-for-storage-and-fast-access-of-infinite-chunk-based-h
- Click on water to place landfill?
- Improve terrain generation
 */
