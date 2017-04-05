// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Sprite/2DMetaballSpriteShader"
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
		Tags { "Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True" }

	Pass
	{
	
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
	//uniform Buffer<float2> _Buffer;
	uniform float xPos[300];
	uniform float yPos[300];
	//float2 lastPos;
	// Set Variables
	int _Width;

	// vertex shader inputs
	struct vertexInput {
		float4 vertex : POSITION;
	};

	// vertex shader outputs
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float2 worldPos : TEXCOORD0;
	};

	// Main equation to get the intensity of a position from a specific ball
	float GetIntensity(float2 start, float pX, float pY, fixed radius)
	{
		float x = (start.x - pX);
		float y = (start.y - pY);
		return (radius / ((x * x) + (y * y)));
	}

	// gets the totalintensity from all balls for this position
	float totalIntensity(float2 pos)
	{
		float totalIntensity = 0;

		for (int i = 0; i < _Width; i++)
		{
			totalIntensity += GetIntensity(pos, xPos[i], yPos[i], _Radius);
		}

		return totalIntensity;
	}

	// vertex shader
	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
		output.worldPos = mul(unity_ObjectToWorld, input.vertex);
		return output;
	}


	// pixel shader; returns low precision ("fixed4" type)
	// color ("SV_Target" semantic)
	fixed4 frag(vertexOutput i) : SV_Target
	{
		
		float intensity = totalIntensity(i.worldPos);

		//if (intensity < 1 - _BrimSize)
		//	discard;

		if (intensity < 1)
			discard;//return _BrimColor;

		return _WaterColor;
	}

		ENDCG
	}

	}
}
