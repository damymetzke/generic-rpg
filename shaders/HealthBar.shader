shader_type canvas_item;

uniform float numPixels = 128.0;
uniform float progress = 1.0; 

void vertex()
{
	
}

void fragment()
{
    float pixelBorderUv = floor(UV.x * numPixels) / numPixels;
	float isLeft = (step(progress, pixelBorderUv) * -1.0) + 1.0;

    COLOR = texture(TEXTURE, UV) * isLeft;
}