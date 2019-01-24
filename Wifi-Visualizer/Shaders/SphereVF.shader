Shader "Custom/SphereVF"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Transparency("Transparency", Range(0,2)) = 0.2
		_Falloff("Falloff", Range(0,10)) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert alpha:fade
			#pragma fragment frag alpha:fade

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
					float3 worldPos : TEXCOORD0;
          float4 pos : SV_POSITION;
    			half3 normal : NORMAL;
			};

					sampler2D _MainTex;
					fixed4 _Color;
					float _Transparency;
					float _Falloff;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.normal = v.normal;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
        fixed4 col = tex2D(_MainTex, i.worldPos) * _Color;

	      half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				col.a = saturate(pow(dot(i.normal, worldViewDir), _Falloff) * _Transparency);
        return col;
			}
			ENDCG
		}
	}
}
