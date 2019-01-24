Shader "Custom/Tetrahedron"
{
	Properties
	{
		_Transparency("Transparency", Range(0,1)) = 0.05
	}

	SubShader
	{
		Tags { "Queue" = "Transparent"}
		LOD 10000

		Pass{
			ZWrite On
			ColorMask 0
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off
			//Cull front

			CGPROGRAM
			#pragma vertex vert alpha:fade
			#pragma fragment frag alpha:fade

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			float _Transparency;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = i.color;
				col.a *= _Transparency;
				return col;
			}
			ENDCG
		}
	}
}