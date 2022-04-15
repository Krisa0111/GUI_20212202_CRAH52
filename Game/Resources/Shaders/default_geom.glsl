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
out vec3 tangent;
out vec3 bitangent;

void main()
{
    // Edges of the triangle
    /*vec3 edge0 = gl_in[1].gl_Position.xyz - gl_in[0].gl_Position.xyz;
    vec3 edge1 = gl_in[2].gl_Position.xyz - gl_in[0].gl_Position.xyz;
    // Lengths of UV differences
    vec2 deltaUV0 = data_in[1].texCoord - data_in[0].texCoord;
    vec2 deltaUV1 = data_in[2].texCoord - data_in[0].texCoord;

    // one over the determinant
    float invDet = 1.0f / (deltaUV0.x * deltaUV1.y - deltaUV1.x * deltaUV0.y);

    tangent = normalize(vec3(invDet * (deltaUV1.y * edge0 - deltaUV0.y * edge1)));
    bitangent = normalize(vec3(invDet * (-deltaUV1.x * edge0 + deltaUV0.x * edge1)));
    normal = normalize(cross(edge1, edge0));*/


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