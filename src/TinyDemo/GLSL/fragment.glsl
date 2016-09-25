#version 140

in vec4 out_color;
out vec4 fragmentColor;

void main()
{
    vec4 color = out_color;
    fragmentColor = color;
}
