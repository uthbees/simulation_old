using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class ProjectWindow : IDisposable
{
    private const int TileCountX = 15;
    private const int TileCountY = 15;

    private readonly NativeWindow _window;
    private bool _closing = false;

    private readonly int _vertexArrayObject;
    private readonly Shader _shader;

    private readonly float[] _vertices =
    {
        0.5f, 0.5f, 0.0f, // top right
        0.5f, -0.5f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, // bottom left
        -0.5f, 0.5f, 0.0f, // top left
    };

    private readonly uint[] _indices =
    {
        // Note that indices start at 0!
        0, 1, 3, // The first triangle will be the top-right half of the triangle
        1, 2, 3 // Then the second will be the bottom-left half of the triangle
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

        GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, GL.GenBuffer());
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
            BufferUsageHint.StaticDraw);

        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        _shader.Use();
    }

    // Renders the first x by y tiles in the list
    public void RenderFrame(Dictionary<int, Dictionary<int, Tile>> tiles)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

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
}
