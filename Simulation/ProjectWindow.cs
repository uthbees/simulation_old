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

    private Shader _avatarShader;
    private Shader _tileShader;

    private readonly int _tilesFromCenterOfWindow;
    private readonly int _tilesAcrossWindow;

    // A simple triangle.
    private readonly float[] _avatarVertices =
    {
        0.0f, 0.5f, 0.0f, // Top
        -0.35f, -0.5f, 0.0f, // Bottom left
        0.35f, -0.5f, 0.0f // Bottom right
    };

    private readonly Vector3 _avatarColor = new(1, 0.5f, 0);

    public ProjectWindow(int tilesFromCenterOfWindow)
    {
        _tilesFromCenterOfWindow = tilesFromCenterOfWindow;
        _tilesAcrossWindow = tilesFromCenterOfWindow * 2 + 1;

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
        // Set the background color.
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);

        // Initialize the array buffer (this will hold our vertex data).
        GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
        // Load the avatar vertex data into the array buffer, since the avatar is the only thing that uses it.
        GL.BufferData(BufferTarget.ArrayBuffer, _avatarVertices.Length * sizeof(float), _avatarVertices,
            BufferUsageHint.StaticDraw);

        // Initialize the vertex array object (this will hold the data that tells OpenGL how our vertex data is formatted).
        GL.BindVertexArray(GL.GenVertexArray());

        // Load our format configuration into the vertex array object:
        // Argument 0 (position) occurs every 3 slots with no offset.
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Create our tile shader programs.
        _avatarShader = new Shader("shaders/transformableShader.vert", "shaders/genericShader.frag");
        _tileShader = new Shader("shaders/tileShader.vert", "shaders/genericShader.frag");

        // Set the shader uniforms that don't get updated every frame.
        var tileScaleFactor = 1.0f / _tilesAcrossWindow;
        _avatarShader.SetUniform("color", _avatarColor);
        _avatarShader.SetUniform("transform", Matrix4.CreateScale(tileScaleFactor, tileScaleFactor, 1));
        // Tile size is both width and height. We multiply by 2 because normalized device coordinates go from -1 to 1.
        _tileShader.SetUniform("tileSize", tileScaleFactor * 2);
    }

    public void RenderFrame(Map map, Position position)
    {
        // Gather rendering data.
        var tiles = map.GetNearbyTiles(position, _tilesFromCenterOfWindow, _tilesFromCenterOfWindow);

        // Clear the previous frame.
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Draw the tiles.
        _tileShader.Use();
        for (int rowIndex = 0; rowIndex < _tilesAcrossWindow; rowIndex++)
        {
            for (int tileIndexInRow = 0; tileIndexInRow < _tilesAcrossWindow; tileIndexInRow++)
            {
                // Set the color and position for the next tile.
                var currentTile = tiles[rowIndex][tileIndexInRow];
                var tileTopLeftCorner = ConvertCoordsToNDC(new Vector2((float)tileIndexInRow / _tilesAcrossWindow,
                    1 - (float)rowIndex / _tilesAcrossWindow));

                _tileShader.SetUniform("color", currentTile.GetColor());
                _tileShader.SetUniform("tileTopLeftCorner", tileTopLeftCorner);

                // Draw a tile (six vertices, which is two triangles) with the new uniforms.
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }
        }

        // Draw the avatar.
        _avatarShader.Use();
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        // Display the result of our render calls.
        _window.Context.SwapBuffers();
    }

    public void Dispose()
    {
        _window.Dispose();
        GC.SuppressFinalize(this);
        _tileShader.Dispose();
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
