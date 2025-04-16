shader_type canvas_item;

uniform float time : hint_range(0, 10);

void fragment() {
	// horiz CRT
	float scanline = sin(UV.y * 800.0) * 0.5;
	COLOR.rgb = texture(TEXTURE, UV).rgb - scanline;
	// Chromatic Aberration
	vec2 offset = vec2(0.002, 0.0);
	vec3 col;
	col.r = texture(TEXTURE, UV + offset).r;
	col.g = texture(TEXTURE, UV).g;
	col.b = texture(TEXTURE, UV - offset).b;
	COLOR.rgb = col;
	// Lens dist
	vec2 center = vec2(0.5);
	vec2 uv = UV;

	// Push UVs inward slightly to give distortion room to breathe
	float margin = 0.05;
	uv = mix(center, uv, 1.0 - margin); // 5% inward shift

	// Apply lens distortion
	vec2 from_center = uv - center;
	float radius = length(from_center);
	uv += from_center * radius * 0.4;

	// Clamp final UVs to stay safe
	uv = clamp(uv, vec2(0.0), vec2(1.0));

	COLOR = texture(TEXTURE, uv);

	// vignette
	vec2 aspect = vec2(2.5, SCREEN_PIXEL_SIZE.y / SCREEN_PIXEL_SIZE.x);
	float dist = length((UV - center) * aspect);


	// Parameters
	float Vradius = 0.8;      // How far the vignette reaches before fading
	float softness = 0.9;    // How gradually it fades
	float strength = 0.5;    // 0 = no vignette, 1 = full black at edges

	// Vignette mask using smoothstep
	float vignette = smoothstep(Vradius, Vradius + softness, dist);

	// Apply to color
	COLOR.rgb *= mix(1.0, 1.0 - strength, vignette);

}

