﻿// Toony Colors Pro+Mobile 2
// (c) 2014-2023 Jean Moreno

Shader "Toony Colors Pro 2/User/My TCP2 ShaderTextured with Opacity"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[MainTexture] _BaseMap ("Albedo", 2D) = "white" {}
		_Alpha ("Alpha", Float) = 1
		 [NoScaleOffset] _Alpha1 ("Alpha Texture", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		
		[TCP2HeaderHelp(Main Directional Light)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCount ("Bands Count", Range(1,20)) = 4
		[TCP2HeaderHelp(Other Lights)]
		_RampThresholdOtherLights ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothingOtherLights ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCountOtherLights ("Bands Count", Range(1,20)) = 4
		[Space]
		_LightWrapFactor ("Light Wrap Factor", Range(0,2)) = 0.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[TCP2ColorNoAlpha] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularSmoothness ("Smoothness", Float) = 0.2
		[TCP2Separator]

		[TCP2HeaderHelp(Emission)]
		[TCP2ColorNoAlpha] [HDR] _Emission ("Emission Color", Color) = (0,0,0,1)
		 [NoScaleOffset] _Emission1 ("Emission Texture", 2D) = "white" {}
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1
		[TCP2Separator]

		[TCP2HeaderHelp(Reflections)]
		[Toggle(TCP2_REFLECTIONS)] _UseReflections ("Enable Reflections", Float) = 0
		[TCP2ColorNoAlpha] _ReflectionColor ("Color", Color) = (1,1,1,1)
		_PlanarNormalsInfluence ("Reflection Normal Influence", Range(0,1)) = 0.1
		[HideInInspector] _ReflectionTex ("Planar Reflection RenderTexture", 2D) = "white" {}
		_FresnelMin ("Fresnel Min", Range(0,2)) = 0
		_FresnelMax ("Fresnel Max", Range(0,2)) = 1.5
		[TCP2Separator]
		
		[TCP2HeaderHelp(Normal Mapping)]
		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
		_BumpScale ("Scale", Float) = 1
		[TCP2Separator]
		
		[Enum(ToonyColorsPro.ShaderGenerator.BlendOperation)] _blendOperation ("Blend Operation", Float) = 0

		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1

		// Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"IgnoreProjectors"="True"
		}

		HLSLINCLUDE
		#define fixed half
		#define fixed2 half2
		#define fixed3 half3
		#define fixed4 half4

		#if UNITY_VERSION >= 202020
			#define URP_10_OR_NEWER
		#endif
		#if UNITY_VERSION >= 202120
			#define URP_12_OR_NEWER
		#endif
		#if UNITY_VERSION >= 202220
			#define URP_14_OR_NEWER
		#endif

		// Texture/Sampler abstraction
		#define TCP2_TEX2D_WITH_SAMPLER(tex)						TEXTURE2D(tex); SAMPLER(sampler##tex)
		#define TCP2_TEX2D_NO_SAMPLER(tex)							TEXTURE2D(tex)
		#define TCP2_TEX2D_SAMPLE(tex, samplertex, coord)			SAMPLE_TEXTURE2D(tex, sampler##samplertex, coord)
		#define TCP2_TEX2D_SAMPLE_LOD(tex, samplertex, coord, lod)	SAMPLE_TEXTURE2D_LOD(tex, sampler##samplertex, coord, lod)

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		// Uniforms

		// Shader Properties
		TCP2_TEX2D_WITH_SAMPLER(_BumpMap);
		TCP2_TEX2D_WITH_SAMPLER(_BaseMap);
		TCP2_TEX2D_WITH_SAMPLER(_Alpha1);
		TCP2_TEX2D_WITH_SAMPLER(_Emission1);
		sampler2D _ReflectionTex;

		CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float _BumpScale;
			float4 _BaseMap_ST;
			float _Alpha;
			fixed4 _Color;
			half4 _Emission;
			float _LightWrapFactor;
			float _RampThreshold;
			float _RampSmoothing;
			float _BandsCount;
			float _RimMin;
			float _RimMax;
			fixed4 _RimColor;
			float _SpecularSmoothness;
			fixed4 _SpecularColor;
			float _RampThresholdOtherLights;
			float _RampSmoothingOtherLights;
			float _BandsCountOtherLights;
			fixed4 _SColor;
			fixed4 _HColor;
			float _PlanarNormalsInfluence;
			float _FresnelMin;
			float _FresnelMax;
			fixed4 _ReflectionColor;
		CBUFFER_END

		static const float DITHER_THRESHOLD_8x8[64] =
		{
			0.2627,0.7725,0.3843,0.9529,0.2941,0.8196,0.4118,1.0000,
			0.5020,0.0392,0.6235,0.1412,0.5333,0.0510,0.6549,0.1725,
			0.3216,0.8627,0.2039,0.6824,0.3529,0.9098,0.2314,0.7294,
			0.5647,0.0824,0.4431,0.0078,0.5922,0.1137,0.4745,0.0235,
			0.2784,0.7961,0.4000,0.9765,0.2471,0.7529,0.3686,0.9333,
			0.5176,0.0471,0.6392,0.1569,0.4902,0.0314,0.6078,0.1294,
			0.3373,0.8863,0.2196,0.7059,0.3098,0.8431,0.1882,0.6706,
			0.5804,0.0980,0.4588,0.0157,0.5490,0.0667,0.4275,0.0001
		};
		float Dither8x8(float2 positionCS)
		{
			uint index = (uint(positionCS.x) % 8) * 8 + uint(positionCS.y) % 8;
			return DITHER_THRESHOLD_8x8[index];
		}
		
		static const float DITHER_THRESHOLDS_4x4[16] =
		{
			0.0588, 0.5294, 0.1765, 0.6471,
			0.7647, 0.2941, 0.8823, 0.4118,
			0.2353, 0.7059, 0.1176, 0.5882,
			0.9412, 0.4706, 0.8235, 0.3529
		};
		float Dither4x4(float2 positionCS)
		{
			uint index = (uint(positionCS.x) % 4) * 4 + uint(positionCS.y) % 4;
			return DITHER_THRESHOLDS_4x4[index];
		}
		
		//Specular help functions (from UnityStandardBRDF.cginc)
		inline float3 SpecSafeNormalize(float3 inVec)
		{
			half dp3 = max(0.001f, dot(inVec, inVec));
			return inVec * rsqrt(dp3);
		}
		
		// Built-in renderer (CG) to SRP (HLSL) bindings
		#define UnityObjectToClipPos TransformObjectToHClip
		#define _WorldSpaceLightPos0 _MainLightPosition
		
		ENDHLSL

		Pass
		{
			Name "Main"
			Tags
			{
				"LightMode"="UniversalForward"
			}
			Blend SrcAlpha OneMinusSrcAlpha
			BlendOp [_blendOperation]
			AlphaToMask On

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard SRP library
			// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 3.0

			// -------------------------------------
			// Material keywords
			#pragma shader_feature_local _ _RECEIVE_SHADOWS_OFF

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile _ _FORWARD_PLUS

			// -------------------------------------
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex Vertex
			#pragma fragment Fragment

			//--------------------------------------
			// Toony Colors Pro 2 keywords
			#pragma shader_feature_local_fragment TCP2_RIM_LIGHTING
			#pragma shader_feature_local_fragment TCP2_REFLECTIONS

			// vertex input
			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
				#if defined(LIGHTMAP_ON)
				float2 texcoord1    : TEXCOORD1;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
				float2 texcoord2    : TEXCOORD2;
				#endif
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			// vertex output / fragment input
			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
				float4 worldPosAndFog : TEXCOORD0;
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord    : TEXCOORD1; // compute shadow coord per-vertex for the main light
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				half3 vertexLights : TEXCOORD2;
			#endif
			#if defined(LIGHTMAP_ON) && defined(DYNAMICLIGHTMAP_ON)
				float4 lightmapUV  : TEXCOORD3;
				#define staticLightmapUV lightmapUV.xy
				#define dynamicLightmapUV lightmapUV.zw
			#elif defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
				float2 lightmapUV  : TEXCOORD3;
				#define staticLightmapUV lightmapUV.xy
				#define dynamicLightmapUV lightmapUV.xy
			#endif
				float4 screenPosition : TEXCOORD4;
				float3 pack1 : TEXCOORD5; /* pack1.xyz = tangent */
				float3 pack2 : TEXCOORD6; /* pack2.xyz = bitangent */
				float2 pack3 : TEXCOORD7; /* pack3.xy = texcoord0 */
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if USE_FORWARD_PLUS
				// Fake InputData struct needed for Forward+ macro
				struct InputDataForwardPlusDummy
				{
					float3  positionWS;
					float2  normalizedScreenSpaceUV;
				};
			#endif

			Varyings Vertex(Attributes input)
			{
				Varyings output = (Varyings)0;

				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				// Texture Coordinates
				output.pack3.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				#if defined(LIGHTMAP_ON)
					output.staticLightmapUV = input.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
					output.dynamicLightmapUV = input.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif

				float3 worldPos = mul(UNITY_MATRIX_M, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
			#endif
				float4 clipPos = vertexInput.positionCS;

				float4 screenPos = ComputeScreenPos(clipPos);
				output.screenPosition.xyzw = screenPos;

				VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(input.normal, input.tangent);
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				// Vertex lighting
				output.vertexLights = VertexLighting(vertexInput.positionWS, vertexNormalInput.normalWS);
			#endif

				// world position
				output.worldPosAndFog = float4(vertexInput.positionWS.xyz, 0);

				// normal
				output.normal = normalize(vertexNormalInput.normalWS);

				// tangent
				output.pack1.xyz = vertexNormalInput.tangentWS;
				output.pack2.xyz = vertexNormalInput.bitangentWS;

				// clip position
				output.positionCS = vertexInput.positionCS;

				return output;
			}

			half4 Fragment(Varyings input
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				float3 positionWS = input.worldPosAndFog.xyz;
				float3 normalWS = normalize(input.normal);
				half3 viewDirWS = GetWorldSpaceNormalizeViewDir(positionWS);
				half3 tangentWS = input.pack1.xyz;
				half3 bitangentWS = input.pack2.xyz;
				half3x3 tangentToWorldMatrix = half3x3(tangentWS.xyz, bitangentWS.xyz, normalWS.xyz);

				// Shader Properties Sampling
				float4 __normalMap = ( TCP2_TEX2D_SAMPLE(_BumpMap, _BumpMap, input.pack3.xy).rgba );
				float __bumpScale = ( _BumpScale );
				float4 __albedo = ( TCP2_TEX2D_SAMPLE(_BaseMap, _BaseMap, input.pack3.xy).rgba );
				float __alpha = ( _Alpha * TCP2_TEX2D_SAMPLE(_Alpha1, _Alpha1, input.pack3.xy).r );
				float4 __mainColor = ( _Color.rgba );
				float __ambientIntensity = ( 1.0 );
				float3 __emission = ( _Emission.rgb * TCP2_TEX2D_SAMPLE(_Emission1, _Emission1, input.pack3.xy).rgb );
				float __lightWrapFactor = ( _LightWrapFactor );
				float __rampThreshold = ( _RampThreshold );
				float __rampSmoothing = ( _RampSmoothing );
				float __bandsCount = ( _BandsCount );
				float __bandsCrispSmoothing = ( 2.0 );
				float __rimMin = ( _RimMin );
				float __rimMax = ( _RimMax );
				float3 __rimColor = ( _RimColor.rgb );
				float __rimStrength = ( 1.0 );
				float __specularSmoothness = ( _SpecularSmoothness );
				float3 __specularColor = ( _SpecularColor.rgb );
				float __rampThresholdOtherLights = ( _RampThresholdOtherLights );
				float __rampSmoothingOtherLights = ( _RampSmoothingOtherLights );
				float __bandsCountOtherLights = ( _BandsCountOtherLights );
				float3 __shadowColor = ( _SColor.rgb );
				float3 __highlightColor = ( _HColor.rgb );
				float __planarNormalsInfluence = ( _PlanarNormalsInfluence );
				float __fresnelMin = ( _FresnelMin );
				float __fresnelMax = ( _FresnelMax );
				float3 __reflectionColor = ( _ReflectionColor.rgb );

				half4 normalMap = __normalMap;
				half3 normalTS = UnpackNormalScale(normalMap, __bumpScale);
				normalWS = normalize( mul(normalTS, tangentToWorldMatrix) );

				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				// main texture
				half3 albedo = __albedo.rgb;
				half alpha = __alpha;

				half3 emission = half3(0,0,0);
				// Alpha Testing
				half cutoffValue = Dither4x4(input.positionCS.xy);
				
				albedo *= __mainColor.rgb;

				// main light: direction, color, distanceAttenuation, shadowAttenuation
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord = input.shadowCoord;
			#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
				float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
			#else
				float4 shadowCoord = float4(0, 0, 0, 0);
			#endif

			#if defined(URP_10_OR_NEWER)
				#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
					half4 shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);
				#elif !defined (LIGHTMAP_ON)
					half4 shadowMask = unity_ProbesOcclusion;
				#else
					half4 shadowMask = half4(1, 1, 1, 1);
				#endif

				Light mainLight = GetMainLight(shadowCoord, positionWS, shadowMask);
			#else
				Light mainLight = GetMainLight(shadowCoord);
			#endif

			#if defined(_SCREEN_SPACE_OCCLUSION) || defined(USE_FORWARD_PLUS)
				float2 normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
			#endif

				// ambient or lightmap
			#if defined(LIGHTMAP_ON) && defined(DYNAMICLIGHTMAP_ON)
				// Static & Dynamic Lightmap
				half3 bakedGI = SampleLightmap(input.staticLightmapUV, input.dynamicLightmapUV, normalWS);
				MixRealtimeAndBakedGI(mainLight, normalWS, bakedGI, half4(0, 0, 0, 0));
			#elif defined(LIGHTMAP_ON)
				// Static Lightmap
				half3 bakedGI = SampleLightmap(input.staticLightmapUV, 0, normalWS);
				MixRealtimeAndBakedGI(mainLight, normalWS, bakedGI, half4(0, 0, 0, 0));
			#elif defined(DYNAMICLIGHTMAP_ON)
				// Dynamic Lightmap
				half3 bakedGI = SampleLightmap(0, input.dynamicLightmapUV, normalWS);
				MixRealtimeAndBakedGI(mainLight, normalWS, bakedGI, half4(0, 0, 0, 0));
			#else
				half3 bakedGI = half3(0,0,0);
			#endif
				half occlusion = 1;

				half3 indirectDiffuse = bakedGI;
				indirectDiffuse *= occlusion * albedo * __ambientIntensity;
				emission += __emission;

				half3 lightDir = mainLight.direction;
				half3 lightColor = mainLight.color.rgb;

				half atten = mainLight.shadowAttenuation * mainLight.distanceAttenuation;

				half ndl = dot(normalWS, lightDir);
				half3 ramp;
				
				// Wrapped Lighting
				half lightWrap = __lightWrapFactor;
				ndl = (ndl + lightWrap) / (1 + lightWrap);
				
				half rampThreshold = __rampThreshold;
				half rampSmooth = __rampSmoothing * 0.5;
				half bandsCount = __bandsCount;
				ndl = saturate(ndl);
				half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
				half gradientLength = fwidth(ndl);
				half bandsSmooth = gradientLength * (__bandsCrispSmoothing + bandsCount);
				ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

				// apply attenuation
				ramp *= atten;

				half3 color = half3(0,0,0);
				// Rim Lighting
				#if defined(TCP2_RIM_LIGHTING)
				half rim = 1 - ndvRaw;
				rim = ( rim );
				half rimMin = __rimMin;
				half rimMax = __rimMax;
				rim = smoothstep(rimMin, rimMax, rim);
				half3 rimColor = __rimColor;
				half rimStrength = __rimStrength;
				emission.rgb += rim * rimColor * rimStrength;
				#endif
				half3 accumulatedRamp = ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
				half3 accumulatedColors = ramp * lightColor.rgb;

				half3 halfDir = SpecSafeNormalize(float3(lightDir) + float3(viewDirWS));
				
				//Blinn-Phong Specular
				float ndh = max(0, dot (normalWS, halfDir));
				float spec = pow(ndh, 1e-4h + __specularSmoothness * 128.0);
				spec *= ndl;
				spec *= atten;
				
				//Apply specular
				emission.rgb += spec * lightColor.rgb * __specularColor;

				// Additional lights loop
			#ifdef _ADDITIONAL_LIGHTS
				uint pixelLightCount = GetAdditionalLightsCount();

				#if USE_FORWARD_PLUS
					// Additional directional lights in Forward+
					for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
					{
						FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK

						Light light = GetAdditionalLight(lightIndex, positionWS, shadowMask);

						#if defined(_LIGHT_LAYERS)
							if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
						#endif
						{
							half atten = light.shadowAttenuation * light.distanceAttenuation;

							#if defined(_LIGHT_LAYERS)
								half3 lightDir = half3(0, 1, 0);
								half3 lightColor = half3(0, 0, 0);
								if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
								{
									lightColor = light.color.rgb;
									lightDir = light.direction;
								}
							#else
								half3 lightColor = light.color.rgb;
								half3 lightDir = light.direction;
							#endif

							half ndl = dot(normalWS, lightDir);
							half3 ramp;
							
							// Wrapped Lighting
							half lightWrap = __lightWrapFactor;
							ndl = (ndl + lightWrap) / (1 + lightWrap);
							
							half rampThreshold = __rampThresholdOtherLights;
							half rampSmooth = __rampSmoothingOtherLights * 0.5;
							half bandsCount = __bandsCountOtherLights;
							ndl = saturate(ndl);
							half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
							half gradientLength = fwidth(ndl);
							half bandsSmooth = gradientLength * (__bandsCrispSmoothing + bandsCount);
							ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

							// apply attenuation (shadowmaps & point/spot lights attenuation)
							ramp *= atten;

							accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
							accumulatedColors += ramp * lightColor.rgb;

							half3 halfDir = SpecSafeNormalize(float3(lightDir) + float3(viewDirWS));
							
							//Blinn-Phong Specular
							float ndh = max(0, dot (normalWS, halfDir));
							float spec = pow(ndh, 1e-4h + __specularSmoothness * 128.0);
							spec *= ndl;
							spec *= atten;
							
							//Apply specular
							emission.rgb += spec * lightColor.rgb * __specularColor;
						}
					}

					// Data with dummy struct used in Forward+ macro (LIGHT_LOOP_BEGIN)
					InputDataForwardPlusDummy inputData;
					inputData.normalizedScreenSpaceUV = normalizedScreenSpaceUV;
					inputData.positionWS = positionWS;
				#endif

				LIGHT_LOOP_BEGIN(pixelLightCount)
				{
					#if defined(URP_10_OR_NEWER)
						Light light = GetAdditionalLight(lightIndex, positionWS, shadowMask);
					#else
						Light light = GetAdditionalLight(lightIndex, positionWS);
					#endif
					half atten = light.shadowAttenuation * light.distanceAttenuation;

					#if defined(_LIGHT_LAYERS)
						half3 lightDir = half3(0, 1, 0);
						half3 lightColor = half3(0, 0, 0);
						if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
						{
							lightColor = light.color.rgb;
							lightDir = light.direction;
						}
					#else
						half3 lightColor = light.color.rgb;
						half3 lightDir = light.direction;
					#endif

					half ndl = dot(normalWS, lightDir);
					half3 ramp;
					
					// Wrapped Lighting
					half lightWrap = __lightWrapFactor;
					ndl = (ndl + lightWrap) / (1 + lightWrap);
					
					half rampThreshold = __rampThresholdOtherLights;
					half rampSmooth = __rampSmoothingOtherLights * 0.5;
					half bandsCount = __bandsCountOtherLights;
					ndl = saturate(ndl);
					half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
					half gradientLength = fwidth(ndl);
					half bandsSmooth = gradientLength * (__bandsCrispSmoothing + bandsCount);
					ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

					// apply attenuation (shadowmaps & point/spot lights attenuation)
					ramp *= atten;

					accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
					accumulatedColors += ramp * lightColor.rgb;

					half3 halfDir = SpecSafeNormalize(float3(lightDir) + float3(viewDirWS));
					
					//Blinn-Phong Specular
					float ndh = max(0, dot (normalWS, halfDir));
					float spec = pow(ndh, 1e-4h + __specularSmoothness * 128.0);
					spec *= ndl;
					spec *= atten;
					
					//Apply specular
					emission.rgb += spec * lightColor.rgb * __specularColor;
				}
				LIGHT_LOOP_END
			#endif
			#ifdef _ADDITIONAL_LIGHTS_VERTEX
				color += input.vertexLights * albedo;
			#endif

				accumulatedRamp = saturate(accumulatedRamp);
				half3 shadowColor = (1 - accumulatedRamp.rgb) * __shadowColor;
				accumulatedRamp = accumulatedColors.rgb * __highlightColor + shadowColor;
				color += albedo * accumulatedRamp;

				// apply ambient
				color += indirectDiffuse;

				half3 reflections = half3(0, 0, 0);
				#if defined(TCP2_REFLECTIONS)
				float2 planarReflectionCoords = (input.screenPosition.xyzw.xy + (normalTS.xy * __planarNormalsInfluence)) / input.screenPosition.xyzw.w;
				reflections.rgb += tex2D(_ReflectionTex, planarReflectionCoords).rgb;
				#endif
				half fresnelMin = __fresnelMin;
				half fresnelMax = __fresnelMax;
				half fresnelTerm = smoothstep(fresnelMin, fresnelMax, 1 - ndvRaw);
				reflections *= fresnelTerm;
				reflections *= __reflectionColor;
				color.rgb += reflections;

				color += emission;

				return half4(color, alpha);
			}
			ENDHLSL
		}

		// Depth & Shadow Caster Passes
		HLSLINCLUDE

		#if defined(SHADOW_CASTER_PASS) || defined(DEPTH_ONLY_PASS)

			#define fixed half
			#define fixed2 half2
			#define fixed3 half3
			#define fixed4 half4

			float3 _LightDirection;
			float3 _LightPosition;

			struct Attributes
			{
				float4 vertex   : POSITION;
				float3 normal   : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				float4 positionCS     : SV_POSITION;
				float3 normal         : NORMAL;
			#if defined(DEPTH_NORMALS_PASS)
				float3 normalWS : TEXCOORD0;
			#endif
				float4 screenPosition : TEXCOORD1;
				float3 pack1 : TEXCOORD2; /* pack1.xyz = positionWS */
				float2 pack2 : TEXCOORD3; /* pack2.xy = texcoord0 */
			#if defined(DEPTH_ONLY_PASS)
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			#endif
			};

			float4 GetShadowPositionHClip(Attributes input)
			{
				float3 positionWS = TransformObjectToWorld(input.vertex.xyz);
				float3 normalWS = TransformObjectToWorldNormal(input.normal);

				#if _CASTING_PUNCTUAL_LIGHT_SHADOW
					float3 lightDirectionWS = normalize(_LightPosition - positionWS);
				#else
					float3 lightDirectionWS = _LightDirection;
				#endif
				float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

				#if UNITY_REVERSED_Z
					positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#else
					positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#endif

				return positionCS;
			}

			Varyings ShadowDepthPassVertex(Attributes input)
			{
				Varyings output = (Varyings)0;
				UNITY_SETUP_INSTANCE_ID(input);
				#if defined(DEPTH_ONLY_PASS)
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
				#endif

				float3 worldNormalUv = mul(UNITY_MATRIX_M, float4(input.normal, 1.0)).xyz;

				// Texture Coordinates
				output.pack2.xy = input.texcoord0.xy;

				float3 worldPos = mul(UNITY_MATRIX_M, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);

				// Screen Space UV
				float4 screenPos = ComputeScreenPos(vertexInput.positionCS);
				output.screenPosition.xyzw = screenPos;
				output.normal = normalize(worldNormalUv);
				output.pack1.xyz = vertexInput.positionWS;

				#if defined(DEPTH_ONLY_PASS)
					output.positionCS = TransformObjectToHClip(input.vertex.xyz);
					#if defined(DEPTH_NORMALS_PASS)
						float3 normalWS = TransformObjectToWorldNormal(input.normal);
						output.normalWS = normalWS; // already normalized in TransformObjectToWorldNormal
					#endif
				#elif defined(SHADOW_CASTER_PASS)
					output.positionCS = GetShadowPositionHClip(input);
				#else
					output.positionCS = float4(0,0,0,0);
				#endif

				return output;
			}

			half4 ShadowDepthPassFragment(
				Varyings input
	#if defined(DEPTH_NORMALS_PASS) && defined(_WRITE_RENDERING_LAYERS)
				, out float4 outRenderingLayers : SV_Target1
	#endif
			) : SV_TARGET
			{
				#if defined(DEPTH_ONLY_PASS)
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
				#endif

				float3 positionWS = input.pack1.xyz;
				float3 normalWS = normalize(input.normal);

				// Shader Properties Sampling
				float __alpha = ( _Alpha * TCP2_TEX2D_SAMPLE(_Alpha1, _Alpha1, input.pack2.xy).r );

				half3 viewDirWS = GetWorldSpaceNormalizeViewDir(positionWS);
				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				half3 albedo = half3(1,1,1);
				half alpha = __alpha;
				half3 emission = half3(0,0,0);
				// Alpha Testing
				half cutoffValue = Dither4x4(input.positionCS.xy);

				#if defined(DEPTH_NORMALS_PASS)
					#if defined(_WRITE_RENDERING_LAYERS)
						uint meshRenderingLayers = GetMeshRenderingLayer();
						outRenderingLayers = float4(EncodeMeshRenderingLayer(meshRenderingLayers), 0, 0, 0);
					#endif

					#if defined(URP_12_OR_NEWER)
						return float4(input.normalWS.xyz, 0.0);
					#else
						return float4(PackNormalOctRectEncode(TransformWorldToViewDir(input.normalWS, true)), 0.0, 0.0);
					#endif
				#endif

				return alpha;
			}

		#endif
		ENDHLSL

		Pass
		{
			Name "ShadowCaster"
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

			AlphaToMask On
			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile SHADOW_CASTER_PASS

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing
			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "DepthOnly"
			Tags
			{
				"LightMode" = "DepthOnly"
			}

			AlphaToMask On
			ZWrite On
			ColorMask 0

			HLSLPROGRAM

			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile DEPTH_ONLY_PASS

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment

			ENDHLSL
		}

		Pass
		{
			Name "DepthNormals"
			Tags
			{
				"LightMode" = "DepthNormals"
			}

			ZWrite On

			HLSLPROGRAM
			#pragma exclude_renderers gles gles3 glcore
			#pragma target 2.0

			#pragma multi_compile_instancing

			// using simple #define doesn't work, we have to use this instead
			#pragma multi_compile DEPTH_ONLY_PASS
			#pragma multi_compile DEPTH_NORMALS_PASS

			#pragma vertex ShadowDepthPassVertex
			#pragma fragment ShadowDepthPassFragment

			ENDHLSL
		}

	}

	FallBack "Hidden/InternalErrorShader"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(ver:"2.9.16";unity:"6000.0.32f1";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2019_4","UNITY_2020_1","UNITY_2021_1","UNITY_2021_2","UNITY_2022_2","ENABLE_DEPTH_NORMALS_PASS","ENABLE_FORWARD_PLUS","RAMP_MAIN_OTHER","RAMP_SEPARATED","WRAPPED_LIGHTING_CUSTOM","SPEC_LEGACY","SPECULAR","RIM","BUMP","BUMP_SCALE","RAMP_BANDS_CRISP","ENABLE_LIGHTMAP","PLANAR_REFLECTION","REFLECTION_SHADER_FEATURE","REFLECTION_FRESNEL","SKETCH_SHADER_FEATURE","OUTLINE_ZSMOOTH","OUTLINE_LIGHTING_WRAP","RIM_SHADER_FEATURE","EMISSION","ALPHA_BLENDING","SHADER_BLENDING","ALPHA_TESTING_DITHERING","ALPHA_TESTING","ALPHA_TO_COVERAGE_RAW","BLEND_OP","NO_AMBIENT","ALPHA_TO_COVERAGE","TEMPLATE_LWRP"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="TransparentCutout",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",RIM_LABEL="Rim Lighting"];shaderProperties:list[,sp(name:"Main Color";imps:list[imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:False;cc:4;chan:"RGBA";prop:"_Color";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"df6029d6-669d-4ffd-9a6e-1080d7112935";op:Multiply;lbl:"Color";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False),sp(name:"Alpha";imps:list[imp_mp_float(def:1;prop:"_Alpha";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"c45dc92a-cacd-448b-93f8-89bd04e59839";op:Multiply;lbl:"Alpha";gpu_inst:False;dots_inst:False;locked:False;impl_index:-1),imp_mp_texture(uto:False;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:1;chan:"R";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_Alpha1";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"b9c727ef-d13d-4e9a-945d-cf06e31f1556";op:Multiply;lbl:"Alpha Texture";gpu_inst:False;dots_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list["1d7310"];layer_blend:dict[1d7310=Add];custom_blend:dict[1d7310="lerp(a, b, s)"];clones:dict[1d7310=sp(name:"Alpha_1d7310";imps:list[imp_mp_float(def:1;prop:"_Alpha_1d7310";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"00000000-0000-0000-0000-000000000000";op:Multiply;lbl:"Alpha";gpu_inst:False;dots_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:True)];isClone:False),,,,,,,,,,,,,,,,,,,,sp(name:"Emission";imps:list[imp_mp_color(def:RGBA(0, 0, 0, 1);hdr:True;cc:3;chan:"RGB";prop:"_Emission";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"47cd869f-6718-40a9-b1ad-8a8cb2729a37";op:Multiply;lbl:"Emission Color";gpu_inst:False;dots_inst:False;locked:False;impl_index:0),imp_mp_texture(uto:False;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:3;chan:"RGB";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_Emission1";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"3e2b807e-0620-4efe-9586-bcd5392be8d8";op:Multiply;lbl:"Emission Texture";gpu_inst:False;dots_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];layer_blend:dict[1d7310=LinearInterpolation];custom_blend:dict[1d7310="lerp(a, b, s)"];clones:dict[];isClone:False),,,,,,sp(name:"Outline Width";imps:list[imp_mp_range(def:1;min:0.001;max:4;prop:"_OutlineWidth";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"ffbaab38-f570-4eea-ae7f-a88e34edf03a";op:Multiply;lbl:"Width";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False),,,,,,,sp(name:"Blend Operation";imps:list[imp_enum(value_type:1;value:0;enum_type:"ToonyColorsPro.ShaderGenerator.BlendOperation";guid:"7c469339-1763-42b1-ac39-dbd88f5e52df";op:Multiply;lbl:"Blend Operation";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False)];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH fb2d474e76d2efa03e47c55779c98734 */
