Shader "Custom/Measurement"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Falloff("Falloff", Range(0,3)) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
			
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		Cull front

		CGPROGRAM
		#pragma surface surf Lambert alpha:fade

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};
		
		fixed4 _Color;
		float _Falloff;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Falloff * _Color;
			o.Albedo = c.rgb ;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
