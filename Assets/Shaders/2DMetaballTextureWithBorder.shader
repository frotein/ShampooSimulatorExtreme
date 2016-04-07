Shader "Unlit/2DMetaballTextureWithBorder"
{
	Properties
	{
		// we have removed support for texture tiling/offset,
		// so make them not be displayed in material inspector
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		_WaterColor("Water Color", Color) = (1,1,1,1)
		_BrimColor("Brim Color", Color) = (1,1,1,1)
		_Radius("Ball Radius", Float) = 1
		_BrimSize("Brim Size", Float) = 0.1

	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass
		{
			Cull Off // turn off backface culling

			CGPROGRAM
			// use "vert" function as the vertex shader
			#pragma vertex vert
			// use "frag" function as the pixel (fragment) shader
			#pragma fragment frag

			// Property variables
			uniform sampler2D _MainTex;
		fixed4 _WaterColor;
		fixed4 _BrimColor;
		fixed _Radius;
		fixed _BrimSize;
		uniform Buffer<float2> _Buffer;

		// Set Variables
		int _Width;

		// vertex shader inputs
		struct vertexInput {
			float4 vertex : POSITION;
		};

		// vertex shader outputs
		struct vertexOutput {
			float4 pos : SV_POSITION;
			float4 worldPos : TEXCOORD0;
			//float intensity : TESSFACTOR2;
		};

			// Main equation to get the intensity of a position from a specific ball
			float GetIntensity(float2 start, float2 end, fixed radius)
			{
				float x = (start.x - end.x);
				float y = (start.y - end.y);
				return (radius / ((x * x) + (y * y)));
			}

			// gets the totalintensity from all balls for this position
			float totalIntensity(float2 pos)
			{
				float totalIntensity = 0;

				for (int i = 0; i < _Width; i++)
				{
					totalIntensity += GetIntensity(pos, _Buffer[i], _Radius);
				}
				
				return totalIntensity;
			}

			// vertex shader
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(_Object2World, input.vertex);
				//output.intensity = totalIntensity(output.worldPos);
				// transformation of input.vertex from object 
				// coordinates to world coordinates;
				return output;
			}


			// pixel shader; returns low precision ("fixed4" type)
			// color ("SV_Target" semantic)
			fixed4 frag(vertexOutput i) : SV_Target
			{
				float intensity = totalIntensity(i.worldPos);
				if (intensity < 1 - _BrimSize)
				discard;
				
				if (intensity < 1)
					return _BrimColor;


				return _WaterColor;
			}
			ENDCG
		}

	}
}
