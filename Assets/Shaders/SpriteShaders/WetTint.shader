Shader "Sprite/WetTint"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			
			#include "UnityCG.cginc"

			// vertex shader inputs
			struct vertexInput {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			// vertex shader outputs
			struct vertexOutput {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float4 worldPos : TEXCOORD1;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform Buffer<float3> _Buffer;
			
			bool isLeft(float2 a, float2 b, float2 c)
			{
				return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
			}

			// vertex shader
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(_Object2World, input.vertex);
				output.color = input.color;
				output.texcoord = input.texcoord;
				// transformation of input.vertex from object 
				// coordinates to world coordinates;
				return output;
			}
			
			fixed4 frag (vertexOutput i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
				col.rgb *= col.a;
				return col;
			}
			ENDCG
		}
	}
}
