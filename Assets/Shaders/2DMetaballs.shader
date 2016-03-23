Shader "Hidden/2dMetaballs"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PositionsTex("-", 2D) = "grey" {}
		_Color("Water Color", Color) = (0.1,0.1 ,1,1)
	}
		CGINCLUDE
	#include "UnityCG.cginc"
	
	sampler2D _PositionsTex;
	float _Radius;
	float4 _Color;
	int _width;
	int _ScreenHeight;
	int _ScreenWidth;
	float _TestX;
	float _TestY;
	float _XScale;
	float _YScale;

	float GetIntensity(float2 start, float2 end, float radius)
	{
		return (radius / (((start.x - end.x) * (start.x - end.x)) + ((start.y - end.y) * (start.y - end.y)) + 0.000001));	
	
	}
	float2 UVToWorld(float2 uv)
	{
		float2 pt = { (uv.x * 2 * _XScale) - _XScale , (uv.y * 2 * _YScale) - _YScale };
		return pt;
	}

	float totalIntensity(float2 pos)
	{
		float widthF = _width;
		float totalIntensity = 0;
		
		float2 fPos = UVToWorld(pos);
		fPos.y = -fPos.y;
		//float2 screen = { _ScreenWidth * 1.17, _ScreenHeight * 1.17 };
		for (int i = 0; i < _width; i++)
		{
			float iFloat = i;
			float2 uv = float2((iFloat + 1.0) / 100.0, 0.5);
			fixed4 col = tex2D(_PositionsTex, uv);
			float2 uvPos = { col.x , col.y };
			float2 ballPos = UVToWorld(uvPos);
			totalIntensity += GetIntensity(ballPos, fPos, _Radius);
		}
		
		return totalIntensity;
	}
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				
				//float2 screenPos = { i.uv.x * _ScreenWidth , i.uv.y * _ScreenHeight };
				//float2 worldPos = UVToWorld(i.uv);
				
				float intensity = totalIntensity(i.uv);
				
				//if(i.uv.x > _TestX)
				//	col = _Color;

				if(intensity > 1)
					col = _Color;
				//if (screenPos.y > _TestY0)
				//	col = _Color;

				return col;
			}
			ENDCG
		}
	}
}
