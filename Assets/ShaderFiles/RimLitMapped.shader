Shader "Custom/RimLitCelShadePatternMapped" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_RimColor ("RimColor", Color) = (1,1,1,1)
		_Outline("Outline width", Range(.002, 0.03)) = .005
		_RimPower("Rim Power", Range(1,20)) = 5

		_MainTex ("Black Texture (RGB)", 2D) = "white" {}
		_MapTex("Map Data Texture", 2D) = "black" {}
		_RMapTex("Red Map Albedo", 2D) = "white" {}
		_GMapTex("Green Map Albedo", 2D) = "white" {}
		_BMapTex("Blue Map Albedo", 2D) = "white" {}

		_ShadePattern("Shade Pattern", 2D) = "black" {}
		_ShadePatternAmount("Shade Pattern Amount", Range(1, 256)) = 32
		_ShadeColor("Shade Color", Color) = (1,1,1,1)
		_Ramp ("Ramp", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200	

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Ramp fullforwardshadows

		// Use shader model 4.0 target, to get nicer looking lighting
		#pragma target 4.0

		sampler2D _MainTex;
		sampler2D _Ramp;
		sampler2D _ShadePattern;

		sampler2D _RMapTex;
		sampler2D _GMapTex;
		sampler2D _BMapTex;
		sampler2D _MapTex;

		fixed4 _RimColor;
		fixed4 _ShadeColor;

		uniform float _Outline,_RimPower,_ShadePatternAmount;

		struct Input {
			float2 uv_MainTex;

			float2 uv_MapTex;
			float2 uv_RMapTex;
			float2 uv_GMapTex;
			float2 uv_BMapTex;

			fixed4 screenPos;
			float3 viewDir;
		};

		struct SurfaceOutputCustom {
			half3 Albedo;
			half3 Normal;

			half3 Emission;
			half Alpha;

			half3 scrPos;
		};

		
		half4 LightingRamp(SurfaceOutputCustom s, half3 lightDir, fixed3 viewDir, half atten) {
			lightDir = normalize(lightDir);
			viewDir = normalize(viewDir);

			half NdotL = dot(s.Normal, lightDir);
			half diff = NdotL * 0.5 + 0.5;

			float2 rampPos = float2(diff, diff);

			float3 lightDirPerp = normalize(lightDir - dot(lightDir, viewDir) * viewDir);

			fixed3 rim = atten * pow ( max (0, dot ( lightDirPerp, s.Normal ) ), _RimPower ) * _RimColor.rgb * _RimColor.a;

			//rim = rim > 0.5 ? 1 : 0;

			half3 ramp = tex2D(_Ramp, rampPos).rgb;

			half3 shadeLevel = tex2D(_ShadePattern, s.scrPos.xy * _ShadePatternAmount).rgb * (1 + NdotL);

			half4 c;
			c.rgb = ( s.Albedo * _LightColor0.rgb * max (ramp , max(shadeLevel, (1 - shadeLevel) * 0.5) * NdotL * atten * 1.25 * _ShadeColor)  * ( atten * 2) ) + (rim * atten);
			c.a = s.Alpha;
			return c;
		}

		
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputCustom o) {

			fixed4 c;
			fixed4 texCol = tex2D(_MapTex, IN.uv_MapTex);
			
			o.scrPos = IN.screenPos.xyz / (IN.screenPos.w == 0 ? 1 : IN.screenPos.w);

			fixed4 rmt = tex2D(_RMapTex, IN.uv_RMapTex);
			fixed4 gmt = tex2D(_GMapTex, IN.uv_GMapTex);
			fixed4 bmt = tex2D(_BMapTex, IN.uv_BMapTex);

			c = texCol.r * rmt + texCol.g * gmt + texCol.b * bmt;

			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			fixed3 rimCol = fixed3(1,1,1);
			
			o.Albedo = c.rgb * _Color + -1 * pow(rimCol * rim * 1, 3);
			
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
