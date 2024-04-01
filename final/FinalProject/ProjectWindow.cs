using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class ProjectWindow : IDisposable
{
    private readonly NativeWindow _window;
    private bool _closing = false;

    private readonly int _vertexBufferObject;
    private readonly int _vertexArrayObject;
    private readonly Shader _shader;

    private const int TileCountX = 15;
    private const int TileCountY = 15;
    
    // Create the vertices for our triangle. These are listed in normalized device coordinates (NDC)
    // In NDC, (0, 0) is the center of the screen.
    // Negative X coordinates move to the left, positive X move to the right.
    // Negative Y coordinates move to the bottom, positive Y move to the top.
    // OpenGL only supports rendering in 3D, so to create a flat triangle, the Z coordinate will be kept as 0.
    private readonly float[] _vertices =
    {
        -0.1f, -0.1f, 0.0f, // Bottom-left vertex
        0.1f, -0.1f, 0.0f, // Bottom-right vertex
        0.0f,  0.1f, 0.0f  // Top vertex
    };

    public ProjectWindow()
    {
        var window = new NativeWindow(new NativeWindowSettings
        {
            ClientSize = (TileCountX * 100, TileCountY * 100),
            Title = "Final Project"
        });
        _window = window;
        _window.Closing += (_) => { _closing = true; };

        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
        // TODO: orthographic projection?
        // https://stackoverflow.com/a/5879422 (?)
        // https://opentk.net/learn/chapter1/8-coordinate-systems.html (?)
        // GL.Viewport(0, 0, TileCountX * 100, TileCountY * 100);
        // GL.MatrixMode(MatrixMode.Projection);
        // GL.LoadIdentity();
        // GL.Ortho(0, TileCountX * 100, TileCountY * 100, 0, -10, 10);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        _shader.Use();
    }

    // Renders the first x by y tiles in the list
    public void RenderFrame(Dictionary<int, Dictionary<int, Tile>> tiles)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        // Show in the window the results of the rendering calls.
        _window.Context.SwapBuffers();
        Console.WriteLine(GL.GetError());
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
}
