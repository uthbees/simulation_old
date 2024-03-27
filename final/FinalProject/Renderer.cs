using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Renderer : IDisposable
{
    private readonly FinalProjectWindow _window;

    public Renderer()
    {
        var window = new FinalProjectWindow();
        _window = window;
    }

    public void Start()
    {
        _window.Run();
    }

    public void Dispose()
    {
        _window.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class FinalProjectWindow : GameWindow
{
    public FinalProjectWindow() : base(GameWindowSettings.Default, new NativeWindowSettings
    {
        ClientSize = (1600, 1600),
        Title = "Final Project"
    })
    {
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        // Show that we can use OpenGL: Clear the window to cornflower blue.
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Show in the window the results of the rendering calls.
        SwapBuffers();
    }
}
