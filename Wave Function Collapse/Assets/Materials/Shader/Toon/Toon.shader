Shader "CustomShader/Outline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_AnimationSpeed ("Animation Speed", Range(0,3)) = 0
		_OffsetSize ("Offset Size", Range(0,10)) = 0
		_Outline("Outline Shadow", Range(0,1)) = 0.2
		_Brightness("Brightness", Range(0,1)) = 0.3
		_Strength("Strength", Range(0,1)) = 0.5
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
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv: TEXCOORD0;
				float3 normal : NORMAL;
				float3 toCamera : TEXCOORD1;
			};

			CBUFFER_START(UnityPerMaterial)
			float _AnimationSpeed;
			float _OffsetSize;
			fixed4 _Color;
			sampler2D _MainTex;
			float _Outline;
			float _Brightness;
			float _Strength;
			CBUFFER_END

			float Toon(float3 normal, float3 lightDir)
			{
				float NdotL = max(0.0,dot(normalize(normal), normalize(lightDir)));
				return floor(NdotL/0.3);
			}

			v2f vert (appData In)
			{
				v2f OUT;
				
				In.vertex.x += sin(_Time.y * _AnimationSpeed * _OffsetSize);

				OUT.position = UnityObjectToClipPos(In.vertex);
				OUT.uv = In.uv;
				OUT.normal = UnityObjectToWorldNormal(In.normal);
				OUT.toCamera = _WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, In.vertex).xyz;

				return OUT;
			}

			fixed4 frag (v2f In) : SV_Target
			{
				// calculate the normal of the vertex and if it is aproximatly perpendicular to the camera, make it black
				
				float dotProduct = dot(normalize(In.normal), normalize(In.toCamera));
                if (abs(dotProduct) < _Outline) // If angle is close to 90 degrees
                {
                    return float4(0.0, 0.0, 0.0, 1.0); // Set color to black
                }
                else
                {
                    fixed4 tex = tex2D(_MainTex, In.uv)  * _Color;
					tex *= Toon(In.normal,_WorldSpaceLightPos0.xyz) * _Strength + _Brightness;
                    return tex;
                }
			}

			ENDCG
		}
	}
}