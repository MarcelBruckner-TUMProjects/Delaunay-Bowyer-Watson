Shader "Custom/Tetrahedron Surf"
{
	Properties
	{
		_Transparency("Transparency", Range(0,1)) = 0.02
	}

		SubShader
	{ 
		//Blend SrcAlpha OneMinusSrcAlpha
			//ZWrite off
			//Cull front
		Tags { "Queue" = "Transparent"}
		/*Pass{
			ZWrite On
			ColorMask 0
		}*/

		CGPROGRAM

#pragma surface surf Lambert alpha:fade 
		struct Input {
			float4 color : COLOR;
		};

	float _Transparency;

	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = float3(1,1,1);
		o.Alpha = _Transparency;
		}

		ENDCG
	}
}