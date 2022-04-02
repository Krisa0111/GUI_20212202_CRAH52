#version 330 core

in vec3 normal;
in vec3 color;
in vec2 uv;

out vec4 outputColor;

uniform sampler2D texture0;

void main()
{
    outputColor = vec4(color,0.0f) * texture(texture0, uv);
}