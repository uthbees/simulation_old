using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class ProjectWindow : IDisposable
{
    private const int TileCountX = 15;
    private const int TileCountY = 15;
    private const int ResolutionX = 1500;
    private const int ResolutionY = 1500;

    private readonly NativeWindow _window;
    private bool _closing = false;

    private readonly Shader _shader;

    public ProjectWindow()
    {
        var window = new NativeWindow(new NativeWindowSettings
        {
            ClientSize = (ResolutionX, ResolutionY),
            Title = "Final Project"
        });
        _window = window;
        _window.Closing += (_) => { _closing = true; };

        // Set the background color
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);

        // Initialize the vertex array object.
        // Normally, this would hold the data that tells OpenGL how our vertex data is formatted. We're passing all our
        // vertex data through uniforms, so we don't need it, but OpenGL refuses to run if one isn't bound.
        GL.BindVertexArray(GL.GenVertexArray());

        // Create and initialize our shader program
        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        _shader.Use();

        // Set the tile size (width/height). We multiply by 2 because coordinates go from -1 to 1.
        // Note: only looks at x dimension at the moment.
        _shader.SetUniform("tileSize", 1.0f / TileCountX * 2);
    }

    // Renders the first x by y tiles in the list
    public void RenderFrame(List<List<Tile>> tiles)
    {
        // Clear the previous frame
        GL.Clear(ClearBufferMask.ColorBufferBit);

        for (int rowIndex = 0; rowIndex < TileCountY; rowIndex++)
        {
            for (int tileIndexInRow = 0; tileIndexInRow < TileCountX; tileIndexInRow++)
            {
                // Set the color and position for the next tile.
                var currentTile = tiles[rowIndex][tileIndexInRow];
                var tileTopLeftCorner = ConvertCoordsToNDC(new Vector2((float)tileIndexInRow / TileCountX,
                    1 - (float)rowIndex / TileCountY));

                _shader.SetUniform("currentColor", currentTile.GetColor());
                _shader.SetUniform("tileTopLeftCorner", tileTopLeftCorner);

                // Draw a tile (six vertices, which is two triangles) with the new uniforms.
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }
        }

        Console.WriteLine();

        // Display the result of our render calls
        _window.Context.SwapBuffers();
    }

    public void Dispose()
    {
        _window.Dispose();
        GC.SuppressFinalize(this);
        _shader.Dispose();
    }

    public bool WindowShouldClose()
    {
        return _closing;
    }

    public void RegisterKeyAction(Keys key, Action action)
    {
        _window.KeyDown += args =>
        {
            if (args.Key == key)
            {
                action();
            }
        };
    }

    public static void WaitForNextInput()
    {
        NativeWindow.ProcessWindowEvents(true);
    }

    // Converts a set of coordinates (0 to 1) to Normalized Device Coordinates (-1 to 1).
    private static Vector2 ConvertCoordsToNDC(Vector2 coords)
    {
        return new Vector2(coords.X * 2 - 1, coords.Y * 2 - 1);
    }
}
