#version 140

uniform ViewProjectionMatrix
{
    mat4 viewProjection;
};

in vec3 position;
in vec4 color;

out vec4 out_color;

void main()
{
    gl_Position = viewProjection * vec4(position, 1);
    out_color = color;
}
