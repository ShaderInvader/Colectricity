Shader "Hidden/Shader/ScreenSplit"
{
    Properties
    {
        // This property is necessary to make the CommandBuffer.Blit bind the source texture to _MainTex
        _MainTex("Main Texture", 2DArray) = "grey" {}
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // List of properties to control your post process effect
    float2 _SplitAxis;
    TEXTURE2D_X(_MainTex);
    TEXTURE2D(_SecondTex);

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        // Note that if HDUtils.DrawFullScreen is used to render the post process, use ClampAndScaleUVForBilinearPostProcessTexture(input.texcoord.xy) to get the correct UVs

        float3 sourceColor = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord).xyz;
        float3 secondColor = SAMPLE_TEXTURE2D(_SecondTex, s_linear_clamp_sampler, input.texcoord).xyz;

        // Apply split screen lerp
        float2 _MiddlePoint = float2(0.5, 0.5);
        _SplitAxis += _MiddlePoint;
        _SplitAxis = normalize(_SplitAxis);
        float lerpValue = step(0.0, (input.texcoord.y - _MiddlePoint.y) * (_SplitAxis.x - _MiddlePoint.x) - (_SplitAxis.y - _MiddlePoint.y) * (input.texcoord.x - _MiddlePoint.x));
        //float lerpValue = (input.texcoord.y - 0.5) * (_SplitAxis.x - 0.5) - (_SplitAxis.y - 0.5) * (input.texcoord.x - 0.5);
        //float lerpValue = step(0.5, saturate((_SplitAxis.x * (input.texcoord.x - 0.5) - (input.texcoord.y - 0.5))));// step(input.texcoord.x, _SplitAxis.x* input.texcoord.x);// *step(input.texcoord.y, _SplitAxis.x);
        float3 finalColor = lerp(sourceColor, secondColor, lerpValue);

        return float4(finalColor, 1);
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "ScreenSplit"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}