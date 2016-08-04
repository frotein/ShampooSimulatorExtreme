// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Sprite/WetTint"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor ("Color", Color) = (1,1,1,1)
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
				float2 texcoord : TEXCOORD0;
			};

			// vertex shader outputs
			struct vertexOutput {
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			int _Width;
			uniform Buffer<float4> _LeftLines;
			uniform Buffer<float4> _RightLines;
			float4 _TintColor;
			bool inside(float2 a, float2 b, float2 c, bool left)
			{
				bool val = ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
				return  val == left;
			}

			bool IsInLines(float2 wPoint)
			{
				for (int i = 0; i < _Width; i++)
				{
					if (inside(_LeftLines[i].xy, _LeftLines[i].zw, wPoint, true) &&
						inside(_RightLines[i].xy, _RightLines[i].zw, wPoint, false) && 
						inside(_LeftLines[i].xy, _RightLines[i].xy, wPoint, false) &&
						inside(_LeftLines[i].zw, _RightLines[i].zw, wPoint, true))
					{
						return true;
					}
				}

				return false;
			}
			// vertex shader
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(unity_ObjectToWorld, input.vertex);
				output.texcoord = input.texcoord;
				// transformation of input.vertex from object 
				// coordinates to world coordinates;
				return output;
			}
			
			fixed4 frag (vertexOutput i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.texcoord);
				col.rgb *= col.a;
				if (IsInLines(i.worldPos))
					col *= _TintColor;

				return col;
			}
			ENDCG
		}
	}
}
