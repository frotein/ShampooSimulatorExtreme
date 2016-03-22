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
	float _TestX0;
	float _TestY0;
	float _TestX1;
	float _TestY1;
	
	float GetIntensity(float2 start, float2 end, float radius)
	{
		return (radius / (((start.x - end.x) * (start.x - end.x)) + ((start.y - end.y) * (start.y - end.y)) + 0.000001));	
	}

	float totalIntensity()
	{
		float widthF = _width;
		for (int i = 0; i < _width; i++)
		{
			float iFloat = i;
			float2 uv = float2(iFloat / widthF, 0.5);
			fixed4 col = tex2D(_PositionsTex, uv);
		}

		return 0.0;
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
				
				float intensity = totalIntensity();
				// just set color
				
				if(i.vertex.x > _TestX0)
				col = _Color;

				return col;
			}
			ENDCG
		}
	}
}
