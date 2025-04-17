shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
uniform float time : hint_range(0, 10);

void fragment() {
	vec2 center = vec2(0.5);
	vec2 uv = UV;

	// Push UVs slightly inward to leave room for distortion
	float margin = 0.05;
	uv = mix(center, uv, 1.0 - margin);

	// Lens distortion (barrel type)
	vec2 from_center = uv - center;
	float radius = length(from_center);
	float distortion_strength = 1.0;
	uv = from_center * (1.0 + distortion_strength * pow(radius, 6.0)) + center;

	// Clamp to prevent out-of-bounds sampling
	uv = clamp(uv, vec2(0.0), vec2(1.0));

	// Chromatic aberration on final UVs
	vec2 offset = vec2(0.0001, 0.0);
	vec3 col;
	col.r = texture(screen_texture, uv + offset).r;
	col.g = texture(screen_texture, uv).g;
	col.b = texture(screen_texture, uv - offset).b;

	// Vignette
	vec2 aspect = vec2(2.5, SCREEN_PIXEL_SIZE.y / SCREEN_PIXEL_SIZE.x);
	float dist = length((UV - center) * aspect);
	float Vradius = 0.8;
	float softness = 0.6;
	float strength = 0.5;
	float vignette = smoothstep(Vradius, Vradius + softness, dist);
	col *= mix(1.0, 1.0 - strength, vignette);

	COLOR.rgb = col;
}
