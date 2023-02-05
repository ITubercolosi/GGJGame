// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Earth"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_FresnelPower("FresnelPower", Float) = 0
		_FresnelBias("FresnelBias", Float) = 0
		_FresnelScale("FresnelScale", Float) = 0
		_FresnelStep("FresnelStep", Range( 0 , 1)) = 1
		_Color0("Color 0", Color) = (0,0,0,0)
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Texture("Texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _Color0;
		uniform float _FresnelStep;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform sampler2D _TextureSample1;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV236 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode236 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV236, _FresnelPower ) );
			float temp_output_3_0_g10 = ( _FresnelStep - fresnelNode236 );
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult255 = dot( ase_worldNormal , ase_worldlightDir );
			float2 temp_cast_0 = ((0.5 + (dotResult255 - 0.0) * (1.0 - 0.5) / (1.0 - 0.0))).xx;
			c.rgb = ( ( ( _Color0 * saturate( ( ( 1.0 - saturate( ( temp_output_3_0_g10 / fwidth( temp_output_3_0_g10 ) ) ) ) + fresnelNode236 ) ) ) + tex2D( _Texture, uv_Texture ) ) * tex2D( _TextureSample1, temp_cast_0 ) ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.CommentaryNode;234;1527.998,327.3366;Inherit;False;200;161;;1;233;WarpedUVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;148;-1133.547,-605.9623;Inherit;False;2251.393;1234.208;;39;29;49;34;30;35;33;43;42;45;44;48;46;28;47;50;27;36;37;40;39;41;135;136;137;140;143;32;132;144;142;145;147;26;138;133;127;128;130;139;Planet Curvature, Rotation and lighting;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-659.0126,-143.1392;Inherit;False;Property;_SpherizeStrengthY;SpherizeStrengthY;8;0;Create;True;0;0;0;False;0;False;0;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-488.0759,-49.80127;Inherit;False;Property;_RotationX;RotationX;40;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;34;-318.2237,-45.21201;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-487.0126,29.86118;Inherit;False;Property;_RotationY;RotationY;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;35;-317.9503,34.78428;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-118.2237,-22.21228;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-659.2484,-229.7877;Inherit;False;Property;_SpherizeStrengthX;SpherizeStrengthX;9;0;Create;True;0;0;0;False;0;False;0;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-431.2484,-198.7877;Inherit;False;FLOAT2;4;0;FLOAT;15;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-653.7573,-326.189;Inherit;False;Property;_PivotY;PivotY;7;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;44;-331.757,-344.189;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-330.3788,-442.7751;Inherit;False;2;2;0;FLOAT;-1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-330.3349,-542.393;Inherit;False;2;2;0;FLOAT;-1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-652.5214,-404.5406;Inherit;False;Property;_PivotX;PivotX;6;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-174.4135,-506.7607;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;50;-201.1318,-238.3766;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-138.1721,-351.5004;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;112.8575,-555.9623;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,-0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;37;352.0728,-555.7014;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;528.174,-473.5267;Inherit;False;Property;_RimStep;RimStep;42;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;39;687.0728,-554.7014;Inherit;True;Step Antialiasing;-1;;3;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;955.8453,-292.3589;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;135;-390.6588,159.1833;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LightAttenuation;136;-432.9648,288.8195;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;11.67044,208.4107;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;140;294.4669,59.60903;Inherit;True;Property;_LightGradient;LightGradient;11;0;Create;True;0;0;0;False;0;False;-1;None;b4825519fc2e4ee4a9b13dcb5efb0ae3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;701.2534,-11.44997;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;32;401.2059,-274.9829;Inherit;True;Property;_TextureSample0;Texture Sample 0;41;0;Create;True;0;0;0;False;0;False;-1;None;56576aa2ec3e72d4dab77866b32f3e73;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-171.5337,169.3768;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;133.8685,82.71475;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;142;888.6164,-42.79355;Inherit;False;SoftLight;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;145;682.6351,151.6691;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.1226415;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;147;389.8524,270.9185;Inherit;False;Property;_DarkestLight;DarkestLight;12;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;26;108.4755,-244.5383;Inherit;False;Spherize;-1;;4;1488bb72d8899174ba0601b595d32b07;0;4;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;127;-641.3532,338.3973;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;130;-1083.547,334.7773;Float;False;Property;_DefaultNormal;DefaultNormal;5;0;Create;True;0;0;0;False;0;False;0,0,1;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMaxOpNode;139;-281.669,391.7556;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-147.6647,1576.941;Inherit;False;Property;_TileScale;TileScale;13;0;Create;True;0;0;0;False;0;False;0;9.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-791.1735,1454.355;Inherit;False;Property;_NoiseTileY;NoiseTileY;14;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-792.1735,1364.355;Inherit;False;Property;_NoiseTileX;NoiseTileX;15;0;Create;True;0;0;0;False;0;False;0;-0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;161;120.4598,1270.512;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;167;-203.2358,1143.686;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;-510.0358,1304.885;Inherit;False;Property;_NoiseScale;NoiseScale;17;0;Create;True;0;0;0;False;0;False;1;1.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;151;-410.6123,1425.416;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;169;-466.45,1117.648;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;173;-833.7939,1138.856;Inherit;False;Property;_MovinNoiseTileX;MovinNoiseTileX;19;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;155;-623.1739,1400.355;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;175;-636.194,1163.557;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;174;-853.2938,1222.056;Inherit;False;Property;_MovinNoiseTileY;MovinNoiseTileY;18;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-22.43664,1829.934;Inherit;False;Property;_NoiseStep;Noise Step;16;0;Create;True;0;0;0;False;0;False;0;0.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;156;470.4189,1752.978;Inherit;False;Step Antialiasing;-1;;5;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;144.8746,1081.104;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;-123.021,1025.017;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;178;2.582654,933.9493;Inherit;False;Property;_DistortionPower;DistortionPower;21;0;Create;True;0;0;0;False;0;False;0;0.0025;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;180;-693.2415,999.098;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;163;-961.183,1057.635;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;149;99.93405,1509.712;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;219;3562.383,556.7927;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;213;3060.275,381.6786;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;202;2322.8,510.5294;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;217;2733.449,132.2666;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;216;3086.998,121.7132;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;218;2279.25,147.3657;Inherit;False;1;0;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;220;3123.968,722.0784;Inherit;False;Property;_PerlinStrength;PerlinStrength;25;0;Create;True;0;0;0;False;0;False;0;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;222;2663.476,691.9755;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;199;2540.29,968.6669;Inherit;False;Property;_BandsTiling;BandsTiling;23;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;212;2565.958,865.3749;Inherit;False;Property;_Seed;Seed;26;0;Create;True;0;0;0;False;0;False;0;34.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;2053.07,191.7122;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;224;2099.87,412.7118;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;1907.47,360.7118;Inherit;False;Property;_WobbleSpeed;WobbleSpeed;28;0;Create;True;0;0;0;False;0;False;1;0.025;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;185;2233.674,732.7022;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;188;2001.674,802.7021;Inherit;False;Property;_Offset;Offset;20;0;Create;True;0;0;0;False;0;False;0,0;0,17.76;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;187;2001.674,670.7021;Inherit;False;Property;_Tiling;Tiling;22;0;Create;True;0;0;0;False;0;False;0,0;1,17.46;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;206;3321.764,1269.193;Inherit;False;Property;_VoronoiStep;VoronoiStep;24;0;Create;True;0;0;0;False;0;False;0;0.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;227;3211.645,1083.358;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;229;3147.682,896.4551;Inherit;False;Property;_FadedNoiseStrength;FadedNoiseStrength;27;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;3414.185,908.1555;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;198;2838.901,960.8111;Inherit;True;0;0;1;0;1;False;1;False;True;False;4;0;FLOAT2;0,0;False;1;FLOAT;28.6;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;230;2581.271,1111.789;Inherit;False;Property;_VoronoiSmoothness;VoronoiSmoothness;30;0;Create;True;0;0;0;False;0;False;0;0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;221;3798.369,649.4647;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;231;1673.231,692.7331;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;241;4574.691,-53.92627;Inherit;False;Property;_Color0;Color 0;34;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.4242848,0.2113206,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;3460.36,1017.787;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;205;3776.217,1177.926;Inherit;False;Step Antialiasing;-1;;9;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;226;4451.486,918.8568;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;237;3958.677,408.6914;Inherit;False;Property;_FresnelPower;FresnelPower;29;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;238;3921.677,320.6914;Inherit;False;Property;_FresnelScale;FresnelScale;32;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;3932.677,230.6915;Inherit;False;Property;_FresnelBias;FresnelBias;31;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;236;4312.44,161.6171;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;243;4636.975,207.4561;Inherit;False;Step Antialiasing;-1;;10;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;245;4825.531,193.566;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;246;4999.409,272.506;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;5151.686,91.27377;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;242;5271.077,442.4494;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;247;5446.746,493.0862;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;250;5035.312,710.9935;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;128;-899.7361,338.7822;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;138;-427.2298,392.2712;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;254;5013.657,1168.563;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;133;-704.3049,445.2457;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;253;4814.226,1033.099;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;256;2714.647,488.1725;Inherit;False;Property;_PerlinScale;PerlinScale;37;0;Create;True;0;0;0;False;0;False;0;6.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;214;2649.659,263.1093;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;2852.647,400.1725;Inherit;False;2;2;0;FLOAT;2;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;4365.531,397.566;Inherit;False;Property;_FresnelStep;FresnelStep;33;0;Create;True;0;0;0;False;0;False;1;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;263;3856.072,1511.217;Inherit;False;Property;_PrimaryColour;PrimaryColour;36;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.9137256,0.7137255,0.2039215,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;4328.536,1088.611;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;264;3521.609,1357.878;Inherit;False;Property;_SecondaryColour;SecondaryColour;35;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.937255,0.8196079,0.4392156,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;266;4108.837,1275.814;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;262;4158.458,1463.742;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;267;5485.271,891.8574;Inherit;True;Property;_TextureSample1;Texture Sample 1;38;0;Create;True;0;0;0;False;0;False;-1;None;b4825519fc2e4ee4a9b13dcb5efb0ae3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;251;5864.273,923.7246;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;255;5177.749,1027.844;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;268;5313.155,927.3707;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;269;5923.168,1239.309;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;270;4990.819,157.5362;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;235;1888.362,559.9819;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RelayNode;233;1575.265,385.537;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;272;6171.752,438.7186;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;274;5566.44,579.0646;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6167.34,-143.5438;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Earth;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;0;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.PosVertexDataNode;273;5835.791,717.0531;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotateAboutAxisNode;271;5771.813,498.1696;Inherit;False;False;4;0;FLOAT3;0,1,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;275;4398.301,666.1056;Inherit;True;Property;_Texture;Texture;39;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;276;4033.901,581.4633;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
WireConnection;34;0;49;0
WireConnection;35;0;30;0
WireConnection;33;0;34;0
WireConnection;33;1;35;0
WireConnection;42;0;43;0
WireConnection;42;1;29;0
WireConnection;44;0;28;0
WireConnection;44;1;45;0
WireConnection;48;1;45;0
WireConnection;46;1;28;0
WireConnection;47;0;46;0
WireConnection;47;1;48;0
WireConnection;50;0;44;0
WireConnection;36;1;47;0
WireConnection;37;0;36;0
WireConnection;39;1;37;0
WireConnection;39;2;40;0
WireConnection;41;0;39;0
WireConnection;41;1;32;0
WireConnection;137;0;136;0
WireConnection;137;1;139;0
WireConnection;140;1;144;0
WireConnection;143;0;39;0
WireConnection;143;1;145;0
WireConnection;32;1;26;0
WireConnection;132;0;135;2
WireConnection;144;0;135;2
WireConnection;144;1;137;0
WireConnection;142;0;41;0
WireConnection;142;1;143;0
WireConnection;145;0;147;0
WireConnection;145;1;140;0
WireConnection;145;2;140;4
WireConnection;26;2;27;0
WireConnection;26;3;50;0
WireConnection;26;4;42;0
WireConnection;26;5;33;0
WireConnection;127;0;128;0
WireConnection;139;0;138;0
WireConnection;161;0;177;0
WireConnection;161;1;151;0
WireConnection;167;0;169;0
WireConnection;167;1;166;0
WireConnection;151;0;155;0
WireConnection;169;0;175;0
WireConnection;155;0;153;0
WireConnection;155;1;154;0
WireConnection;175;0;173;0
WireConnection;175;1;174;0
WireConnection;156;1;149;0
WireConnection;156;2;157;0
WireConnection;177;0;178;0
WireConnection;177;1;171;0
WireConnection;171;0;180;0
WireConnection;171;1;167;0
WireConnection;180;0;163;0
WireConnection;149;0;161;0
WireConnection;149;1;152;0
WireConnection;219;0;216;0
WireConnection;219;1;213;0
WireConnection;219;2;220;0
WireConnection;213;0;214;0
WireConnection;213;1;256;0
WireConnection;202;0;224;0
WireConnection;217;1;218;0
WireConnection;216;0;217;0
WireConnection;216;1;257;0
WireConnection;218;0;225;0
WireConnection;222;0;219;0
WireConnection;222;1;185;2
WireConnection;225;0;224;0
WireConnection;224;0;223;0
WireConnection;185;0;187;0
WireConnection;185;1;188;0
WireConnection;227;0;198;0
WireConnection;228;0;229;0
WireConnection;228;1;227;0
WireConnection;198;0;222;0
WireConnection;198;1;212;0
WireConnection;198;2;199;0
WireConnection;198;3;230;0
WireConnection;221;0;219;0
WireConnection;215;0;221;0
WireConnection;215;1;198;0
WireConnection;205;1;215;0
WireConnection;205;2;206;0
WireConnection;226;0;228;0
WireConnection;226;1;266;0
WireConnection;236;1;239;0
WireConnection;236;2;238;0
WireConnection;236;3;237;0
WireConnection;243;1;236;0
WireConnection;243;2;244;0
WireConnection;245;0;243;0
WireConnection;246;0;245;0
WireConnection;246;1;236;0
WireConnection;240;0;241;0
WireConnection;240;1;270;0
WireConnection;242;0;240;0
WireConnection;242;1;275;0
WireConnection;247;0;242;0
WireConnection;247;1;267;0
WireConnection;128;0;130;0
WireConnection;138;0;127;0
WireConnection;138;1;133;0
WireConnection;214;1;202;0
WireConnection;257;1;256;0
WireConnection;265;0;205;0
WireConnection;266;0;264;0
WireConnection;266;1;263;0
WireConnection;266;2;205;0
WireConnection;262;1;263;0
WireConnection;267;1;268;0
WireConnection;255;0;253;0
WireConnection;255;1;254;0
WireConnection;268;0;255;0
WireConnection;270;0;246;0
WireConnection;235;0;233;0
WireConnection;233;0;26;0
WireConnection;272;0;271;0
WireConnection;272;1;273;0
WireConnection;0;13;247;0
WireConnection;271;1;274;0
WireConnection;271;3;273;0
ASEEND*/
//CHKSM=1A63DDC457B5FAB2742343DE87F7127067B4B971