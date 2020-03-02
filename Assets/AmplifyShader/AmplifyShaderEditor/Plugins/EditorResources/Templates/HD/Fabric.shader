Shader /*ase_name*/ "Hidden/HD/Fabric" /*end*/
{
	Properties
	{
		/*ase_props*/
		[HideInInspector] _EmissionColor( "Emission Color", Color ) = ( 1, 1, 1, 1 )
		[HideInInspector] [ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 2
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 3
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 0
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 48
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 128
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 176
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 64
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 64
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 51
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 2
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector] [ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector] [ToggleUI] _ReceivesSSR("Receives SSR", Float) = 0
		[HideInInspector] _SurfaceType("Surface Type", Float) = 0
		[HideInInspector] _BlendMode("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector] [ToggleUI] _ZWrite("ZWrite", Float) = 0
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Int) = 0
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2
		[HideInInspector] [Enum(Front, 1, Back, 2)] _TransparentCullMode("Transparent Cull Mode", Float) = 2
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4
		[HideInInspector] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Float) = 4
		[HideInInspector] [ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector] [ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0
		[HideInInspector] [ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector] [ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 0
		[HideInInspector] [Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = ( 1, 1, -1, 0 )
	}

	SubShader
	{
		/*ase_subshader_options:Name=Additional Options
			Port:ForwardOnly:Bent Normal
				On:SetDefine:ASE_BENT_NORMAL 1
			Port:ForwardOnly:Occlusion
				On:SetDefine:_AMBIENT_OCCLUSION 1
			Port:ForwardOnly:Baked GI
				On:SetDefine:_ASE_BAKEDGI 1
			Port:ForwardOnly:Baked Back GI
				On:SetDefine:_ASE_BAKEDBACKGI 1
			Option:Surface Type:Opaque,Transparent:Opaque
				Opaque:SetShaderProperty:_SurfaceType,0
				Opaque:SetPropertyOnSubShader:RenderQueue,Geometry
				Opaque:HideOption:  Blend Preserves Specular
				Opaque:HideOption:  Fog
				Opaque:HideOption:  Depth Write
				Opaque:HideOption:  Cull Mode
				Opaque:HideOption:  Depth Test
				Opaque:ShowOption:  Subsurface Scattering
				Transparent:SetShaderProperty:_SurfaceType,1
				Transparent:SetPropertyOnSubShader:RenderQueue,Transparent
				Transparent:ShowOption:  Blend Preserves Specular
				Transparent:ShowOption:  Fog
				Transparent:ShowOption:  Depth Write
				Transparent:ShowOption:  Cull Mode
				Transparent:ShowOption:  Depth Test
				Transparent:HideOption:  Subsurface Scattering
			Option:  Blend Preserves Specular:false,true:true
				true:SetDefine:_BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
				false,disable:RemoveDefine:_BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
			Option:  Fog:false,true:true
				true:SetDefine:_ENABLE_FOG_ON_TRANSPARENT 1
				false,disable:RemoveDefine:_ENABLE_FOG_ON_TRANSPARENT 1
			Option:  Depth Write:false,true:false
				true:SetShaderProperty:_ZWrite,1
				false,disable:SetShaderProperty:_ZWrite,0
			Option:  Cull Mode:Back,Front:Back
				Front:SetShaderProperty:_TransparentCullMode,1
				Back:SetShaderProperty:_TransparentCullMode,2
			Option:  Depth Test:Disabled,Never,Less,Equal,Less Equal,Greater,Not Equal,Greater Equal,Always:Less Equal
				Never:SetShaderProperty:_ZTestTransparent,1
				Less:SetShaderProperty:_ZTestTransparent,2
				Equal:SetShaderProperty:_ZTestTransparent,3
				Less Equal:SetShaderProperty:_ZTestTransparent,4
				Greater:SetShaderProperty:_ZTestTransparent,5
				Not Equal:SetShaderProperty:_ZTestTransparent,6
				Greater Equal:SetShaderProperty:_ZTestTransparent,7
				Always:SetShaderProperty:_ZTestTransparent,8
			Option:  Subsurface Scattering:false,true:false
				true:ShowPort:ForwardOnly:Subsurface Mask
				true:SetDefine:_MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
				true:SetShaderProperty:_StencilRef,1
				true:SetShaderProperty:_StencilRefGBuffer,1
				true:SetShaderProperty:_RequireSplitLighting,1
				false,disable:HidePort:ForwardOnly:Subsurface Mask
				false,disable:RemoveDefine:_MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			Option:Alpha Clipping:false,true:false
				true:SetShaderProperty:_AlphaCutoffEnable,1
				true:SetDefine:ForwardOnly:SHADERPASS_FORWARD_BYPASS_ALPHA_TEST 1
				true:SetDefine:_ALPHATEST_ON 1
				true:ShowPort:ForwardOnly:Alpha Clip Threshold
				false:RemoveDefine:ForwardOnly:SHADERPASS_FORWARD_BYPASS_ALPHA_TEST 1
				false:RemoveDefine:_ALPHATEST_ON 1
				false:HidePort:ForwardOnly:Alpha Clip Threshold
			Option:Double-Sided:Disabled,Enabled,Flipped Normals,Mirrored Normals:Disabled
				Disabled:RemoveDefine:ASE_NEED_CULLFACE 1
				Enabled,Flipped Normals,Mirrored Normals:SetDefine:ASE_NEED_CULLFACE 1
				Enabled,Flipped Normals,Mirrored Normals:SetShaderProperty:_DoubleSidedEnable,1
				Flipped Normals:SetShaderProperty:_DoubleSidedNormalMode,0
				Mirrored Normals:SetShaderProperty:_DoubleSidedNormalMode,1
				Mirrored Normals:SetShaderProperty:_StencilRefDepth,32
				Mirrored Normals:SetShaderProperty:_StencilRefGBuffer,1
			Option:Energy Conserving Specular:false,true:true
				true:SetDefine:_ENERGY_CONSERVING_SPECULAR 1
				false,disable:RemoveDefine:_ENERGY_CONSERVING_SPECULAR 1
			Option:Material Type:Cotton Wool,Silk:Cotton Wool
				Silk:ShowPort:ForwardOnly:Anisotropy
				Silk:ShowPort:ForwardOnly:Tangent
				Silk:RemoveDefine:_MATERIAL_FEATURE_COTTON_WOOL 1
				Silk:SetDefine:_MATERIAL_FEATURE_ANISOTROPY 1
				Cotton Wool:HidePort:ForwardOnly:Anisotropy
				Cotton Wool:HidePort:ForwardOnly:Tangent
				Cotton Wool:SetDefine:_MATERIAL_FEATURE_COTTON_WOOL 1
				Cotton Wool:RemoveDefine:_MATERIAL_FEATURE_ANISOTROPY 1
			Option:Transmission:false,true:false
				true:ShowPort:ForwardOnly:Thickness
				true:SetDefine:_MATERIAL_FEATURE_TRANSMISSION 1
				false:HidePort:ForwardOnly:Thickness
				false:RemoveDefine:_MATERIAL_FEATURE_TRANSMISSION 1
			Option:Receive Decals:false,true:true
				true:RemoveDefine:_DISABLE_DECALS 1
				false:SetDefine:_DISABLE_DECALS 1
			Option:Receives SSR:false,true:true
				false:SetDefine:_DISABLE_SSR 1
				false:SetShaderProperty:_StencilRefDepth,32
				false:SetShaderProperty:_StencilRefGBuffer,34
				false:SetShaderProperty:_StencilRefMV,160
				true:RemoveDefine:_DISABLE_SSR 1
				true:SetShaderProperty:_ReceivesSSR,1
			Option:Add Precomputed Velocity:false,true:false
				false:RemoveDefine:_ADD_PRECOMPUTED_VELOCITY 1
				false:SetShaderProperty:_AddPrecomputedVelocity,[HideInInspector] [ToggleUI] _AddPrecomputedVelocityD("Add Precomputed Velocity", Float) = 1
				true:SetDefine:_ADD_PRECOMPUTED_VELOCITY 1
			Option:Specular Occlusion Mode:Off,From AO,From AO And Bent Normal,Custom:Off
				Off:RemoveDefine:_SPECULAR_OCCLUSION_FROM_AO 1
				Off:RemoveDefine:_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
				Off:RemoveDefine:_SPECULAR_OCCLUSION_CUSTOM 1
				Off:HidePort:Specular Occlusion
				From AO:SetDefine:_SPECULAR_OCCLUSION_FROM_AO 1
				From AO:RemoveDefine:_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
				From AO:RemoveDefine:_SPECULAR_OCCLUSION_CUSTOM 1
				From AO:HidePort:Specular Occlusion
				From AO And Bent Normal:RemoveDefine:_SPECULAR_OCCLUSION_FROM_AO 1
				From AO And Bent Normal:SetDefine:_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
				From AO And Bent Normal:RemoveDefine:_SPECULAR_OCCLUSION_CUSTOM 1
				From AO And Bent Normal:HidePort:Specular Occlusion
				Custom:RemoveDefine:_SPECULAR_OCCLUSION_FROM_AO 1
				Custom:RemoveDefine:_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
				Custom:SetDefine:_SPECULAR_OCCLUSION_CUSTOM 1
				Custom:ShowPort:Specular Occlusion
			Option:Override Baked GI:false,true:false
				true:ShowPort:ForwardOnly:Baked GI
				true:ShowPort:ForwardOnly:Baked Back GI
				false:HidePort:ForwardOnly:Baked GI
				false:HidePort:ForwardOnly:Baked Back GI
			Option:Depth Offset:false,true:false
				true:SetDefine:_DEPTHOFFSET_ON 1
				true:ShowPort:ForwardOnly:DepthOffset
				false:RemoveDefine:_DEPTHOFFSET_ON 1
				false:HidePort:ForwardOnly:DepthOffset
			Option:Support LOD CrossFade:false,true:false
				true:SetDefine:pragma multi_compile _ LOD_FADE_CROSSFADE
				false:RemoveDefine:pragma multi_compile _ LOD_FADE_CROSSFADE
			Option:Vertex Position:Absolute,Relative:Relative
				Absolute:SetDefine:ASE_ABSOLUTE_VERTEX_POS 1
				Absolute:SetPortName:ForwardOnly:18,Vertex Position
				Relative:RemoveDefine:ASE_ABSOLUTE_VERTEX_POS 1
				Relative:SetPortName:ForwardOnly:18,Vertex Offset
		*/
		Tags
		{
			"RenderPipeline"="HDRenderPipeline"
			"RenderType"="HDLitShader"
			"Queue"="Geometry+0"
		}

		HLSLINCLUDE
		#pragma target 4.5
		#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer
		ENDHLSL

		/*ase_pass*/
		Pass
		{
			/*ase_main_pass*/
			Name "ForwardOnly"
			Tags { "LightMode" = "ForwardOnly" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			Stencil
			{
				WriteMask [_StencilWriteMask]
				Ref [_StencilRef]
				Comp Always
				Pass Replace
			}

			HLSLPROGRAM

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#define SHADERPASS SHADERPASS_FORWARD
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
			#pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH SHADOW_VERY_HIGH

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"

			#define HAS_LIGHTLOOP

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Fabric/Fabric.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			/*ase_pragma*/

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float3 Albedo;
				float3 Normal;
				float3 BentNormal;
				float Smoothness;
				float Occlusion;
				float3 Specular;
				float3 Emission;
				float Alpha;
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
					float DiffusionProfile;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					float SubsurfaceMask;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
					float Thickness;
				#endif
				#ifdef _ALPHATEST_ON
					float AlphaClipThreshold;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
					float Anisotropy;
					float3 Tangent;
				#endif
			};

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				/*ase_vdata:p=p;n=n;t=t;uv1=tc1;uv2=tc2*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 interp03 : TEXCOORD3;
				float4 interp04 : TEXCOORD4;
				/*ase_interp(5,):sp=sp.xyzw;rwp=tc0;wn=tc1;wt=tc2;uv1=tc3;uv2=tc4*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
            float4 _EmissionColor;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
            float _StencilRef;
            float _StencilWriteMask;
            float _StencilRefDepth;
            float _StencilWriteMaskDepth;
            float _StencilRefMV;
            float _StencilWriteMaskMV;
            float _StencilRefDistortionVec;
            float _StencilWriteMaskDistortionVec;
            float _StencilWriteMaskGBuffer;
            float _StencilRefGBuffer;
            float _ZTestGBuffer;
            float _RequireSplitLighting;
            float _ReceivesSSR;
            float _SurfaceType;
            float _BlendMode;
            float _SrcBlend;
            float _DstBlend;
            float _AlphaSrcBlend;
            float _AlphaDstBlend;
            float _ZWrite;
            float _CullMode;
            float _TransparentSortPriority;
            float _CullModeForward;
            float _TransparentCullMode;
            float _ZTestDepthEqualForOpaque;
            float _ZTestTransparent;
            float _TransparentBackfaceEnable;
            float _AlphaCutoffEnable;
            float _UseShadowThreshold;
            float _DoubleSidedEnable;
            float _DoubleSidedNormalMode;
            float4 _DoubleSidedConstants;
            CBUFFER_END

			/*ase_globals*/

			/*ase_funcs*/

			void ApplyDecalToSurfaceData( DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData )
            {
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_DIFFUSE)
                {
                    surfaceData.baseColor.xyz = surfaceData.baseColor.xyz * decalSurfaceData.baseColor.w + decalSurfaceData.baseColor.xyz;
                }
        
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_NORMAL)
                {
                    surfaceData.normalWS.xyz = normalize(surfaceData.normalWS.xyz * decalSurfaceData.normalWS.w + decalSurfaceData.normalWS.xyz);
                }
        
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_MASK)
                {
					#ifdef DECALS_4RT
						surfaceData.ambientOcclusion = surfaceData.ambientOcclusion * decalSurfaceData.MAOSBlend.y + decalSurfaceData.mask.y;
					#endif
        
                    surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
                }
            }

			void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );

				surfaceData.specularOcclusion = 1.0;
				surfaceData.baseColor = surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion = surfaceDescription.Occlusion;
				surfaceData.specularColor = surfaceDescription.Specular;
				#if defined (_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) || defined (_MATERIAL_FEATURE_TRANSMISSION)
					surfaceData.diffusionProfileHash = asuint( surfaceDescription.DiffusionProfile );
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.subsurfaceMask = surfaceDescription.SubsurfaceMask;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.thickness = surfaceDescription.Thickness;
				#endif

				surfaceData.materialFeatures = 0;

				#ifdef _MATERIAL_FEATURE_COTTON_WOOL
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_COTTON_WOOL;
					surfaceData.perceptualSmoothness = lerp(0.0, 0.6, surfaceDescription.Smoothness);
				#endif
        
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_SUBSURFACE_SCATTERING;
				#endif
        
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_TRANSMISSION;
				#endif

				#if defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
				#endif
        
				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif

                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy = surfaceDescription.Anisotropy;
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
        
				#if HAVE_DECALS
					if (_EnableDecals)
					{
						DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
						ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
					}
				#endif
        
                bentNormalWS = surfaceData.normalWS;
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif
        
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );
        
				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
					surfaceData.specularOcclusion = surfaceDescription.SpecularOcclusion;
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
				#endif
        
				#ifdef DEBUG_DISPLAY
					ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription,FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif
        
				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
				#if _ALPHATEST_ON
					DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
				#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS Vert(AttributesMesh inputMesh /*ase_vert_input*/)
			{

				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				/*ase_vert_code:inputMesh=AttributesMesh;outputPackedVaryingsMeshToPS=PackedVaryingsMeshToPS*/

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = /*ase_vert_out:Vertex Offset;Float3;18;-1;_VertexOffset*/defaultVertexValue/*end*/;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = /*ase_vert_out:Vertex Normal;Float3;19;-1;_VertexNormal*/inputMesh.normalOS/*end*/;
				inputMesh.tangentOS = /*ase_vert_out:Vertex Tangent;Float4;20;-1;_VertexTangent*/inputMesh.tangentOS/*end*/;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );
				return outputPackedVaryingsMeshToPS;
			}

			void Frag(PackedVaryingsMeshToPS packedInput,
				#ifdef OUTPUT_SPLIT_LIGHTING
					out float4 outColor : SV_Target0,
					out float4 outDiffuseLighting : SV_Target1,
					OUTPUT_SSSBUFFER(outSSSBuffer)
				#else
					out float4 outColor : SV_Target0
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					, out float4 outMotionVec : SV_Target1
				#endif
				#endif
				#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
				#endif
				/*ase_frag_input*/
				)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				/*ase_local_var:rwp*/float3 positionRWS = packedInput.interp00.xyz;
				/*ase_local_var:wn*/float3 normalWS = packedInput.interp01.xyz;
				/*ase_local_var:wt*/float4 tangentWS = packedInput.interp02.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				/*ase_local_var:vf*/half isFrontFace = input.isFrontFace;

				input.positionSS.xy = _OffScreenRendering > 0 ? ( input.positionSS.xy * _OffScreenDownsampleFactor ) : input.positionSS.xy;

				uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize ();

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex );

				/*ase_local_var:wvd*/float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				/*ase_frag_code:packedInput=PackedVaryingsMeshToPS*/
				surfaceDescription.Albedo = /*ase_frag_out:Albedo;Float3;0;-1;_Albedo*/float3( 0.5, 0.5, 0.5 )/*end*/;
				surfaceDescription.Normal = /*ase_frag_out:Normal;Float3;1;-1;_Normal*/float3( 0, 0, 1 )/*end*/;
				surfaceDescription.BentNormal = /*ase_frag_out:Bent Normal;Float3;2;-1;_BentNormal*/float3( 0, 0, 1 )/*end*/;
				surfaceDescription.Specular = /*ase_frag_out:Specular;Float3;3;-1;_Specular*/0/*end*/;

				surfaceDescription.Emission = /*ase_frag_out:Emission;Float3;4;-1;_Emission*/0/*end*/;
				surfaceDescription.Smoothness = /*ase_frag_out:Smoothness;Float;5;-1;_Smoothness*/0.5/*end*/;
				surfaceDescription.Occlusion = /*ase_frag_out:Occlusion;Float;6;-1;_Occlusion*/1/*end*/;
				surfaceDescription.Alpha = /*ase_frag_out:Alpha;Float;7;-1;_Alpha*/1/*end*/;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = /*ase_frag_out:Alpha Clip Threshold;Float;8;-1;_AlphaClip*/0.5/*end*/;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = /*ase_frag_out:Specular Occlusion;Float;9;-1;_SpecularOcclusion*/0/*end*/;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceDescription.Thickness = /*ase_frag_out:Thickness;Float;10;-1;_Thickness*/1/*end*/;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = /*ase_frag_out:Subsurface Mask;Float;11;-1;_SubsurfaceMask*/1/*end*/;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = /*ase_frag_out:Diffusion Profile;Float;12;-1;_DiffusionProfile*/0/*end*/;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = /*ase_frag_out:Anisotropy;Float;13;-1;_Anisotropy*/0.8/*end*/;
				surfaceDescription.Tangent = /*ase_frag_out:Tangent;Float3;14;-1;_Tangent*/float3( 1, 0, 0 )/*end*/;
				#endif

				#ifdef _ASE_BAKEDGI
				surfaceDescription.BakedGI = /*ase_frag_out:Baked GI;Float3;15;-1;_BakedGI*/0/*end*/;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = /*ase_frag_out:Baked Back GI;Float3;16;-1;_BakedBackGI*/0/*end*/;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = /*ase_frag_out:DepthOffset;Float;17;-1;_DepthOffset*/0/*end*/;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

				PreLightData preLightData = GetPreLightData(V, posInput, bsdfData);

				outColor = float4(0.0, 0.0, 0.0, 0.0);
				{
					#ifdef _SURFACE_TYPE_TRANSPARENT
					uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
					#else
					uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
					#endif
					float3 diffuseLighting;
					float3 specularLighting;

					LightLoop( V, posInput, preLightData, bsdfData, builtinData, featureFlags, diffuseLighting, specularLighting );

					diffuseLighting *= GetCurrentExposureMultiplier();
					specularLighting *= GetCurrentExposureMultiplier();

					#ifdef OUTPUT_SPLIT_LIGHTING
					if( _EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting( bsdfData ) )
					{
						outColor = float4( specularLighting, 1.0 );
						outDiffuseLighting = float4( TagLightingForSSS( diffuseLighting ), 1.0 );
					}
					else
					{
						outColor = float4( diffuseLighting + specularLighting, 1.0 );
						outDiffuseLighting = 0;
					}
					ENCODE_INTO_SSSBUFFER( surfaceData, posInput.positionSS, outSSSBuffer );
					#else
					outColor = ApplyBlendMode( diffuseLighting, specularLighting, builtinData.opacity );
					outColor = EvaluateAtmosphericScattering( posInput, V, outColor );
					#endif
					#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					/*VaryingsPassToPS inputPass = UnpackVaryingsPassToPS(packedInput.vpass);
					bool forceNoMotion = any(unity_MotionVectorsParams.yw == 0.0);
					if (forceNoMotion)
					{
						outMotionVec = float4(2.0, 0.0, 0.0, 0.0);
					}
					else
					{
						float2 motionVec = CalculateMotionVector(inputPass.positionCS, inputPass.previousPositionCS);
						EncodeMotionVector(motionVec * 0.5, outMotionVec);
						outMotionVec.zw = 1.0;
					}*/
					#endif
				}

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		/*ase_pass*/
        Pass
        {
			/*ase_hide_pass*/
            Name "DepthForwardOnly"
            Tags { "LightMode" = "DepthForwardOnly" }
			
			Cull [_CullMode]
            ZWrite On
			Stencil
			{
			   WriteMask [_StencilWriteMaskDepth]
			   Ref [_StencilRefDepth]
			   Comp Always
			   Pass Replace
			}
        
            HLSLPROGRAM

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

			#pragma vertex Vert
			#pragma fragment Frag
        
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH
			#define RAYTRACING_SHADER_GRAPH_HIGH
        
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Fabric/Fabric.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
			/*ase_pragma*/

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			int _ObjectId;
			int _PassValue;
			
			struct SurfaceDescription
			{
				float Alpha;
				#ifdef _ALPHATEST_ON
				float AlphaClipThreshold;
				#endif
				float3 Normal;
				float Smoothness;
			};

			struct VertexInput 
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				/*ase_vdata:p=p;n=n;t=t*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
        
			struct VertexOutput 
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				/*ase_interp(2,):sp=sp.xyzw;rwp=tc0;wn=tc1*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
            float4 _EmissionColor;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
            float _StencilRef;
            float _StencilWriteMask;
            float _StencilRefDepth;
            float _StencilWriteMaskDepth;
            float _StencilRefMV;
            float _StencilWriteMaskMV;
            float _StencilRefDistortionVec;
            float _StencilWriteMaskDistortionVec;
            float _StencilWriteMaskGBuffer;
            float _StencilRefGBuffer;
            float _ZTestGBuffer;
            float _RequireSplitLighting;
            float _ReceivesSSR;
            float _SurfaceType;
            float _BlendMode;
            float _SrcBlend;
            float _DstBlend;
            float _AlphaSrcBlend;
            float _AlphaDstBlend;
            float _ZWrite;
            float _CullMode;
            float _TransparentSortPriority;
            float _CullModeForward;
            float _TransparentCullMode;
            float _ZTestDepthEqualForOpaque;
            float _ZTestTransparent;
            float _TransparentBackfaceEnable;
            float _AlphaCutoffEnable;
            float _UseShadowThreshold;
            float _DoubleSidedEnable;
            float _DoubleSidedNormalMode;
            float4 _DoubleSidedConstants;
            CBUFFER_END
			/*ase_globals*/
				
			/*ase_funcs*/

			void ApplyDecalToSurfaceData( DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData )
            {
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_DIFFUSE)
                {
                    surfaceData.baseColor.xyz = surfaceData.baseColor.xyz * decalSurfaceData.baseColor.w + decalSurfaceData.baseColor.xyz;
                }
        
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_NORMAL)
                {
                    surfaceData.normalWS.xyz = normalize(surfaceData.normalWS.xyz * decalSurfaceData.normalWS.w + decalSurfaceData.normalWS.xyz);
                }
        
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_MASK)
                {
					#ifdef DECALS_4RT
						surfaceData.ambientOcclusion = surfaceData.ambientOcclusion * decalSurfaceData.MAOSBlend.y + decalSurfaceData.mask.y;
					#endif
        
                    surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
                }
            }
			
			void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData )
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );

				surfaceData.specularOcclusion = 1.0;
				surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;

				surfaceData.materialFeatures = 0;

				#ifdef _MATERIAL_FEATURE_COTTON_WOOL
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_COTTON_WOOL;
					surfaceData.perceptualSmoothness = lerp(0.0, 0.6, surfaceDescription.Smoothness);
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_SUBSURFACE_SCATTERING;
				#endif
        
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_TRANSMISSION;
				#endif

				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif

                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
				#if HAVE_DECALS
					if (_EnableDecals)
					{
						DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
						ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
					}
				#endif
    
				#ifdef DEBUG_DISPLAY
					ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription,FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif
        
				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#if _ALPHATEST_ON
					DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
				#endif
        
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData );
        
				ZERO_INITIALIZE( BuiltinData, builtinData );
				builtinData.opacity = surfaceDescription.Alpha;
			}

			VertexOutput Vert( VertexInput inputMesh /*ase_vert_input*/ )
			{
				VertexOutput o;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);

				/*ase_vert_code:inputMesh=VertexInput;o=VertexOutput*/
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  /*ase_vert_out:Vertex Offset;Float3;4;-1;_VertexOffset*/defaultVertexValue/*end*/;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = /*ase_vert_out:Vertex Normal;Float3;5;-1;_VertexNormal*/inputMesh.normalOS/*end*/;
				inputMesh.tangentOS = /*ase_vert_out:Vertex Tangent;Float4;6;-1;_VertexTangent*/inputMesh.tangentOS/*end*/;

				float3 positionRWS = TransformObjectToWorld( inputMesh.positionOS );
				float3 normalWS = TransformObjectToWorldNormal( inputMesh.normalOS );

				o.positionCS = TransformWorldToHClip( positionRWS );
				o.interp00.xyz = positionRWS;
				o.interp01.xyz = normalWS;

				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				return o;
			}

			void Frag( VertexOutput packedInput
					#ifdef WRITE_NORMAL_BUFFER
					, out float4 outNormalBuffer : SV_Target0
					#ifdef WRITE_MSAA_DEPTH
					, out float1 depthColor : SV_Target1
					#endif
					#elif defined(WRITE_MSAA_DEPTH)
					, out float4 outNormalBuffer : SV_Target0
					, out float1 depthColor : SV_Target1
					#else
					, out float4 outColor : SV_Target0
					#endif

					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					/*ase_frag_input*/
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );

				/*ase_local_var:rwp*/float3 positionRWS = packedInput.interp00.xyz;
				/*ase_local_var:wn*/float3 normalWS = packedInput.interp01.xyz;

				FragInputs input;
				ZERO_INITIALIZE( FragInputs, input );
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld( float4( 1, 0, 0, 0 ), normalWS );

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif

				/*ase_local_var:vf*/half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				/*ase_local_var:wvd*/float3 V = GetWorldSpaceNormalizeViewDir( input.positionRWS );

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				/*ase_frag_code:packedInput=VertexOutput*/
				surfaceDescription.Normal = /*ase_frag_out:Normal;Float3;0;-1;_Normal*/float3( 0, 0, 1 )/*end*/;
				surfaceDescription.Smoothness = /*ase_frag_out:Smoothness;Float;1;-1;_Smoothness*/0.5/*end*/;
				surfaceDescription.Alpha = /*ase_frag_out:Alpha;Float;2;-1;_Alpha*/1/*end*/;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = /*ase_frag_out:Alpha Clip Threshold;Float;3;-1;_AlphaClip*/0.5/*end*/;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
					#ifdef WRITE_MSAA_DEPTH
					depthColor = packedInput.positionCS.z;
					#endif
				#elif defined(WRITE_MSAA_DEPTH)
					outNormalBuffer = float4(0.0, 0.0, 0.0, 1.0);
					depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
					outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
				#else
					outColor = float4(0.0, 0.0, 0.0, 0.0);
				#endif
			}
        
            ENDHLSL
        }

		/*ase_pass*/
        Pass
        {
			/*ase_hide_pass*/
			Name "SceneSelectionPass"
			Tags { "LightMode" = "SceneSelectionPass" }
			
			Cull [_CullMode]
            ZWrite On

			ColorMask 0
        
            HLSLPROGRAM

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

			#pragma vertex Vert
			#pragma fragment Frag
        
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS
			#pragma editor_sync_compilation
        
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Fabric/Fabric.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
			/*ase_pragma*/
			
			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				#ifdef _ALPHATEST_ON
				float AlphaClipThreshold;
				#endif
			};

			struct VertexInput 
			{
				float3 positionOS : POSITION;
				float4 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				/*ase_vdata:p=p;n=n;t=t*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
        
			struct VertexOutput 
			{
				float4 positionCS : SV_Position;
				/*ase_interp(0,):sp=sp.xyzw*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
            float4 _EmissionColor;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
            float _StencilRef;
            float _StencilWriteMask;
            float _StencilRefDepth;
            float _StencilWriteMaskDepth;
            float _StencilRefMV;
            float _StencilWriteMaskMV;
            float _StencilRefDistortionVec;
            float _StencilWriteMaskDistortionVec;
            float _StencilWriteMaskGBuffer;
            float _StencilRefGBuffer;
            float _ZTestGBuffer;
            float _RequireSplitLighting;
            float _ReceivesSSR;
            float _SurfaceType;
            float _BlendMode;
            float _SrcBlend;
            float _DstBlend;
            float _AlphaSrcBlend;
            float _AlphaDstBlend;
            float _ZWrite;
            float _CullMode;
            float _TransparentSortPriority;
            float _CullModeForward;
            float _TransparentCullMode;
            float _ZTestDepthEqualForOpaque;
            float _ZTestTransparent;
            float _TransparentBackfaceEnable;
            float _AlphaCutoffEnable;
            float _UseShadowThreshold;
            float _DoubleSidedEnable;
            float _DoubleSidedNormalMode;
            float4 _DoubleSidedConstants;
            CBUFFER_END
			/*ase_globals*/
				
			/*ase_funcs*/

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
			}
        
			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{ 
				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif
        
				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif

                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);

				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;
			}

			VertexOutput Vert( VertexInput inputMesh /*ase_vert_input*/ )
			{
				VertexOutput o;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);

				/*ase_vert_code:inputMesh=VertexInput;o=VertexOutput*/
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = /*ase_vert_out:Vertex Offset;Float3;2;-1;_VertexOffset*/defaultVertexValue/*end*/;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = /*ase_vert_out:Vertex Normal;Float3;3;-1;_VertexNormal*/inputMesh.normalOS/*end*/;
				inputMesh.tangentOS = /*ase_vert_out:Vertex Tangent;Float4;4;-1;_VertexTangent*/inputMesh.tangentOS/*end*/;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);  
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				return o;
			}

			void Frag( VertexOutput packedInput
					, out float4 outColor : SV_Target0
					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					/*ase_frag_input*/
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif

				/*ase_local_var:vf*/half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3(1.0, 1.0, 1.0); // Avoid the division by 0

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				/*ase_frag_code:packedInput=VertexOutput*/
				surfaceDescription.Alpha = /*ase_frag_out:Alpha;Float;0;-1;_Alpha*/1/*end*/;
				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold =  /*ase_frag_out:Alpha Clip Threshold;Float;1;-1;_AlphaClip*/0.5/*end*/;
				#endif
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
			}
        
            ENDHLSL
        }

		/*ase_pass*/
        Pass
        {
			/*ase_hide_pass*/
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
			Blend One Zero
			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
            ColorMask 0
        
            HLSLPROGRAM
			
			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
			#define SHADERPASS SHADERPASS_SHADOWS

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Fabric/Fabric.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
			/*ase_pragma*/
        
			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				#ifdef _ALPHATEST_ON
				float AlphaClipThreshold;
				#endif
			};

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				/*ase_vdata:p=p;n=n;t=t*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
        
			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				/*ase_interp(0,):sp=sp.xyzw*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
            float4 _EmissionColor;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
            float _StencilRef;
            float _StencilWriteMask;
            float _StencilRefDepth;
            float _StencilWriteMaskDepth;
            float _StencilRefMV;
            float _StencilWriteMaskMV;
            float _StencilRefDistortionVec;
            float _StencilWriteMaskDistortionVec;
            float _StencilWriteMaskGBuffer;
            float _StencilRefGBuffer;
            float _ZTestGBuffer;
            float _RequireSplitLighting;
            float _ReceivesSSR;
            float _SurfaceType;
            float _BlendMode;
            float _SrcBlend;
            float _DstBlend;
            float _AlphaSrcBlend;
            float _AlphaDstBlend;
            float _ZWrite;
            float _CullMode;
            float _TransparentSortPriority;
            float _CullModeForward;
            float _TransparentCullMode;
            float _ZTestDepthEqualForOpaque;
            float _ZTestTransparent;
            float _TransparentBackfaceEnable;
            float _AlphaCutoffEnable;
            float _UseShadowThreshold;
            float _DoubleSidedEnable;
            float _DoubleSidedNormalMode;
            float4 _DoubleSidedConstants;
            CBUFFER_END
			/*ase_globals*/
				
			/*ase_funcs*/
			    
			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
			}
        
			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif
        
				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#if _ALPHATEST_ON
					DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
				#endif
        
				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE (BuiltinData, builtinData); 
				builtinData.opacity = surfaceDescription.Alpha;
			}
        
			VertexOutput Vert( VertexInput inputMesh /*ase_vert_input*/ )
			{
				VertexOutput o;
				
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);

				/*ase_vert_code:inputMesh=VertexInput;o=VertexOutput*/
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = /*ase_vert_out:Vertex Offset;Float3;2;-1;_VertexOffset*/defaultVertexValue/*end*/;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = /*ase_vert_out:Vertex Normal;Float3;3;-1;_VertexNormal*/inputMesh.normalOS/*end*/;
				inputMesh.tangentOS = /*ase_vert_out:Vertex Tangent;Float4;4;-1;_VertexTangent*/inputMesh.tangentOS/*end*/;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				return o;
			}

			void Frag( VertexOutput packedInput
					#ifdef WRITE_NORMAL_BUFFER
					, out float4 outNormalBuffer : SV_Target0
					#ifdef WRITE_MSAA_DEPTH
					, out float1 depthColor : SV_Target1
					#endif
					#elif defined(WRITE_MSAA_DEPTH)
					, out float4 outNormalBuffer : SV_Target0
					, out float1 depthColor : SV_Target1
					#else
					, out float4 outColor : SV_Target0
					#endif

					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					/*ase_frag_input*/ 
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif

				/*ase_local_var:vf*/half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				/*ase_frag_code:packedInput=VertexOutput*/
				surfaceDescription.Alpha = /*ase_frag_out:Alpha;Float;0;-1;_Alpha*/1/*end*/;
				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = /*ase_frag_out:Alpha Clip Threshold;Float;1;-1;_AlphaClip*/0.5/*end*/;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH) 
				outNormalBuffer = float4(0.0, 0.0, 0.0, 1.0);
				depthColor = packedInput.vmesh.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
				#else
				outColor = float4(0.0, 0.0, 0.0, 0.0);
				#endif
			}

            ENDHLSL
        }

		/*ase_pass*/
		Pass
		{
			/*ase_hide_pass*/
			Name "META"
			Tags { "LightMode" = "Meta" }

			Cull Off

			HLSLPROGRAM

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Fabric/Fabric.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			/*ase_pragma*/

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float3 Albedo;
				float3 Emission;
				float Alpha;
				#ifdef _ALPHATEST_ON
				float AlphaClipThreshold;
				#endif
			};

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				/*ase_vdata:p=p;n=n;t=t;uv1=tc1;uv2=tc2*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				/*ase_interp(0,):sp=sp.xyzw*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			CBUFFER_START( UnityPerMaterial )
            float4 _EmissionColor;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
            float _StencilRef;
            float _StencilWriteMask;
            float _StencilRefDepth;
            float _StencilWriteMaskDepth;
            float _StencilRefMV;
            float _StencilWriteMaskMV;
            float _StencilRefDistortionVec;
            float _StencilWriteMaskDistortionVec;
            float _StencilWriteMaskGBuffer;
            float _StencilRefGBuffer;
            float _ZTestGBuffer;
            float _RequireSplitLighting;
            float _ReceivesSSR;
            float _SurfaceType;
            float _BlendMode;
            float _SrcBlend;
            float _DstBlend;
            float _AlphaSrcBlend;
            float _AlphaDstBlend;
            float _ZWrite;
            float _CullMode;
            float _TransparentSortPriority;
            float _CullModeForward;
            float _TransparentCullMode;
            float _ZTestDepthEqualForOpaque;
            float _ZTestTransparent;
            float _TransparentBackfaceEnable;
            float _AlphaCutoffEnable;
            float _UseShadowThreshold;
            float _DoubleSidedEnable;
            float _DoubleSidedNormalMode;
            float4 _DoubleSidedConstants;
            CBUFFER_END

			CBUFFER_START( UnityMetaPass )
			bool4 unity_MetaVertexControl;
			bool4 unity_MetaFragmentControl;
			CBUFFER_END

			float unity_OneOverOutputBoost;
			float unity_MaxOutputValue;
			/*ase_globals*/

			/*ase_funcs*/

			void BuildSurfaceData( FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData )
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );
				surfaceData.baseColor = surfaceDescription.Albedo;
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData )
			{
				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#if _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData( fragInputs, surfaceDescription, V, surfaceData );
				ZERO_INITIALIZE( BuiltinData, builtinData );
				builtinData.opacity = surfaceDescription.Alpha;
				builtinData.emissiveColor = surfaceDescription.Emission;
			}

			VertexOutput Vert( VertexInput inputMesh /*ase_vert_input*/ )
			{
				VertexOutput o;

				UNITY_SETUP_INSTANCE_ID( inputMesh );
				UNITY_TRANSFER_INSTANCE_ID( inputMesh, o );

				/*ase_vert_code:inputMesh=VertexInput;o=VertexOutput*/
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = /*ase_vert_out:Vertex Offset;Float3;4;-1;_VertexOffset*/defaultVertexValue/*end*/;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = /*ase_vert_out:Vertex Normal;Float3;5;-1;_VertexNormal*/inputMesh.normalOS/*end*/;
				inputMesh.tangentOS = /*ase_vert_out:Vertex Tangent;Float4;6;-1;_VertexTangent*/inputMesh.tangentOS/*end*/;

				float2 uv = float2( 0.0, 0.0 );
				if( unity_MetaVertexControl.x )
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if( unity_MetaVertexControl.y )
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}
				
				o.positionCS = float4( uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0 );
				return o;
			}

			float4 Frag( VertexOutput packedInput /*ase_frag_input*/ ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE( FragInputs, input );
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS );

				float3 V = float3( 1.0, 1.0, 1.0 ); // Avoid the division by 0

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				/*ase_frag_code:packedInput=VertexOutput*/
				surfaceDescription.Albedo = /*ase_frag_out:Color;Float3;0;-1;_Color*/float3( 1, 1, 1 )/*end*/;
				surfaceDescription.Emission = /*ase_frag_out:Emission;Float3;1;-1;_Emission*/0/*end*/;
				surfaceDescription.Alpha = /*ase_frag_out:Alpha;Float;2;-1;_Alpha*/1/*end*/;
				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = /*ase_frag_out:Alpha Clip Threshold;Float;3;-1;_AlphaClip*/0.5/*end*/;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription,input, V, posInput, surfaceData, builtinData );
				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );
				LightTransportData lightTransportData = GetLightTransportData( surfaceData, builtinData, bsdfData );

				float4 res = float4( 0.0, 0.0, 0.0, 1.0 );
				if( unity_MetaFragmentControl.x )
				{
					res.rgb = clamp( pow( abs( lightTransportData.diffuseColor ), saturate( unity_OneOverOutputBoost ) ), 0, unity_MaxOutputValue );
				}

				if( unity_MetaFragmentControl.y )
				{
					res.rgb = lightTransportData.emissiveColor;
				}

				return res;
			}

			ENDHLSL
		}

		/*ase_pass*/
		Pass
		{
			/*ase_hide_pass*/
			Name "MotionVectors"
			Tags { "LightMode" = "MotionVectors" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				WriteMask [_StencilWriteMaskMV]
				Ref [_StencilRefMV]
				Comp Always
				Pass Replace
			}

			HLSLPROGRAM

			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#define SHADERPASS SHADERPASS_MOTION_VECTORS
			#define WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH
			#define RAYTRACING_SHADER_GRAPH_HIGH

			#define HAVE_MESH_MODIFICATION

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Fabric/Fabric.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			/*ase_pragma*/

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct SurfaceDescription
			{
				float Alpha;
				#ifdef _ALPHATEST_ON
				float AlphaClipThreshold;
				#endif
				float3 Normal;
				float Smoothness;
			};

			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float3 previousPositionOS : TEXCOORD4;
				/*ase_vdata:p=p;n=n;t=t*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 vpasspositionCS : TEXCOORD1;
				float3 vpasspreviousPositionCS : TEXCOORD2;
				/*ase_interp(3,):sp=sp.xyzw;rwp=tc0*/
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			void MotionVectorPositionZBias( inout float4 positionCS )
			{
				#if defined(UNITY_REVERSED_Z)
					positionCS.z -= unity_MotionVectorsParams.z * positionCS.w;
				#else
					positionCS.z += unity_MotionVectorsParams.z * positionCS.w;
				#endif
			}

			CBUFFER_START( UnityPerMaterial )
            float4 _EmissionColor;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
            float _StencilRef;
            float _StencilWriteMask;
            float _StencilRefDepth;
            float _StencilWriteMaskDepth;
            float _StencilRefMV;
            float _StencilWriteMaskMV;
            float _StencilRefDistortionVec;
            float _StencilWriteMaskDistortionVec;
            float _StencilWriteMaskGBuffer;
            float _StencilRefGBuffer;
            float _ZTestGBuffer;
            float _RequireSplitLighting;
            float _ReceivesSSR;
            float _SurfaceType;
            float _BlendMode;
            float _SrcBlend;
            float _DstBlend;
            float _AlphaSrcBlend;
            float _AlphaDstBlend;
            float _ZWrite;
            float _CullMode;
            float _TransparentSortPriority;
            float _CullModeForward;
            float _TransparentCullMode;
            float _ZTestDepthEqualForOpaque;
            float _ZTestTransparent;
            float _TransparentBackfaceEnable;
            float _AlphaCutoffEnable;
            float _UseShadowThreshold;
            float _DoubleSidedEnable;
            float _DoubleSidedNormalMode;
            float4 _DoubleSidedConstants;
            CBUFFER_END
			/*ase_globals*/

			/*ase_funcs*/

			void ApplyDecalToSurfaceData( DecalSurfaceData decalSurfaceData, inout SurfaceData surfaceData )
            {
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_DIFFUSE)
                {
                    surfaceData.baseColor.xyz = surfaceData.baseColor.xyz * decalSurfaceData.baseColor.w + decalSurfaceData.baseColor.xyz;
                }
        
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_NORMAL)
                {
                    surfaceData.normalWS.xyz = normalize(surfaceData.normalWS.xyz * decalSurfaceData.normalWS.w + decalSurfaceData.normalWS.xyz);
                }
        
                if (decalSurfaceData.HTileMask & DBUFFERHTILEBIT_MASK)
                {
					#ifdef DECALS_4RT
						surfaceData.ambientOcclusion = surfaceData.ambientOcclusion * decalSurfaceData.MAOSBlend.y + decalSurfaceData.mask.y;
					#endif
        
                    surfaceData.perceptualSmoothness = surfaceData.perceptualSmoothness * decalSurfaceData.mask.w + decalSurfaceData.mask.z;
                }
            }

			void BuildSurfaceData( FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData )
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );

				surfaceData.specularOcclusion = 1.0;
				surfaceData.perceptualSmoothness = surfaceDescription.Smoothness;

				surfaceData.materialFeatures = 0;

				#ifdef _MATERIAL_FEATURE_COTTON_WOOL
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_COTTON_WOOL;
					surfaceData.perceptualSmoothness = lerp(0.0, 0.6, surfaceDescription.Smoothness);
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_SUBSURFACE_SCATTERING;
				#endif
        
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_FABRIC_TRANSMISSION;
				#endif

				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif

                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
				#if HAVE_DECALS
					if (_EnableDecals)
					{
						DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
						ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
					}
				#endif
    
				#ifdef DEBUG_DISPLAY
					ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif
        
				#ifdef _DOUBLESIDED_ON
					float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
				#endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#if _ALPHATEST_ON
					DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
				#endif
        
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData );
        
				ZERO_INITIALIZE( BuiltinData, builtinData );
				builtinData.opacity = surfaceDescription.Alpha;
			}

