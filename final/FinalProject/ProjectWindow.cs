using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
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

        // Assemble the vertex data for the number of tiles we'll be displaying
        var verticesData = AssembleVertices(TileCountX, TileCountY);
        var vertexIndices = AssembleTileVertexIndices(TileCountX, TileCountY);

        // Initialize the array buffer (this will hold our vertex data)
        GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GenBuffer());
        // Load our vertex data into the array buffer
        GL.BufferData(BufferTarget.ArrayBuffer, verticesData.Length * sizeof(float), verticesData,
            BufferUsageHint.StaticDraw);

        // Initialize the vertex array object (this will hold the data that tells OpenGL how our vertex data is formatted)
        GL.BindVertexArray(GL.GenVertexArray());

        // Load our format configuration into the vertex array object
        // Argument 0 (position) occurs every 3 slots with no offset
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Initialize the element array buffer (this will tell OpenGL how to assemble our vertices into larger shapes, such as rectangles)
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, GL.GenBuffer());
        // Load our configuration into the element array buffer
        GL.BufferData(BufferTarget.ElementArrayBuffer, vertexIndices.Length * sizeof(uint), vertexIndices,
            BufferUsageHint.StaticDraw);

        // Create and initialize our shader program
        _shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        _shader.Use();
    }

    // Renders the first x by y tiles in the list
    public void RenderFrame(List<List<Tile>> tiles)
    {
        // Clear the previous frame
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // TODO: Would it instead be better to just have the vertices data for one tile and to pass the position as a
        // uniform?
        // If I implement smooth movement, I think the answer is definitely yes, because I'd have to pass an offset
        // as a uniform anyway.
        for (int rowIndex = 0; rowIndex < TileCountY; rowIndex++)
        {
            for (int tileIndexInRow = 0; tileIndexInRow < TileCountX; tileIndexInRow++)
            {
                // Set the color for the next tile.
                var currentTile = tiles[rowIndex][tileIndexInRow];
                _shader.SetVector3Uniform("currentColor", currentTile.GetColor());

                // Draw the next tile.
                int absoluteTileIndex = rowIndex * TileCountX + tileIndexInRow;
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt,
                    6 * sizeof(uint) * absoluteTileIndex);
            }
        }

        // Draw the data that we previously loaded into the array buffer
        // GL.DrawElements(PrimitiveType.Triangles, AssembleTileVertexIndices(TileCountX, TileCountY).Length, DrawElementsType.UnsignedInt, 0);

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

    // Assembles the vertices for the number of tiles we'll need.
    // Vertices are ordered left to right, top to bottom.
    private static float[] AssembleVertices(int tileCountX, int tileCountY)
    {
        // The number of vertices we will be assembling. Each vertex is three floats (positions in one dimension).
        int totalVertices = (tileCountX + 1) * (tileCountY + 1);

        float xVerticesGap = 1.0f / tileCountX;
        float yVerticesGap = 1.0f / tileCountY;

        float[] verticesData = new float[3 * totalVertices];

        for (int vertexIndex = 0; vertexIndex < totalVertices; vertexIndex++)
        {
            // Calculate x and y indices.
            int xIndex = vertexIndex % (tileCountX + 1);
            int yIndex = vertexIndex / (tileCountX + 1);

            // Calculate x and y positions if 0, 0 was the top left corner.
            // The "* 2 - 1" part maps the range from 0 - 1 to -1 - 1.
            float rawXPosition = xIndex * xVerticesGap * 2 - 1;
            float rawYPosition = yIndex * yVerticesGap * 2 - 1;

            // Calculate Normalized Device Coordinates - between -1 and 1, where 0, 0 is the center.
            float normalizedXPosition = rawXPosition * 2 - 1;
            float normalizedYPosition = -rawYPosition * 2 + 1;

            // Push x and y positions to verticesData, plus 0 for the z position.
            int realIndex = vertexIndex * 3;
            verticesData[realIndex] = normalizedXPosition;
            verticesData[realIndex + 1] = normalizedYPosition;
            verticesData[realIndex + 2] = 0.0f;
        }

        return verticesData;
    }

    // Assembles the index data that allows us to turn the triangle vertexes into squares.
    // Indices are per vertex, not per position. So one index corresponds to three floats in the vertices array,
    // since each vertex is composed of three floats.
    // Tiles are expected to be rendered left to right, top to bottom.
    private static uint[] AssembleTileVertexIndices(int tileCountX, int tileCountY)
    {
        // The number of tiles we will be assembling. Each tile is six indices.
        int totalTiles = tileCountX * tileCountY;

        uint[] tileVertexIndices = new uint[6 * totalTiles];

        for (int tileIndex = 0; tileIndex < totalTiles; tileIndex++)
        {
            // Calculate x and y tile indices.
            int xTileIndex = tileIndex % tileCountX;
            int yTileIndex = tileIndex / tileCountX;

            // Calculate the indices of the four corners of this tile.
            uint tileTopLeftIndex = (uint)yTileIndex * ((uint)tileCountX + 1) + (uint)xTileIndex;
            uint tileTopRightIndex = tileTopLeftIndex + 1;
            uint tileBottomLeftIndex = tileTopLeftIndex + (uint)tileCountX + 1;
            uint tileBottomRightIndex = tileBottomLeftIndex + 1;

            // Push six indices to vertexIndices. The first three are the top left triangle of the square and the
            // second three are the bottom right triangle.
            int realIndex = tileIndex * 6;
            tileVertexIndices[realIndex] = tileBottomLeftIndex;
            tileVertexIndices[realIndex + 1] = tileTopRightIndex;
            tileVertexIndices[realIndex + 2] = tileTopLeftIndex;
            tileVertexIndices[realIndex + 3] = tileBottomLeftIndex;
            tileVertexIndices[realIndex + 4] = tileTopRightIndex;
            tileVertexIndices[realIndex + 5] = tileBottomRightIndex;
        }

        return tileVertexIndices;
    }
}
