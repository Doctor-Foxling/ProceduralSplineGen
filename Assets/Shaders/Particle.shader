Shader "Unlit/Particle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Size("Point Size", Float) = 0.1
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _Size;

            struct appdata
            {
                float4 vertex : POSITION;
				fixed3 color : COLOR0;
            };

            struct v2g
            {
				float4 vertex : SV_POSITION;
				float3 color : COLOR0;
            };

			struct g2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 color : COLOR0;
			};


            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color;

			v2g vert (appdata v)
            {
				v2g o;
                o.vertex = v.vertex;
				o.color = v.color;
                return o;
            }

			[maxvertexcount(4)]
			void geom(point v2g input[1], inout TriangleStream<g2f> tristream) {
				g2f gOut;
				gOut.color = input[0].color;

				float4 positionModel = mul(UNITY_MATRIX_M, input[0].vertex);
				float4 positionView = mul(UNITY_MATRIX_V, positionModel);

				gOut.vertex = mul(UNITY_MATRIX_P, positionView + float4(-0.5f, -0.5f, 0.0f, 0.0f) * _Size);
				gOut.uv = float2(0, 0);
				tristream.Append(gOut);

				gOut.vertex = mul(UNITY_MATRIX_P, positionView + float4(-0.5f, 0.5f, 0.0f, 0.0f) * _Size);
				gOut.uv = float2(0, 1);
				tristream.Append(gOut);

				gOut.vertex = mul(UNITY_MATRIX_P, positionView + float4(0.5f, -0.5f, 0.0f, 0.0f) * _Size);
				gOut.uv = float2(1, 0);
				tristream.Append(gOut);

				gOut.vertex = mul(UNITY_MATRIX_P, positionView + float4(0.5f, 0.5f, 0.0f, 0.0f) * _Size);
				gOut.uv = float2(1, 1);
				tristream.Append(gOut);
			}

            fixed4 frag (g2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * float4(i.color, 1) * _Color;
				if (col.a < 0.5) discard; 
                // apply fog
                return col;
            }
            ENDCG
        }
    }
}