#if UNITY_VERSION < 201930
			float3 TransformPreviousObjectToWorldNormal(float3 normalOS)
			{
				#ifdef UNITY_ASSUME_UNIFORM_SCALING
				return normalize( mul( (float3x3)unity_MatrixPreviousM, normalOS ) );
				#else
				return normalize( mul( normalOS, (float3x3)unity_MatrixPreviousMI ) );
				#endif
			}

			float3 TransformPreviousObjectToWorld(float3 positionOS)
			{
				float4x4 previousModelMatrix = ApplyCameraTranslationToMatrix(unity_MatrixPreviousM);
				return mul(previousModelMatrix, float4(positionOS, 1.0)).xyz;
			}

#endif
			VertexOutput ApplyMeshModification( inout VertexInput inputMesh /*ase_vert_input*/, float4 timeParams )
			{
				VertexOutput o;

				UNITY_SETUP_INSTANCE_ID( inputMesh );
				UNITY_TRANSFER_INSTANCE_ID( inputMesh, o );

				_Time.y = timeParams.x;

				/*ase_vert_code:inputMesh=VertexInput;o=VertexOutput*/

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = /*ase_vert_out:Vertex Offset;Float3;3;-1;_VertexOffset*/defaultVertexValue/*end*/;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = /*ase_vert_out:Vertex Normal;Float3;4;-1;_VertexNormal*/inputMesh.normalOS/*end*/;

				return o;
			}

			VertexOutput Vert( VertexInput inputMesh )
			{
				VertexOutput o = ApplyMeshModification( inputMesh, float4( _Time.y, 0, 0, 0 ) );

				float3 positionRWS = TransformObjectToWorld( inputMesh.positionOS );
				float4 positionCS = TransformWorldToHClip( positionRWS );


				MotionVectorPositionZBias( positionCS );
				float4 vpasspositionCS = mul( UNITY_MATRIX_UNJITTERED_VP, float4( positionRWS, 1.0));
				float4 vpasspreviousPositionCS = 0;

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					vpasspreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0; // Skin or morph target
					float3 effectivePositionOS = ( hasDeformation ? inputMesh.previousPositionOS : inputMesh.positionOS );
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
						effectivePositionOS -= inputPass.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						VertexInput previousMesh = inputMesh;
						previousMesh.positionOS = effectivePositionOS;

						ApplyMeshModification( previousMesh, _LastTimeParameters );
						
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					//#ifdef ATTRIBUTES_NEED_NORMAL
					//	float3 normalWS = TransformPreviousObjectToWorldNormal(inputMesh.normalOS);
					//#else
					//	float3 normalWS = float3(0.0, 0.0, 0.0);
					//#endif

					//#if defined(HAVE_VERTEX_MODIFICATION)
					//	ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					//#endif

					vpasspreviousPositionCS = mul( UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				o.positionCS = positionCS;
				o.interp00.xyz = positionRWS;

				o.vpasspositionCS = float3( vpasspositionCS.xyw );
				o.vpasspreviousPositionCS = float3( vpasspreviousPositionCS.xyw );

				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				return o;
			}

			void Frag( VertexOutput packedInput
						, out float4 outMotionVector : SV_Target0
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target1
							#ifdef WRITE_MSAA_DEPTH
								, out float1 depthColor : SV_Target2
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target1
						, out float1 depthColor : SV_Target2
						#endif

						#ifdef _DEPTHOFFSET_ON
							, out float outputDepth : SV_Depth
						#endif
						/*ase_frag_input*/
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = packedInput.interp00.xyz;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif

				/*ase_local_var:vf*/half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				/*ase_local_var:wvd*/float3 V = GetWorldSpaceNormalizeViewDir( input.positionRWS );

				SurfaceData surfaceData;
				BuiltinData builtinData;

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				/*ase_frag_code:packedInput=VertexOutput*/
				surfaceDescription.Smoothness = /*ase_frag_out:Smoothness;Float;0;-1;_Smoothness*/1/*end*/;
				surfaceDescription.Alpha = /*ase_frag_out:Alpha;Float;1;-1;_Alpha*/1/*end*/;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = /*ase_frag_out:Alpha Clip Threshold;Float;2;-1;_AlphaClip*/0.5/*end*/;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = /*ase_frag_out:DepthOffset;Float;5;-1;_DepthOffset*/0/*end*/;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );

				float4 positionCS = float4( packedInput.vpasspositionCS.xy, 0.0, packedInput.vpasspositionCS.z );
				float4 previousPositionCS = float4( packedInput.vpasspreviousPositionCS.xy, 0.0, packedInput.vpasspreviousPositionCS.z );
				
				#ifdef _DEPTHOFFSET_ON
				positionCS.w += builtinData.depthOffset;
				previousPositionCS.w += builtinData.depthOffset;
				#endif

				float2 motionVector = CalculateMotionVector( positionCS, previousPositionCS );
				EncodeMotionVector( motionVector * 0.5, outMotionVector );

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if( forceNoMotion )
					outMotionVector = float4( 2.0, 0.0, 0.0, 0.0 );

				#ifdef WRITE_NORMAL_BUFFER
					EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
					#ifdef WRITE_MSAA_DEPTH
						depthColor = packedInput.positionCS.z;
					#endif
				#elif defined(WRITE_MSAA_DEPTH)
					outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
					depthColor = packedInput.positionCS.z;
				#endif

				#ifdef _DEPTHOFFSET_ON
					outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}
		/*ase_pass_end*/
	}
	CustomEditor "UnityEditor.Rendering.HighDefinition.FabricGUI"
	FallBack "Hidden/Shader Graph/FallbackError"
}
