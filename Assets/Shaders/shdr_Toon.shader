﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "LD/Toon" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_OutlineDist ("Outline Distance", Float) = 1

		_Emission ("Emission", Color) = (0,0,0,1)
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass
		{
			ZWrite Off
			ColorMask 0
		}

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		
		float _OutlineDist;

		struct Input 
		{
			float v;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			v.vertex.xyz += v.normal.xyz * 0.05f * _OutlineDist;
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = half3(0,0,0);
			o.Alpha = 1;
		}
		ENDCG
	} 

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf ToonRamp vertex:vert

		sampler2D _Ramp;

		// custom lighting function that uses a texture ramp based
		// on angle between light direction and normal
		#pragma lighting ToonRamp exclude_path:prepass
		inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif
	
			half d = dot (s.Normal, lightDir)*0.5 + 0.5;
			half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = 0;

			return c;
		}


		sampler2D _MainTex;
		float4 _Color;
		half3 _Emission;

		struct Input 
		{
			float2 uv_MainTex : TEXCOORD0;
			float3 screenNormal;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.screenNormal = UnityObjectToClipPos(v.normal);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;

			o.Alpha = c.a;
			o.Emission = _Emission;
		}
		ENDCG
	} 

	Fallback "Diffuse"
}
