Shader "Tutorials/Tuto_01"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			struct appData {
				float4 vertex : POSITION;
				float2 uv: TEXCOORD0;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			CBUFFER_START(UnityPerMaterial)
			fixed4 _Color;
			sampler2D _MainTex;
			CBUFFER_END

			v2f vert (appData In)
			{
				v2f OUT;
				
				OUT.position = UnityObjectToClipPos(In.vertex);
				OUT.uv = In.uv;

				return OUT;
			}

			fixed4 frag (v2f In) : SV_Target
			{
				fixed4 pixelColor = tex2D(_MainTex, In.uv) * _Color;
				
				return pixelColor;
			}

			ENDCG
		}
	}
}