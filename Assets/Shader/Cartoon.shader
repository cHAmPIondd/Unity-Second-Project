// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "MyShader/Cartoon"
{
	Properties
	{
		_MainColor("MainColor",Color)=(1,1,1,1)
		_MainTex("MainTex",2D)="white"{}
		_SpecularStrength("SpecularStrength",Range(0,1.0))=0.5
		_Gloss("Gloss",Range(8.0,256))=20
		_Steps("Steps",Range(2,8))=3
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		CGINCLUDE
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
				float4 texcoord:TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal:TEXCOORD0;
				float3 worldPos:TEXCOORD1;
				float2 uv:TEXCOORD2;
				SHADOW_COORDS(3)
			};

			float4 _MainColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _SpecularStrength;
			float _Gloss;
			float _Steps;
			v2f vert (a2v v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos=mul(unity_ObjectToWorld,v.vertex).xyz;
				o.worldNormal=UnityObjectToWorldNormal(v.normal);
				o.uv=TRANSFORM_TEX(v.texcoord,_MainTex);
				TRANSFER_SHADOW(o);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 worldNormal=normalize(i.worldNormal);
				fixed3 worldLightDir=normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 albedo =tex2D(_MainTex,i.uv)*_MainColor;
				fixed3 ambient =UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;
			//	fixed halfLambert=0.5*dot(worldNormal,worldLightDir)+0.5;
				fixed halfLambert=dot(worldNormal,worldLightDir);
				fixed3 diffuse=_LightColor0.rgb*halfLambert;

				diffuse=smoothstep(0,1,diffuse);

				diffuse=(floor(diffuse*_Steps)/_Steps)*albedo;

				fixed3 viewDir=normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 halfDir=normalize(worldLightDir+viewDir);
				fixed3 specular =_LightColor0.rgb*pow(max(0,dot(worldNormal,halfDir)),_Gloss);

				specular=lerp(0,1,smoothstep(0,0.01,specular))*_SpecularStrength;

				UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);

				return fixed4((diffuse+specular)*atten+ambient,1.0);
			}
			fixed4 frag0 (v2f i) : SV_Target
			{
				fixed3 worldNormal=normalize(i.worldNormal);
				fixed3 worldLightDir=normalize(UnityWorldSpaceLightDir(i.worldPos));

				fixed3 albedo =tex2D(_MainTex,i.uv)*_MainColor;
			//	fixed3 ambient =UNITY_LIGHTMODEL_AMBIENT.xyz;

			//	fixed halfLambert=0.5*dot(worldNormal,worldLightDir)+0.5;
					fixed halfLambert=dot(worldNormal,worldLightDir);
				fixed3 diffuse=_LightColor0.rgb*halfLambert;

				diffuse=smoothstep(0,1,diffuse);

				diffuse=(floor(diffuse*_Steps)/_Steps)*albedo;
				fixed3 viewDir=normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 halfDir=normalize(worldLightDir+viewDir);
				fixed3 specular =_LightColor0.rgb*pow(max(0,dot(worldNormal,halfDir)),_Gloss);

				specular=lerp(0,1,smoothstep(0,0.01,specular))*_SpecularStrength;

				UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);

				return fixed4((diffuse+specular)*atten,1.0);
			}
		ENDCG
			Pass
			{
				Tags{"LightMode"="ForwardBase"}
				Cull Back
				CGPROGRAM
				#pragma multi_compile_fwdbase
				#pragma vertex vert
				#pragma fragment frag
				ENDCG
			}
			Pass
			{
				Tags{"LightMode"="ForwardAdd"}
				Cull Back
				Blend One One
				CGPROGRAM
				#pragma multi_compile_fwdadd_fullshadows
				#pragma vertex vert
				#pragma fragment frag0
				ENDCG
			}

		}
		Fallback "VertexLit"
	}

