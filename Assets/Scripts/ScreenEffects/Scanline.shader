Shader "Custom/Scanline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LineSize ("LineSize", Float) = 0.001
		_ScanLineSize ("ScanLineSize", Float) = 0.001
		_Move ("Move", Float) = 0.0005
		_Color1 ("Color1", Color) = (0,0,0,0)
		_Color2 ("Color2", Color) = (0,0,0,0)
		_Speed ("Speed", Float) = 1
		_Alpha ("Alpha", Float) = 1
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
			float _LineSize;
			float _ScanLineSize;
			float _Move;
			float4 _Color1;
			float4 _Color2;
			float _Speed;
			float _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed timer = _Time.y*_Speed;
				fixed scanline = fmod(i.uv.y+timer*_Speed, _LineSize*2);
				fixed4 col = tex2D(_MainTex, i.uv+_Alpha*fixed2((scanline<_LineSize?_Move:-_Move),0));
				// just invert the colors
				//col.rgb = 1 - col.rgb;
				fixed scanline2 = fmod(i.uv.y+_Time*_Speed, _ScanLineSize*2);
				return _Alpha*(scanline2<_ScanLineSize?col-_Color1:col-_Color2)+(1-_Alpha)*col;
			}
			ENDCG
		}
	}
}
