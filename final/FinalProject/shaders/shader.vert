#version 330 core
uniform float tileSize;
uniform vec2 tileTopLeftCorner;

void main()
{
    switch (gl_VertexID) {
        case 0:
            // Top left
            gl_Position = vec4(tileTopLeftCorner.xy, 0.0, 1.0);
            break;
        case 1:
            // Top right
            gl_Position = vec4(tileTopLeftCorner.x + tileSize, tileTopLeftCorner.y, 0.0, 1.0);
            break;
        case 2:
            // Bottom left
            gl_Position = vec4(tileTopLeftCorner.x, tileTopLeftCorner.y - tileSize, 0.0, 1.0);
            break;
        case 3:
            // Bottom right
            gl_Position = vec4(tileTopLeftCorner.x + tileSize, tileTopLeftCorner.y - tileSize, 0.0, 1.0);
            break;
        case 4:
            // Top right
            gl_Position = vec4(tileTopLeftCorner.x + tileSize, tileTopLeftCorner.y, 0.0, 1.0);
            break;
        case 5:
            // Bottom left
            gl_Position = vec4(tileTopLeftCorner.x, tileTopLeftCorner.y - tileSize, 0.0, 1.0);
            break;
    }
}
