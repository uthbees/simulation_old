using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public struct FrameData
{
    public FrameData()
    {
    }
}

public class WindowManager : IDisposable
{
    private readonly NativeWindow _window;
    private bool _closing = false;

    public WindowManager()
    {
        var window = new NativeWindow(new NativeWindowSettings
        {
            ClientSize = (1600, 1600),
            Title = "Final Project"
        });
        _window = window;
        _window.Closing += (_) => { _closing = true; };
    }

    public void Dispose()
    {
        _window.Dispose();
        GC.SuppressFinalize(this);
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

    public void RenderFrame(FrameData frameData)
    {
        GL.ClearColor(0.39f, 0.58f, 0.93f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Show in the window the results of the rendering calls.
        _window.Context.SwapBuffers();
    }
}
