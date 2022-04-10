#version 330 core

layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

in DATA
{
    vec3 normal;
	vec3 color;
	vec2 texCoord;
	vec3 fragPos;
} data_in[];

out vec3 normal;
out vec3 color;
out vec2 texCoord;
out vec3 fragPos;

void main()
{
    gl_Position = gl_in[0].gl_Position;
    normal = data_in[0].normal;
    color = data_in[0].color;
    texCoord = data_in[0].texCoord;
    fragPos = data_in[0].fragPos;
    EmitVertex();

    gl_Position = gl_in[1].gl_Position;
    normal = data_in[1].normal;
    color = data_in[1].color;
    texCoord = data_in[1].texCoord;
    fragPos = data_in[1].fragPos;
    EmitVertex();

    gl_Position = gl_in[2].gl_Position;
    normal = data_in[2].normal;
    color = data_in[2].color;
    texCoord = data_in[2].texCoord;
    fragPos = data_in[2].fragPos;
    EmitVertex();

    EndPrimitive();
}