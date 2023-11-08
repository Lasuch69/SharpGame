shader_type canvas_item;

uniform bool enabled = true;

void fragment() {
	
	float value = 0.0;
	
	for (int y = -1; y <= 1; y++) {
		
		if (y == 0) {
			continue;
		}
		
		if (texture(TEXTURE, UV + vec2(0.0, float(y) * TEXTURE_PIXEL_SIZE.y)).a > 0.0) {
			value = 1.0;
		}
	}
	
	for (int x = -1; x <= 1; x++) {
		
		if (x == 0) {
			continue;
		}
		
		if (texture(TEXTURE, UV + vec2(float(x) * TEXTURE_PIXEL_SIZE.x, 0.0)).a > 0.0) {
			value = 1.0;
		}
	}
	
	if (enabled) {
		COLOR.rgba = vec4(value - COLOR.a);
	}
}
