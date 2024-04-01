using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

// Taken from https://opentk.net/learn/chapter1/2-hello-triangle.html
public class Shader : IDisposable
{
    private readonly int _handle;
    private readonly Dictionary<string, int> _uniformLocations;

    public Shader(string vertexPath, string fragmentPath)
    {
        // Set up shader objects.
        string vertexShaderSource = File.ReadAllText(vertexPath);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);

        // Compile shaders and print any errors.
        GL.CompileShader(vertexShader);

        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexSuccess);
        if (vertexSuccess == 0)
        {
            string infoLog = GL.GetShaderInfoLog(vertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(fragmentShader);

        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentSuccess);
        if (fragmentSuccess == 0)
        {
            string infoLog = GL.GetShaderInfoLog(fragmentShader);
            Console.WriteLine(infoLog);
        }

        // Set up the full shader program with the individual shaders and print any errors.
        _handle = GL.CreateProgram();

        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);

        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(_handle);
            Console.WriteLine(infoLog);
        }

        // The data has been copied to the shader program, so we can clean up the individual shaders.
        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        _uniformLocations = GetAllUniformLocations();
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
        GC.SuppressFinalize(this);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public void SetVector3Uniform(string name, Vector3 data)
    {
        GL.UseProgram(_handle);
        GL.Uniform3(_uniformLocations[name], data);
    }

    // Taken from https://github.com/opentk/LearnOpenTK/blob/master/Common/Shader.cs
    private Dictionary<string, int> GetAllUniformLocations()
    {
        var uniformLocations = new Dictionary<string, int>();

        // Get the number of active uniforms.
        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        // Iterate through the uniforms and add each to the dictionary.
        for (var i = 0; i < numberOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(_handle, i, out _, out _);
            var location = GL.GetUniformLocation(_handle, key);
            uniformLocations.Add(key, location);
        }

        // Return the list of uniform locations.
        return uniformLocations;
    }
}
