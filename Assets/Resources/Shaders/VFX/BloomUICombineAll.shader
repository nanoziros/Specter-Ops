// Amplify Bloom - Advanced Bloom Post-Effect for Unity
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

Shader "Hidden/BloomUICombineAll"
{
	Properties
	{
		_MainTex ( "Texture", 2D ) = "white" {}
		_GameTex ( "Texture", 2D ) = "white" {}
	}
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img_custom
			#pragma fragment frag

			#include "UnityCG.cginc"

				uniform float4	_MainTex_TexelSize;

				struct v2f_img_custom
				{
					float4 pos : SV_POSITION;
					float2 uv   : TEXCOORD0;
			#if UNITY_UV_STARTS_AT_TOP
					float4 uv2 : TEXCOORD1;
			#endif
				};

				v2f_img_custom vert_img_custom ( appdata_img v )
				{
					v2f_img_custom o;

					o.pos = mul ( UNITY_MATRIX_MVP, v.vertex );
					o.uv = float4( v.texcoord.xy, 1, 1 );

			#ifdef UNITY_HALF_TEXEL_OFFSET
					o.uv.y += _MainTex_TexelSize.y;
			#endif

			#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
			#endif
					return o;
				}

				uniform sampler2D _MainTex;
				uniform sampler2D _GameTex;
		
				float4 frag ( v2f_img_custom i ) : SV_Target
				{
					return tex2D ( _GameTex, i.uv ) + tex2D ( _MainTex, i.uv );
				}
			ENDCG
		}
	}
}
