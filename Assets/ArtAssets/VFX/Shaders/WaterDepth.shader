Shader "TechArt/WaterDepth"
{
	Properties
	{
		[NoScaleOffset] _MainTex ("Noise Texture(RGBA)", 2D) = "white" {}

		[Header(Color)]
		_ColorSurface("Surface", Color) = (1, 1, 1, 1)
		_ColorDepth("Depth", Color) = (0, 0, 0, 1)

		[Header(Depth)]
		_DepthRange("Range", Float) = 0
		_DepthBlend("Blend", Float) = 1
		
		[Header(Shore)]
		_ShoreWidth("Width", Float) = 1
		_ShoreBrightness("Brightness", Float) = 1

		[Header(Foam)]
		_FoamAmount("Amount", Range(0, 1)) = .5
		_FoamTilling("Tilling", Float) = 1
		_FoamRange("Range", Float) = 0
		_FoamBlend("Blend", Float) = 1
		_FoamTurbulence("Turbulence", Float) = 0
		_FoamSurfOpacity("Surface Opacity", Float) = 1
		_FoamShoreBrightness("Shore Brightness", Float) = 1

		[Header(Gross)]
		_GrossAmount("Amount", Range(0, 1)) = .5
		_GrossPanner("Panner", Vector) = (0, 0, 0, 0)
		_GrossTilling("Tilling", Float) = 1
		_GrossBrightness("Brightness", Float) = 1

		[Header(Wave)]
		_WaveTilling("Tilling", Float) = 1
		_WaveSpeed("Speed", Float) = 1
		_WaveHeight("Height", Float) = 0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		Lighting Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex     : POSITION;
				float2 uv         : TEXCOORD0;
			};

			struct v2f
			{
				float4 uv         : TEXCOORD0; //uv tilling for gross and foam
				float4 vertex     : SV_POSITION;
				float4 scrPos     : TEXCOORD1;
				float4 uvPanner   : TEXCOORD2;
				float2 wavePanner : TEXCOORD3;
			};

			uniform sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float _WaveSpeed;
			float _WaveTilling;
			float _WaveHeight;
			float4 _GrossPanner;
			float _GrossTilling;
			float _FoamTilling;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.wavePanner = v.uv * _WaveTilling + _Time.y * _WaveSpeed;
				float4 WaveTex = tex2Dlod(_MainTex, float4(o.wavePanner * .5, 0, 0));
				v.vertex.y += WaveTex * _WaveHeight;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = float4(v.uv * _GrossTilling, v.uv * _FoamTilling);
				float4 timePanner = _Time.y * _GrossPanner;
				o.uvPanner = float4(o.uv.xy * .25 + timePanner.xy, o.uv.xy * .5 + timePanner.zw);

				o.scrPos = ComputeScreenPos(o.vertex);
				return o;
			}

			float _DepthRange;
			float _DepthBlend;
			fixed4 _ColorDepth;
			fixed4 _ColorSurface;
			float _FoamAmount;
			float _FoamRange;
			float _FoamBlend;
			float _FoamSurfOpacity;
			float _FoamTurbulence;
			float _FoamShoreBrightness;
			float _GrossAmount;
			float _GrossBrightness;
			float _ShoreWidth;
			float _ShoreBrightness;
			
			fixed4 frag (v2f i) : SV_Target
			{
				half depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
				depth = saturate(1 - (depth - i.scrPos.w));
				//fixed depthCtrl = smoothstep(_DepthRange, _DepthRange + _DepthBlend, depth);
				fixed depthCtrl = pow(depth + _DepthRange, _DepthBlend);

				fixed foamTex = tex2D(_MainTex, i.uv.zw + tex2D(_MainTex, i.wavePanner).a * _FoamTurbulence).b;
				fixed foamSurface = foamTex * _FoamSurfOpacity;
				fixed foamDepth = smoothstep(_FoamRange, _FoamRange + _FoamBlend, depth);
				fixed foamShore = step(_FoamAmount, foamTex) * _FoamShoreBrightness * foamDepth;

				fixed gross = tex2D(_MainTex, i.uv.xy).r * tex2D(_MainTex, i.uvPanner.xy).r * tex2D(_MainTex, i.uvPanner.zw).g;
				gross = step(1 - gross, _GrossAmount * .5 + .5) * _GrossBrightness;

				fixed shore = saturate(step(0, depth - (1 - _ShoreWidth)) + foamShore) * _ShoreBrightness;

				fixed4 waterCol = lerp(_ColorDepth, _ColorSurface, depthCtrl);
				fixed4 col = fixed4(waterCol.rgb + shore + gross + foamSurface, waterCol.a);
				return col;
			}
			ENDCG
		}
	}
}
