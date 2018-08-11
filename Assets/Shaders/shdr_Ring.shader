﻿Shader "Unlit/shdr_Ring"
{
	Properties
	{
		_MainTex ("Main", 2D) = "white" {}
		_MaskTex ("Mask", 2D) = "white" {}
		_EmissionColour("Emission", Color) = (0,0,0)
		
		_MainDirection("Main Direction", Vector) = (0,1,0)
		_MaskDirection("Mask Direction", Vector) = (0,1,0)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			float2 _MainDirection;
			float2 _MaskDirection;
			float3 _EmissionColour;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv + _Time.y * _MainDirection.xy);
				col *= tex2D(_MaskTex, i.uv + _Time.y * _MaskDirection.xy);
				col.rgb *= float3(1,1,1) + _EmissionColour.rgb; 

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
