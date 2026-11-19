Shader "Chicken Shooter/Sprite Flash"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FlashAmount ("White Damage Flash", Range(0,1)) = 0
        _UseSpriteColor ("Use Sprite Renderer Color", Float) = 1
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "CanUseSpriteAtlas"="True" }
        Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            struct Attributes { COMMON_2D_INPUTS float4 color: COLOR; };
            struct Varyings { COMMON_2D_OUTPUTS float4 color: COLOR; };
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/2DCommon.hlsl"
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float _FlashAmount;
            float _UseSpriteColor;
            CBUFFER_END
            Varyings vert(Attributes input)
            {
                UNITY_SETUP_INSTANCE_ID(input);
                SetUpSpriteInstanceProperties();
                input.positionOS=UnityFlipSprite(input.positionOS,unity_SpriteProps.xy);
                Varyings output=CommonUnlitVertex(input);
                output.color=input.color*_Color*lerp(float4(1,1,1,1),unity_SpriteColor,_UseSpriteColor);
                return output;
            }
            float4 frag(Varyings input): SV_Target
            { float4 color=CommonUnlitFragment(input,input.color);color.rgb=lerp(color.rgb,float3(1,1,1),_FlashAmount);return color; }
            ENDHLSL
        }
    }
}
