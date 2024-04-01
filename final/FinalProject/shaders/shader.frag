#version 330 core
uniform vec3 currentColor;

out vec4 FragColor;

void main()
{
    FragColor = vec4(currentColor, 1.0f);
}
