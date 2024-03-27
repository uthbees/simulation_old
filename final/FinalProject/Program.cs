using System;
using OpenTK.Windowing.GraphicsLibraryFramework;

class Program
{
    static void Main(string[] args)
    {
        Map map = new();
        Controller controller = new(0, 0);
        using WindowManager windowManager = new();

        windowManager.RegisterKeyAction(Keys.A, () => Console.WriteLine("a"));

        while (!windowManager.WindowShouldClose())
        {
            windowManager.RenderFrame(new FrameData());
            WindowManager.WaitForNextInput();
        }

        // string lastInput = "";
        // while (lastInput != "Q")
        // {
        //     Console.Clear();
        //     map.Display(controller.GetPosition());
        //     Console.WriteLine("Use arrow keys or WASD to move. Press q to quit.");
        //
        //     lastInput = Console.ReadKey(true).Key.ToString();
        //
        //     switch (lastInput)
        //     {
        //         case "W":
        //         case "UpArrow":
        //             controller.Move(Direction.North);
        //             break;
        //         case "A":
        //         case "LeftArrow":
        //             controller.Move(Direction.West);
        //             break;
        //         case "S":
        //         case "DownArrow":
        //             controller.Move(Direction.South);
        //             break;
        //         case "D":
        //         case "RightArrow":
        //             controller.Move(Direction.East);
        //             break;
        //     }
        // }
    }
}
