Shader "Unlit/2DMetaBallsTexture"
{
	Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
         _MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		_WaterColor("Water Color", Color) = (1,1,1,1)
		_Cutoff("Alpha Cutoff", Float) = 0.5
		
    }
    SubShader
    {
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
	
		Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag
			
			
			uniform sampler2D _MainTex;
			uniform float _Cutoff;
			fixed4 _WaterColor;
			uniform Buffer<float2> _BufferX;
            // vertex shader inputs
			struct vertexInput {
				float4 vertex : POSITION;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};

            // vertex shader
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.worldPos = mul(_Object2World, input.vertex);
				// transformation of input.vertex from object 
				// coordinates to world coordinates;
				return output;
			}
            
           
            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (vertexOutput i) : SV_Target
            {
				//float4 textureColor = tex2D(_MainTex, i.uv);
				/*if (i.worldPos.x < _Cutoff) // alpha value less than user-specified threshold?					
				{
					discard; // yes: discard this fragment
				}*/
				if (_BufferX[2].x > i.worldPos.x)
					discard;
				return _WaterColor;
            }
            ENDCG
        }
    }
}
