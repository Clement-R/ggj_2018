// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Default2"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_LineSize ("LineSize", Float) = 0.001
		_ScanLineSize ("ScanLineSize", Float) = 0.001
		_Move ("Move", Float) = 0.0005
		_Color1 ("Color1", Color) = (0,0,0,0)
		_Color2 ("Color2", Color) = (0,0,0,0)
		_Speed ("Speed", Float) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			float _LineSize;
			float _ScanLineSize;
			float _Move;
			float4 _Color1;
			float4 _Color2;
			float _Speed;
			float _Alpha;
			
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed timer = _Time.y*_Speed;
				fixed scanline = fmod(IN.texcoord.y+timer*_Speed, _LineSize*2);
				// just invert the colors
				//col.rgb = 1 - col.rgb;
				fixed scanline2 = fmod(IN.texcoord.y+_Time*_Speed, _ScanLineSize*2);
				fixed4 c = SampleSpriteTexture (IN.texcoord+fixed2((scanline<_LineSize?_Move:-_Move),0)) * IN.color;
				c.rgb *= c.a;
				return (scanline2<_ScanLineSize?c*_Color1:c*_Color2);
			}
		ENDCG
		}
	}
}