﻿#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 3) in vec2 aUv;

out vec3 normal;
out vec3 color;
out vec2 uv;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform vec2 textureID;

void main()
{
    normal = aNormal;
    uv = vec2(aUv.x / 16.0 + textureID.x / 16.0 , aUv.y / 8.0 + textureID.y / 8.0);
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}