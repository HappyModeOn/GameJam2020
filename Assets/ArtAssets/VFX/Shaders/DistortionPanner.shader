///Vertex Color Controll : R = fade amount, G and B = Noise offset, A = distort amount

Shader "TechArt/DistortionPanner"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Mask Texture (RGB)", 2D) = "white" {} ///R = Alpha Mask || G, B = Noise Panner
        _DistortPanner ("Panner", Vector) = (0, 0, 0, 0)
        _DistortTilling ("Tilling", Float) = 1
        _DistortStr ("Strength", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
        Cull Back
        Zwrite Off
        ZTest Always
        //Blend SrcAlpha OneMinusSrcAlpha
        Blend SrcAlpha One
        Lighting Off

        GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
                float4 panner : TEXCOORD2;
                fixed2 distort : COLOR0;
            };

            sampler2D _BackgroundTexture;
            fixed _DistortStr, _DistortTilling;
            fixed4 _DistortPanner;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.distort = fixed2(v.color.a * _DistortStr, v.color.r);
                o.panner = v.uv.xyxy * _DistortTilling + (_Time.y * _DistortPanner + v.color.gbbg);
                o.pos = UnityObjectToClipPos(v.pos);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed texMask = tex2D(_MainTex, i.uv).r * i.distort.y;
                float2 texPanner = float2(tex2D(_MainTex, i.panner.xy).g, tex2D(_MainTex, i.panner.zw).b) * i.distort.x;
                fixed4 col = tex2Dproj(_BackgroundTexture, i.grabPos + float4(texPanner, 0, 0));
                col.a = tex2D(_MainTex, i.uv).r * i.distort.y;
                return col;
            }
            ENDCG
        }
    }
}
