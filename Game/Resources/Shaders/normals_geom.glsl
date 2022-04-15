#version 330 core

layout (triangles) in;
layout (line_strip, max_vertices = 18) out;

in DATA
{
    vec3 normal;
	vec3 color;
	vec2 texCoord;
    vec3 fragPos;
} data_in[];

out vec3 color;

void main()
{
    color = vec3(0,0,1);

    gl_Position = gl_in[0].gl_Position;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + 0.1f * vec4(data_in[0].normal, 0.0f));
    EmitVertex();
    EndPrimitive();

    gl_Position = gl_in[1].gl_Position;
    EmitVertex();
    gl_Position = (gl_in[1].gl_Position + 0.1f * vec4(data_in[1].normal, 0.0f));
    EmitVertex();
    EndPrimitive();

    gl_Position = gl_in[2].gl_Position;
    EmitVertex();
    gl_Position = (gl_in[2].gl_Position + 0.1f * vec4(data_in[2].normal, 0.0f));
    EmitVertex();
    EndPrimitive();

}