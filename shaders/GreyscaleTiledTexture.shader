shader_type canvas_item;

uniform float numTiled = 1.0;
uniform vec4 colorModifier : hint_color = vec4(1.0, 1.0, 1.0, 1.0);

uniform float currentTile = 0.0;

void vertex()
{

}

void fragment()
{
    float tileOffset = mod(currentTile / numTiled,  1.0);
    float localOffset = UV.x / numTiled;
    COLOR = texture(TEXTURE, vec2(tileOffset + localOffset, UV.y)) * colorModifier;
}