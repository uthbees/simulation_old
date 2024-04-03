using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class ProjectWindow : IDisposable
{
    private const int ResolutionX = 1500;
    private const int ResolutionY = 1500;

    private readonly NativeWindow _window;
    private bool _closing = false;

    private Shader _shader;

    private readonly int _windowTileRadius;
    private readonly int _windowTileDiameter;

    public ProjectWindow(int windowTileRadius)
    {
        _windowTileRadius = windowTileRadius;
        _windowTileDiameter = windowTileRadius * 2 + 1;

        var window = new NativeWindow(new NativeWindowSettings
        {
            ClientSize = (ResolutionX, ResolutionY),
            Title = "Final Project"
        });
        _window = window;
        _window.Closing += (_) => { _closing = true; };

        InitializeRenderer();
    }

    private void InitializeRenderer()
    {
        // Set the background color
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);

        // Initialize the vertex array object.
        // Normally, this would hold the data that tells OpenGL how our vertex data is formatted. We're passing all our
        // vertex data through uniforms, so we don't need it, but OpenGL refuses to run if one isn't bound.
        GL.BindVertexArray(GL.GenVertexArray());

        // Create and initialize our shader program
        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        _shader.Use();

        // Set the tile size (width/height). We multiply by 2 because normalized device coordinates go from -1 to 1.
        // Note: only looks at x dimension at the moment.
        _shader.SetUniform("tileSize", 1.0f / _windowTileDiameter * 2);
    }

    public void RenderFrame(Map map, Position position)
    {
        // Gather rendering data
        var tiles = map.GetNearbyTiles(position, _windowTileRadius, _windowTileRadius);

        // Clear the previous frame
        GL.Clear(ClearBufferMask.ColorBufferBit);

        for (int rowIndex = 0; rowIndex < _windowTileDiameter; rowIndex++)
        {
            for (int tileIndexInRow = 0; tileIndexInRow < _windowTileDiameter; tileIndexInRow++)
            {
                // Set the color and position for the next tile.
                var currentTile = tiles[rowIndex][tileIndexInRow];
                var tileTopLeftCorner = ConvertCoordsToNDC(new Vector2((float)tileIndexInRow / _windowTileDiameter,
                    1 - (float)rowIndex / _windowTileDiameter));

                _shader.SetUniform("currentColor", currentTile.GetColor());
                _shader.SetUniform("tileTopLeftCorner", tileTopLeftCorner);

                // Draw a tile (six vertices, which is two triangles) with the new uniforms.
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }
        }

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
