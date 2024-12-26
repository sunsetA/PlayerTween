// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "QQ/Wave"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Anchor("x'anchor",float) = 0.5
		_Speed("speed",float) = 1.0
		_Length("length",float) = 1
		_Height("height",float) = 1
		_Color("Color",Color)=(1,1,1,1)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				struct a2v
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Anchor;
				float _Speed;
				float _Length;
				float _Height;
				fixed4 _Color;
				v2f vert(a2v v)
				{
					v2f o;
					float a = min(1.0,abs(v.vertex.x - _Anchor) * 0.3);
					if(v.vertex.x<0.4)
					{
						v.vertex.y += a * sin(_Time.y * _Speed + v.vertex.x*_Length)*_Height;
					}

					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv)*_Color;
					return col;
				}
				ENDCG
			}
		}
}