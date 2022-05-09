#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D screenTexture;

void main()
{
    //vec3 col = texture(screenTexture, TexCoords).rgb;
    //FragColor = vec4(col, 1.0);

    vec3 fragment = texture(screenTexture, TexCoords).rgb;

    float gamma  = 2.2;
    float exposure = 0.1f;
    vec3 toneMapped = vec3(1.0f) - exp(-fragment * exposure);

    FragColor.rgb = pow(toneMapped, vec3(1.0f / gamma));
}