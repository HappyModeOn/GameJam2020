// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TechArt/Terrain"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Tessellation("Tessellation", Float) = 1
		_Height("Height", Float) = 0
		_Color1("Color 1", Color) = (0,0,0,0)
		_Color0("Color 0", Color) = (0,0,0,0)
		_Speed("Speed", Float) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture;
		uniform float _Speed;
		uniform float _Height;
		uniform float4 _Color0;
		uniform float4 _Color1;
		uniform float _Tessellation;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_0 = (_Tessellation).xxxx;
			return temp_cast_0;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float smoothstepResult5 = smoothstep( 0.45 , 0.6 , v.texcoord.xy.y);
			float2 appendResult21 = (float2(_Speed , 0.0));
			float2 panner17 = ( 1.0 * _Time.y * appendResult21 + v.texcoord.xy);
			float smoothstepResult7 = smoothstep( -0.15 , 0.48 , tex2Dlod( _Texture, float4( panner17, 0, 0.0) ).b);
			float temp_output_6_0 = ( smoothstepResult5 * smoothstepResult7 );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( temp_output_6_0 * ase_vertexNormal ) * _Height );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float smoothstepResult5 = smoothstep( 0.45 , 0.6 , i.uv_texcoord.y);
			float2 appendResult21 = (float2(_Speed , 0.0));
			float2 panner17 = ( 1.0 * _Time.y * appendResult21 + i.uv_texcoord);
			float smoothstepResult7 = smoothstep( -0.15 , 0.48 , tex2D( _Texture, panner17 ).b);
			float temp_output_6_0 = ( smoothstepResult5 * smoothstepResult7 );
			float4 lerpResult13 = lerp( _Color0 , _Color1 , temp_output_6_0);
			o.Emission = lerpResult13.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
1366;-305;1527;652;1469.314;36.42361;1.012051;True;False
Node;AmplifyShaderEditor.RangedFloatNode;20;-1919.773,142.7577;Inherit;False;Property;_Speed;Speed;6;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1790.393,-105.4921;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;21;-1506.773,199.7577;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;17;-1370.82,-5.435332;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1040.218,8.073394;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;False;0;-1;None;f89a04054167a1b48aec0dd552d1074e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-653.5,-277.5;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;5;-403.9072,-230.2673;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.45;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;7;-594.8154,29.05791;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.15;False;2;FLOAT;0.48;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;9;-1093.442,442.2756;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-189.5,47.5;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-98.5822,-414.3219;Inherit;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;0,0,0,0;0.3584905,0.2113741,0.2113741,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-95.64591,-205.8453;Inherit;False;Property;_Color1;Color 1;3;0;Create;True;0;0;False;0;0,0,0,0;0.5655065,0.9622641,0.3222677,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-764.4415,329.2756;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-860.4415,522.2756;Float;False;Property;_Height;Height;2;0;Create;True;0;0;False;0;0;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-293.5,473.5;Inherit;False;Property;_Tessellation;Tessellation;1;0;Create;True;0;0;False;0;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-559.4415,349.2756;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;13;443.1633,-182.355;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;661.6761,-16.06371;Float;False;True;-1;6;ASEMaterialInspector;0;0;Unlit;TechArt/Terrain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;20;0
WireConnection;17;0;18;0
WireConnection;17;2;21;0
WireConnection;1;1;17;0
WireConnection;5;0;3;2
WireConnection;7;0;1;3
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;10;0;6;0
WireConnection;10;1;9;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;13;0;14;0
WireConnection;13;1;15;0
WireConnection;13;2;6;0
WireConnection;0;2;13;0
WireConnection;0;11;12;0
WireConnection;0;14;2;0
ASEEND*/
//CHKSM=945065DFDF0E8E4398DBC4C299FDB6887B277DB4