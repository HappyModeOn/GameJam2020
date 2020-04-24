Shader "TechArt/ParallaxPanner"
{
	Properties
	{
		[Header(Base Color)]
        [NoScaleOffset] _MainTex ("Noise Texture (RGB)", 2D) = "white" {}
        _Col1 ("Color 1", Color) = (0, 0, 0, 0)
        _Col2 ("Color 2", Color) = (0, 0, 0, 0)
        _Emis ("Brightness", Float) = 1

        [Header(UV)]
        _TillingX ("Tilling X", Float) = 1
        _TillingY ("Tilling Y", Float) = 1
        _Panner ("Panner Controller", Vector) = (0, 0, 0, 0)
        _Depth ("Layer Depth", Vector) = (0, 0, 0, 0)

	}

	SubShader
	{
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        Cull Back
        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float3 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                half maskPos : TEXCOORD1;
                float2 worldUv : TEXCOORD2;
                fixed3 maskCol : COLOR0;
                float4 uvLayer1 : TEXCOORD3;
                float4 uvLayer2 : TEXCOORD4;
                fixed3 gradCol : COLOR1;
            };

            fixed _Emis;
            fixed4 _Col1, _Col2;;
            float _TillingX, _TillingY;
            float4 _Panner, _Depth;
            
            v2f vert(appdata v)
            {
                v2f o;
                float3x3 objectToTangent = float3x3(v.tangent.xyz, cross(v.normal, v.tangent.xyz) * v.tangent.w, v.normal);
                float3 viewDir = normalize(mul(objectToTangent, ObjSpaceViewDir(v.pos)));
                float2 uv = v.uv * float2(_TillingX, _TillingY);
                float4 uvParallax1 = uv.xyxy - viewDir.xyxy * _Depth.xxyy;
                float4 uvParallax2 = uv.xyxy - viewDir.xyxy * _Depth.zzww;
                float4 timePanner = _Time.y * _Panner;
                o.uvLayer1.xy = uvParallax1.xy + timePanner.xy;
                o.uvLayer1.zw = uvParallax1.zw + timePanner.zw;
                o.uvLayer2.xy = uvParallax2.xy + timePanner.xy;
                o.uvLayer2.zw = uvParallax2.zw + timePanner.zw;
                
                float3 worldPos = mul(unity_ObjectToWorld, v.pos).xyz;
                o.gradCol = v.uv.y * _Col1.rgb * _Emis + (1 - v.uv.y) * _Col2.rgb * _Emis;

                o.pos = UnityObjectToClipPos(v.pos);
                return o;
            }

            uniform float GLOBAL_maskRadius;
            fixed _MaskWidth, _MaskTurbulence;
            sampler2D _MainTex;
            

            fixed4 frag(v2f i) : SV_Target
            {

                ///Panner
                fixed4 noiseParallax = fixed4(tex2D(_MainTex, i.uvLayer1.xy).g,
                tex2D(_MainTex, i.uvLayer1.zw).b,
                tex2D(_MainTex, i.uvLayer2.xy).r,
                tex2D(_MainTex, i.uvLayer2.zw).b);

                fixed noiseComb = .25 * (noiseParallax.r + noiseParallax.g + noiseParallax.b + noiseParallax.a);
                noiseComb = saturate(smoothstep(0, .5, noiseComb));

                fixed2 noiseDetail1 = fixed2(noiseParallax.r + noiseParallax.g, noiseParallax.r / noiseParallax.g);
                fixed noiseDetail1Comb = saturate(lerp(noiseDetail1.x, noiseDetail1.y, noiseDetail1.y - noiseDetail1.x));
                fixed2 noiseDetail2 = fixed2(noiseParallax.b + noiseParallax.a, noiseParallax.b / noiseParallax.a);
                fixed noiseDetail2Comb = saturate(lerp(noiseDetail2.x, noiseDetail2.y, noiseDetail2.y - noiseDetail2.x));

                
                ///Color
                fixed4 col = 0;
                col.rgb = lerp(noiseDetail1Comb * _Col2.rgb, noiseDetail2Comb * _Col1.rgb, noiseComb) + i.gradCol;
                col.a = 1;

                return col;
            }
            ENDCG
        }
    }
}