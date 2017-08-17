// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "MyShader/XOcculsionSprite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaxX ("MaxX", Float) = 0
		_MinX ("MinX", Float) = 0
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
		LOD 100
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			float _MaxX;
			float _MinX;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 worldPos:TEXCOORD1;
				float4 pos : SV_POSITION;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos=mul(UNITY_MATRIX_MVP,v.vertex);
				float4 tempPos=mul(unity_ObjectToWorld,v.vertex);
				o.worldPos.x=tempPos.x/tempPos.w;
				o.worldPos.y=tempPos.y/tempPos.w;
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float x=i.worldPos.x;
				float y=i.worldPos.y;
				clip(_MaxX-x);
				clip(x-_MinX);
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb*=col.a;
				return col;
			}
			ENDCG
		}
	}
}
