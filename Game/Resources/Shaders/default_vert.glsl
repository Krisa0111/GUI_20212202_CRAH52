#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aColor;
layout (location = 3) in vec2 aTexCoord;

out DATA
{
    vec3 normal;
	vec3 color;
	vec2 texCoord;
	vec3 fragPos;
} data_out;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 instanceOffset;

void main()
{
	data_out.fragPos = vec3(vec4(aPos + instanceOffset * gl_InstanceID, 1.0) * model);
    data_out.normal = normalize(aNormal * mat3(transpose(inverse(model))));  
	data_out.color = aColor;
    data_out.texCoord = aTexCoord;
    
    gl_Position = vec4(data_out.fragPos, 1.0) * view * projection;
}