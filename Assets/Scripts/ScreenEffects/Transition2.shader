﻿Shader "Custom/Transition2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color1 ("Color1", Color) = (0,0,0,0)
		_Color2 ("Color2", Color) = (0,0,0,0)
		_Color3 ("Color3", Color) = (0,0,0,0)
		_TimeBegin ("TimeBegin", Float) = 0
		_BandLength ("BandLength", Float) = 0.1
		_Duration ("Duration", Float) = 1
		_RealTime ("RealTime", Float) = 0
		_Selector ("Selector", Float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}


			
			sampler2D _MainTex;
			float4 _Color1;
			float4 _Color2;
			float4 _Color3;
			float _TimeBegin;
			float _BandLength;
			float _Duration;
			float _RealTime;
			float _Selector;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors

				fixed prog = _RealTime - _TimeBegin;
				prog = clamp(prog/_Duration,0,1);
				fixed2 centered = i.uv - fixed2(0.5,0.5);
				//centered.x += _Time.y;
				centered.x = centered.x * (_ScreenParams.x/_ScreenParams.y);
				
				centered = fmod(centered, 0.3);					
					
				fixed factor = sin(prog*3.14);
				fixed dist = centered.x + _Time.y;
				//col.rgb = (factor * (1-col.rgb))+((1-factor)*col.rgb);
				fixed mod = fmod(abs(dist),_BandLength*3);
				if(abs(centered.x)/(_BandLength*3)>4){
					factor = 0;
				}
				fixed3 effect = mod < _BandLength ? _Color1 : mod < _BandLength*2 ? _Color2 : _Color3;
				col.rgb = (1-factor) * col.rgb + (factor) * effect;
				return col;
			}
			ENDCG
		}
	}
}
