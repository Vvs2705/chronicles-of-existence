// ADR-0008: ceu pintado de Auren (skybox em gradiente de tres cores + brilho do sol). Custo: so aritmetica, sem textura.
// A cor do horizonte e a cor da fog (LookSetup): a vila some na mesma tinta do ceu.
Shader "COE/Ceu"
{
    Properties
    {
        _CorTopo ("Topo", Color) = (0.44, 0.64, 0.85, 1)
        _CorHorizonte ("Horizonte (= cor da fog)", Color) = (0.96, 0.82, 0.65, 1)
        _CorBase ("Abaixo do horizonte", Color) = (0.73, 0.64, 0.56, 1)
        _Curva ("Curva do gradiente (menor = horizonte mais alto)", Range(0.1, 4)) = 0.6
        _CorSol ("Brilho do sol", Color) = (1, 0.89, 0.69, 1)
        _TamanhoSol ("Estreiteza do brilho do sol", Range(1, 256)) = 24
    }

    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" "RenderPipeline" = "UniversalPipeline" }
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _CorTopo;
                half4 _CorHorizonte;
                half4 _CorBase;
                half _Curva;
                half4 _CorSol;
                half _TamanhoSol;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 direcao : TEXCOORD0; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.direcao = v.positionOS.xyz;   // malha do skybox: posicao = direcao
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 d = normalize(i.direcao);
                half3 cor = lerp(_CorHorizonte.rgb, _CorTopo.rgb, PositivePow(saturate(d.y), _Curva));
                cor = lerp(cor, _CorBase.rgb, saturate(-d.y * 4.0));
                // _MainLightPosition.xyz = direcao PARA a luz principal (zero sem luz: sem brilho).
                cor += _CorSol.rgb * PositivePow(saturate(dot(d, _MainLightPosition.xyz)), _TamanhoSol);
                return half4(cor, 1.0h);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
