// Toony Colors Pro+Mobile 2
// (c) 2014-2023 Jean Moreno

Shader "Toony Colors Pro 2/User/My TCP2 Shader3"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_BaseColor ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[MainTexture] _BaseMap ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		
		[TCP2HeaderHelp(Main Directional Light)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCount ("Bands Count", Range(1,20)) = 4
		_BandsSmoothing ("Bands Smoothing", Range(0.001,1)) = 0.1
		[TCP2HeaderHelp(Other Lights)]
		_RampThresholdOtherLights ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothingOtherLights ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCountOtherLights ("Bands Count", Range(1,20)) = 4
		_BandsSmoothingOtherLights ("Bands Smoothing", Range(0.001,1)) = 0.1
		[Space]
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1
		[TCP2Separator]
		
		[TCP2HeaderHelp(Subsurface Scattering)]
		_SubsurfaceDistortion ("Distortion", Range(0,2)) = 0.2
		_SubsurfacePower ("Power", Range(0.1,16)) = 3
		_SubsurfaceScale ("Scale", Float) = 1
		[TCP2ColorNoAlpha] _SubsurfaceColor ("Color", Color) = (0.5,0.5,0.5,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Normal Mapping)]
		[NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
		[TCP2Separator]
		
		[TCP2ColorNoAlpha] _DiffuseTint ("Diffuse Tint", Color) = (1,0.5,0,1)
		[TCP2Separator]
		
		[TCP2Separator]
		[TCP2HeaderHelp(MATERIAL LAYERS)]

		[TCP2Separator]
		[TCP2Header(Material Layer)]
		[NoScaleOffset] _layer_materialLayer ("Source Texture", 2D) = "white" {}
		[MainTexture] [NoScaleOffset] _BaseMap_materialLayer ("Albedo", 2D) = "white" {}
		_BaseColor_materialLayer ("Color", Color) = (1,1,1,1)
		_RampThreshold_materialLayer ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing_materialLayer ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCount_materialLayer ("Bands Count", Range(1,20)) = 4
		_BandsSmoothing_materialLayer ("Bands Smoothing", Range(0.001,1)) = 0.1
		[TCP2ColorNoAlpha] _DiffuseTint_materialLayer ("Diffuse Tint", Color) = (1,0.5,0,1)
		_RampSmoothingOtherLights_materialLayer ("Smoothing", Range(0.001,1)) = 0.5
		[IntRange] _BandsCountOtherLights_materialLayer ("Bands Count", Range(1,20)) = 4
		_BandsSmoothingOtherLights_materialLayer ("Bands Smoothing", Range(0.001,1)) = 0.1
		[TCP2ColorNoAlpha] _SColor_materialLayer ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[TCP2ColorNoAlpha] _HColor_materialLayer ("Highlight Color", Color) = (0.75,0.75,0.75,1)

		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1

		// Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType"="Opaque"
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
		TCP2_TEX2D_WITH_SAMPLER(_BaseMap_materialLayer);
		TCP2_TEX2D_WITH_SAMPLER(_layer_materialLayer);

		CBUFFER_START(UnityPerMaterial)
			
			// Shader Properties
			float4 _BaseMap_ST;
			float4 _BaseMap_materialLayer_ST;
			fixed4 _BaseColor;
			fixed4 _BaseColor_materialLayer;
			float _RampThreshold;
			float _RampThreshold_materialLayer;
			float _RampSmoothing;
			float _RampSmoothing_materialLayer;
			float _BandsCount;
			float _BandsCount_materialLayer;
			float _BandsSmoothing;
			float _BandsSmoothing_materialLayer;
			fixed4 _DiffuseTint;
			fixed4 _DiffuseTint_materialLayer;
			float _RimMin;
			float _RimMax;
			fixed4 _RimColor;
			float _RampThresholdOtherLights;
			float _RampSmoothingOtherLights;
			float _RampSmoothingOtherLights_materialLayer;
			float _BandsCountOtherLights;
			float _BandsCountOtherLights_materialLayer;
			float _BandsSmoothingOtherLights;
			float _BandsSmoothingOtherLights_materialLayer;
			float _SubsurfaceDistortion;
			float _SubsurfacePower;
			float _SubsurfaceScale;
			fixed4 _SubsurfaceColor;
			fixed4 _SColor;
			fixed4 _SColor_materialLayer;
			fixed4 _HColor;
			fixed4 _HColor_materialLayer;
		CBUFFER_END

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

			//--------------------------------------
			// GPU Instancing
			#pragma multi_compile_instancing

			#pragma vertex Vertex
			#pragma fragment Fragment

			// vertex input
			struct Attributes
			{
				float4 vertex       : POSITION;
				float3 normal       : NORMAL;
				float4 tangent      : TANGENT;
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
				float3 pack0 : TEXCOORD3; /* pack0.xyz = tangent */
				float3 pack1 : TEXCOORD4; /* pack1.xyz = bitangent */
				float2 pack2 : TEXCOORD5; /* pack2.xy = texcoord0 */
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
				output.pack2.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				float3 worldPos = mul(UNITY_MATRIX_M, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
			#endif

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
				output.pack0.xyz = vertexNormalInput.tangentWS;
				output.pack1.xyz = vertexNormalInput.bitangentWS;

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
				half3 tangentWS = input.pack0.xyz;
				half3 bitangentWS = input.pack1.xyz;
				half3x3 tangentToWorldMatrix = half3x3(tangentWS.xyz, bitangentWS.xyz, normalWS.xyz);

				// Shader Properties Sampling
				float4 __normalMap = ( TCP2_TEX2D_SAMPLE(_BumpMap, _BumpMap, input.pack2.xy).rgba );
				float4 __albedo = ( TCP2_TEX2D_SAMPLE(_BaseMap, _BaseMap, input.pack2.xy).rgba );
				float4 __albedo_materialLayer = ( TCP2_TEX2D_SAMPLE(_BaseMap_materialLayer, _BaseMap_materialLayer, input.pack2.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float4 __mainColor_materialLayer = ( _BaseColor_materialLayer.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float __ambientIntensity = ( 1.0 );
				float __rampThreshold = ( _RampThreshold );
				float __rampThreshold_materialLayer = ( _RampThreshold_materialLayer );
				float __rampSmoothing = ( _RampSmoothing );
				float __rampSmoothing_materialLayer = ( _RampSmoothing_materialLayer );
				float __bandsCount = ( _BandsCount );
				float __bandsCount_materialLayer = ( _BandsCount_materialLayer );
				float __bandsSmoothing = ( _BandsSmoothing );
				float __bandsSmoothing_materialLayer = ( _BandsSmoothing_materialLayer );
				float3 __diffuseTint = ( _DiffuseTint.rgb );
				float3 __diffuseTint_materialLayer = ( _DiffuseTint_materialLayer.rgb );
				float __rimMin = ( _RimMin );
				float __rimMax = ( _RimMax );
				float3 __rimColor = ( _RimColor.rgb );
				float __rimStrength = ( 1.0 );
				float __rampThresholdOtherLights = ( _RampThresholdOtherLights );
				float __rampSmoothingOtherLights = ( _RampSmoothingOtherLights );
				float __rampSmoothingOtherLights_materialLayer = ( _RampSmoothingOtherLights_materialLayer );
				float __bandsCountOtherLights = ( _BandsCountOtherLights );
				float __bandsCountOtherLights_materialLayer = ( _BandsCountOtherLights_materialLayer );
				float __bandsSmoothingOtherLights = ( _BandsSmoothingOtherLights );
				float __bandsSmoothingOtherLights_materialLayer = ( _BandsSmoothingOtherLights_materialLayer );
				float __subsurfaceDistortion = ( _SubsurfaceDistortion );
				float __subsurfacePower = ( _SubsurfacePower );
				float __subsurfaceScale = ( _SubsurfaceScale );
				float3 __subsurfaceColor = ( _SubsurfaceColor.rgb );
				float3 __shadowColor = ( _SColor.rgb );
				float3 __shadowColor_materialLayer = ( _SColor_materialLayer.rgb );
				float3 __highlightColor = ( _HColor.rgb );
				float3 __highlightColor_materialLayer = ( _HColor_materialLayer.rgb );
				float __layer_materialLayer = saturate( TCP2_TEX2D_SAMPLE(_layer_materialLayer, _layer_materialLayer, input.pack2.xy).r );

				// Material Layers Blending
				 __albedo = lerp(__albedo, __albedo_materialLayer, __layer_materialLayer);
				 __mainColor = lerp(__mainColor, __mainColor_materialLayer, __layer_materialLayer);
				 __rampThreshold = lerp(__rampThreshold, __rampThreshold_materialLayer, __layer_materialLayer);
				 __rampSmoothing = lerp(__rampSmoothing, __rampSmoothing_materialLayer, __layer_materialLayer);
				 __bandsCount = lerp(__bandsCount, __bandsCount_materialLayer, __layer_materialLayer);
				 __bandsSmoothing = lerp(__bandsSmoothing, __bandsSmoothing_materialLayer, __layer_materialLayer);
				 __diffuseTint = lerp(__diffuseTint, __diffuseTint_materialLayer, __layer_materialLayer);
				 __rampSmoothingOtherLights = lerp(__rampSmoothingOtherLights, __rampSmoothingOtherLights_materialLayer, __layer_materialLayer);
				 __bandsCountOtherLights = lerp(__bandsCountOtherLights, __bandsCountOtherLights_materialLayer, __layer_materialLayer);
				 __bandsSmoothingOtherLights = lerp(__bandsSmoothingOtherLights, __bandsSmoothingOtherLights_materialLayer, __layer_materialLayer);
				 __shadowColor = lerp(__shadowColor, __shadowColor_materialLayer, __layer_materialLayer);
				 __highlightColor = lerp(__highlightColor, __highlightColor_materialLayer, __layer_materialLayer);

				half4 normalMap = __normalMap;
				half3 normalTS = UnpackNormal(normalMap);
				normalWS = normalize( mul(normalTS, tangentToWorldMatrix) );

				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				// main texture
				half3 albedo = __albedo.rgb;
				half alpha = __alpha;

				half3 emission = half3(0,0,0);
				
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
				// Samples SH fully per-pixel. SampleSHVertex and SampleSHPixel functions
				// are also defined in case you want to sample some terms per-vertex.
				half3 bakedGI = SampleSH(normalWS);
				half occlusion = 1;

				half3 indirectDiffuse = bakedGI;
				indirectDiffuse *= occlusion * albedo * __ambientIntensity;

				half3 lightDir = mainLight.direction;
				half3 lightColor = mainLight.color.rgb;

				half atten = mainLight.shadowAttenuation * mainLight.distanceAttenuation;

				half ndl = dot(normalWS, lightDir);
				half3 ramp;
				
				// Wrapped Lighting
				ndl = ndl * 0.5 + 0.5;
				
				half rampThreshold = __rampThreshold;
				half rampSmooth = __rampSmoothing * 0.5;
				half bandsCount = __bandsCount;
				half bandsSmoothing = __bandsSmoothing;
				ndl = saturate(ndl);
				half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
				half bandsSmooth = bandsSmoothing * 0.5;
				ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

				// apply attenuation
				ramp *= atten;

				// Diffuse Tint
				half3 diffuseTint = saturate(__diffuseTint + ndl);
				ramp *= diffuseTint;
				
				half3 color = half3(0,0,0);
				// Rim Lighting
				half rim = 1 - ndvRaw;
				rim = ( rim );
				half rimMin = __rimMin;
				half rimMax = __rimMax;
				rim = smoothstep(rimMin, rimMax, rim);
				half3 rimColor = __rimColor;
				half rimStrength = __rimStrength;
				emission.rgb += rim * rimColor * rimStrength;
				half3 accumulatedRamp = ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
				half3 accumulatedColors = ramp * lightColor.rgb;

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
							ndl = ndl * 0.5 + 0.5;
							
							half rampThreshold = __rampThresholdOtherLights;
							half rampSmooth = __rampSmoothingOtherLights * 0.5;
							half bandsCount = __bandsCountOtherLights;
							half bandsSmoothing = __bandsSmoothingOtherLights;
							ndl = saturate(ndl);
							half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
							half bandsSmooth = bandsSmoothing * 0.5;
							ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

							// apply attenuation (shadowmaps & point/spot lights attenuation)
							ramp *= atten;

							// Diffuse Tint
							half3 diffuseTint = saturate(__diffuseTint + ndl);
							ramp *= diffuseTint;
							
							accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
							accumulatedColors += ramp * lightColor.rgb;

							//Subsurface Scattering for additional lights
							half3 ssLight = lightDir + normalWS * __subsurfaceDistortion;
							half ssDot = pow(saturate(dot(viewDirWS, -ssLight)), __subsurfacePower) * __subsurfaceScale;
							half3 ssColor = (ssDot * __subsurfaceColor);
							ssColor *= atten;
							ssColor *= lightColor;
							color.rgb += albedo * ssColor;
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
					ndl = ndl * 0.5 + 0.5;
					
					half rampThreshold = __rampThresholdOtherLights;
					half rampSmooth = __rampSmoothingOtherLights * 0.5;
					half bandsCount = __bandsCountOtherLights;
					half bandsSmoothing = __bandsSmoothingOtherLights;
					ndl = saturate(ndl);
					half bandsNdl = smoothstep(rampThreshold - rampSmooth, rampThreshold + rampSmooth, ndl);
					half bandsSmooth = bandsSmoothing * 0.5;
					ramp = saturate((smoothstep(0.5 - bandsSmooth, 0.5 + bandsSmooth, frac(bandsNdl * bandsCount)) + floor(bandsNdl * bandsCount)) / bandsCount).xxx;

					// apply attenuation (shadowmaps & point/spot lights attenuation)
					ramp *= atten;

					// Diffuse Tint
					half3 diffuseTint = saturate(__diffuseTint + ndl);
					ramp *= diffuseTint;
					
					accumulatedRamp += ramp * max(lightColor.r, max(lightColor.g, lightColor.b));
					accumulatedColors += ramp * lightColor.rgb;

					//Subsurface Scattering for additional lights
					half3 ssLight = lightDir + normalWS * __subsurfaceDistortion;
					half ssDot = pow(saturate(dot(viewDirWS, -ssLight)), __subsurfacePower) * __subsurfaceScale;
					half3 ssColor = (ssDot * __subsurfaceColor);
					ssColor *= atten;
					ssColor *= lightColor;
					color.rgb += albedo * ssColor;
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
				float3 pack0 : TEXCOORD1; /* pack0.xyz = positionWS */
				float2 pack1 : TEXCOORD2; /* pack1.xy = texcoord0 */
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
				output.pack1.xy.xy = input.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;

				float3 worldPos = mul(UNITY_MATRIX_M, input.vertex).xyz;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
				output.normal = normalize(worldNormalUv);
				output.pack0.xyz = vertexInput.positionWS;

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

				float3 positionWS = input.pack0.xyz;
				float3 normalWS = normalize(input.normal);

				// Shader Properties Sampling
				float4 __albedo = ( TCP2_TEX2D_SAMPLE(_BaseMap, _BaseMap, input.pack1.xy).rgba );
				float4 __albedo_materialLayer = ( TCP2_TEX2D_SAMPLE(_BaseMap_materialLayer, _BaseMap_materialLayer, input.pack1.xy).rgba );
				float4 __mainColor = ( _BaseColor.rgba );
				float4 __mainColor_materialLayer = ( _BaseColor_materialLayer.rgba );
				float __alpha = ( __albedo.a * __mainColor.a );
				float __layer_materialLayer = saturate( TCP2_TEX2D_SAMPLE(_layer_materialLayer, _layer_materialLayer, input.pack1.xy).r );

				// Material Layers Blending
				 __albedo = lerp(__albedo, __albedo_materialLayer, __layer_materialLayer);
				 __mainColor = lerp(__mainColor, __mainColor_materialLayer, __layer_materialLayer);

				half3 viewDirWS = GetWorldSpaceNormalizeViewDir(positionWS);
				half ndv = abs(dot(viewDirWS, normalWS));
				half ndvRaw = ndv;

				half3 albedo = half3(1,1,1);
				half alpha = __alpha;
				half3 emission = half3(0,0,0);

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

				return 0;
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

/* TCP_DATA u config(ver:"2.9.16";unity:"6000.0.32f1";tmplt:"SG2_Template_URP";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2019_4","UNITY_2020_1","UNITY_2021_1","UNITY_2021_2","UNITY_2022_2","ENABLE_DEPTH_NORMALS_PASS","ENABLE_FORWARD_PLUS","RAMP_BANDS","RAMP_MAIN_OTHER","RAMP_SEPARATED","WRAPPED_LIGHTING_HALF","RIM","DIFFUSE_TINT","TEMPLATE_LWRP","SUBSURFACE_SCATTERING","BUMP"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",RIM_LABEL="Rim Lighting"];shaderProperties:list[sp(name:"Albedo";imps:list[imp_mp_texture(uto:True;tov:"";tov_lbl:"";gto:True;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:4;chan:"RGBA";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_BaseMap";md:"[MainTexture]";gbv:False;custom:False;refs:"";pnlock:False;guid:"c7b343be-7508-4a03-92b1-fc3dc7ab2665";op:Multiply;lbl:"Albedo";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Main Color";imps:list[imp_mp_color(def:RGBA(1, 1, 1, 1);hdr:False;cc:4;chan:"RGBA";prop:"_BaseColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"21a800f6-f817-4ffd-b37c-83edf7103fb0";op:Multiply;lbl:"Color";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),,,sp(name:"Ramp Threshold";imps:list[imp_mp_range(def:0.5;min:0.01;max:1;prop:"_RampThreshold";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"eed1e367-e89e-404a-a456-8e6e12dadd51";op:Multiply;lbl:"Threshold";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Ramp Smoothing";imps:list[imp_mp_range(def:0.5;min:0.001;max:1;prop:"_RampSmoothing";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"daf37ac2-d607-4a26-a68f-5f4cc12e7c2d";op:Multiply;lbl:"Smoothing";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Bands Count";imps:list[imp_mp_range(def:4;min:1;max:20;prop:"_BandsCount";md:"[IntRange]";gbv:False;custom:False;refs:"";pnlock:False;guid:"7d23db0f-b99d-47bc-9ff8-5f1351e9fce4";op:Multiply;lbl:"Bands Count";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Bands Smoothing";imps:list[imp_mp_range(def:0.1;min:0.001;max:1;prop:"_BandsSmoothing";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"23715d20-7a7d-43d3-88c8-b9a9e8f4a138";op:Multiply;lbl:"Bands Smoothing";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),,sp(name:"Ramp Smoothing Other Lights";imps:list[imp_mp_range(def:0.5;min:0.001;max:1;prop:"_RampSmoothingOtherLights";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"37205664-f32a-4928-b62f-e387b8756e12";op:Multiply;lbl:"Smoothing";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Bands Count Other Lights";imps:list[imp_mp_range(def:4;min:1;max:20;prop:"_BandsCountOtherLights";md:"[IntRange]";gbv:False;custom:False;refs:"";pnlock:False;guid:"b7a5f60d-429e-4576-be9b-af86e85ea3be";op:Multiply;lbl:"Bands Count";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Bands Smoothing Other Lights";imps:list[imp_mp_range(def:0.1;min:0.001;max:1;prop:"_BandsSmoothingOtherLights";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"fd534272-0d21-490a-bb97-20ee6e00fdd2";op:Multiply;lbl:"Bands Smoothing";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Highlight Color";imps:list[imp_mp_color(def:RGBA(0.75, 0.75, 0.75, 1);hdr:False;cc:3;chan:"RGB";prop:"_HColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"ef1e2c5f-cc81-4cf0-909a-f10e59d79248";op:Multiply;lbl:"Highlight Color";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),sp(name:"Shadow Color";imps:list[imp_mp_color(def:RGBA(0.2, 0.2, 0.2, 1);hdr:False;cc:3;chan:"RGB";prop:"_SColor";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"29def1ce-5d5c-4c25-a01e-4f58367914c6";op:Multiply;lbl:"Shadow Color";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False),,,,,,sp(name:"Diffuse Tint";imps:list[imp_mp_color(def:RGBA(1, 0.5, 0, 1);hdr:False;cc:3;chan:"RGB";prop:"_DiffuseTint";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"7876565e-8ffa-4adb-840b-b8204b36865c";op:Multiply;lbl:"Diffuse Tint";gpu_inst:False;dots_inst:False;locked:False;impl_index:0)];layers:list["4cee88"];unlocked:list[];layer_blend:dict[4cee88=LinearInterpolation];custom_blend:dict[4cee88="lerp(a, b, s)"];clones:dict[];isClone:False)];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[ml(uid:"4cee88";name:"Material Layer";src:sp(name:"layer_4cee88";imps:list[imp_mp_texture(uto:False;tov:"";tov_lbl:"";gto:False;sbt:False;scr:False;scv:"";scv_lbl:"";gsc:False;roff:False;goff:False;sin_anm:False;sin_anmv:"";sin_anmv_lbl:"";gsin:False;notile:False;triplanar_local:False;def:"white";locked_uv:False;uv:0;cc:1;chan:"R";mip:-1;mipprop:False;ssuv_vert:False;ssuv_obj:False;uv_type:Texcoord;uv_chan:"XZ";tpln_scale:1;uv_shaderproperty:__NULL__;uv_cmp:__NULL__;sep_sampler:__NULL__;prop:"_layer_4cee88";md:"";gbv:False;custom:False;refs:"";pnlock:False;guid:"4e57b446-75bf-4434-b5e5-b67853717221";op:Multiply;lbl:"Source Texture";gpu_inst:False;dots_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];layer_blend:dict[];custom_blend:dict[];clones:dict[];isClone:False);use_contrast:False;ctrst:__NULL__;use_noise:False;noise:__NULL__)]) */
/* TCP_HASH f919cda98c8b4911a5f8d8631040fbfc */
