#version 330 core
layout (location = 0) in vec3 argPosition;
layout (location = 1) in vec3 argColor;

out vec3 vertexColor;

void main()
{
    gl_Position = vec4(argPosition, 1.0);
    vertexColor = argColor;
}
