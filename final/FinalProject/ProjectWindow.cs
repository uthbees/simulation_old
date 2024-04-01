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

    private readonly Shader _shader;

    private readonly float[] _verticesData =
    {
        // positions        // colors
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // bottom left
        0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f, // top right
        -0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f // top left
    };

    private readonly uint[] _vertexIndices =
    {
        0, 1, 2, // The first triangle will be the top-right half of the triangle
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

        // Set the background color
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
        
        // Initialize the array buffer (this will hold our vertex data)
        GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
        // Load our vertex data into the array buffer
        GL.BufferData(BufferTarget.ArrayBuffer, _verticesData.Length * sizeof(float), _verticesData,
            BufferUsageHint.StaticDraw);

        // Initialize the vertex array object (this will hold the data that tells OpenGL how to read our vertex data)
        GL.BindVertexArray(GL.GenVertexArray());

        // Load our configuration into the vertex array object
        // Argument 0 (position) occurs every 6 slots with no offset
        // Argument 1 (color) occurs every 6 slots with an offset of 3 slots
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // Initialize the element array buffer (this will tell OpenGL how to assemble our vertices into larger shapes, such as rectangles)
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, GL.GenBuffer());
        // Load our configuration into the element array buffer
        GL.BufferData(BufferTarget.ElementArrayBuffer, _vertexIndices.Length * sizeof(uint), _vertexIndices,
            BufferUsageHint.StaticDraw);

        // Create and initialize our shader program
        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        _shader.Use();
    }

    // Renders the first x by y tiles in the list
    public void RenderFrame(Dictionary<int, Dictionary<int, Tile>> tiles)
    {
        // Clear the previous frame
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Draw the data that we previously loaded into the array buffer
        GL.DrawElements(PrimitiveType.Triangles, _vertexIndices.Length, DrawElementsType.UnsignedInt, 0);

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
}
