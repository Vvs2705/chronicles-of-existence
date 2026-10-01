// ADR-0008: toon/cel-shading do prototipo de estetica (anime estilizado), URP 17, HLSL a mao (sem Shader Graph).
// Custo (celular, o que o shader faz por pixel): 1 amostra de textura + 1 amostra de sombra (dura, 1 cascata no URP_Base)
// + fog. Ambiente (SH) por vertice. Sem luz adicional, sem normal map, sem especular.
// Passes: ToonForward (UniversalForward), Contorno (SRPDefaultUnlit, casco invertido: +1 draw por objeto; desligar por
// material com Material.SetShaderPassEnabled("SRPDefaultUnlit", false) - ver LookSetup), ShadowCaster, DepthOnly.
// SRP Batcher: todas as propriedades no CBUFFER UnityPerMaterial, identico em todos os passes (HLSLINCLUDE).
// ponytail: so a luz principal ilumina (poste/lanterna nao acendem nada). Luz adicional por vertice entra quando a
// cena da noite pedir, medindo no aparelho.
Shader "COE/Toon"
{
    Properties
    {
        [MainTexture] _BaseMap ("Textura base", 2D) = "white" {}
        [MainColor] _BaseColor ("Cor base", Color) = (1, 1, 1, 1)
        _CorSombra ("Tinta da sombra (multiplica a cor base)", Color) = (0.62, 0.55, 0.78, 1)
        _Limiar ("Limiar luz/sombra (meio-Lambert)", Range(0, 1)) = 0.52
        _LimiarMeio ("Limiar do meio-tom", Range(0, 1)) = 0.3
        _MeioTom ("Forca do meio-tom (0 = duas faixas)", Range(0, 1)) = 0.45
        _Suavidade ("Suavidade da borda da faixa", Range(0.001, 0.5)) = 0.03
        _Ambiente ("Peso do ambiente (trilight)", Range(0, 1)) = 0.35
        _CorRim ("Cor do rim", Color) = (1, 0.93, 0.8, 1)
        _ForcaRim ("Forca do rim", Range(0, 1)) = 0.2
        _PotenciaRim ("Estreiteza do rim", Range(1, 8)) = 4
        _CorContorno ("Cor do contorno", Color) = (0.16, 0.12, 0.14, 1)
        _EspessuraContorno ("Espessura do contorno (clip, fracao da meia altura da tela)", Range(0, 0.02)) = 0.003
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            half4 _BaseColor;
            half4 _CorSombra;
            half _Limiar;
            half _LimiarMeio;
            half _MeioTom;
            half _Suavidade;
            half _Ambiente;
            half4 _CorRim;
            half _ForcaRim;
            half _PotenciaRim;
            half4 _CorContorno;
            float _EspessuraContorno;
        CBUFFER_END

        TEXTURE2D(_BaseMap);
        SAMPLER(sampler_BaseMap);
        ENDHLSL

        Pass
        {
            Name "ToonForward"
            Tags { "LightMode" = "UniversalForward" }
            Cull Back
            ZWrite On

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag
            // Sombra da luz principal. Sem _SHADOWS_SOFT: amostra dura (o URP_Base ja desliga sombra suave).
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 positionWS  : TEXCOORD1;
                half3  normalWS    : TEXCOORD2;
                half4  ambienteFog : TEXCOORD3;   // rgb = ambiente (SH por vertice), a = fator de fog
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                VertexPositionInputs p = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = p.positionCS;
                o.positionWS = p.positionWS;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.ambienteFog = half4(SampleSH(o.normalWS), ComputeFogFactor(p.positionCS.z));
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 base = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * _BaseColor;
                half3 n = normalize(i.normalWS);
                Light luz = GetMainLight(TransformWorldToShadowCoord(i.positionWS), i.positionWS, half4(1, 1, 1, 1));

                // Faixas: sombra (0), meio-tom (_MeioTom) e luz (1), cortadas no meio-Lambert. Sombra projetada apaga a luz.
                half h = dot(n, luz.direction) * 0.5h + 0.5h;
                half faixaLuz = smoothstep(_Limiar - _Suavidade, _Limiar + _Suavidade, h);
                half faixaMeio = smoothstep(_LimiarMeio - _Suavidade, _LimiarMeio + _Suavidade, h) * _MeioTom;
                half aceso = min(max(faixaLuz, faixaMeio), luz.shadowAttenuation);

                half3 cor = base.rgb * lerp(_CorSombra.rgb, luz.color, aceso);
                cor += base.rgb * i.ambienteFog.rgb * _Ambiente;

                half3 olhar = SafeNormalize(GetWorldSpaceViewDir(i.positionWS));
                half rim = PositivePow(1.0h - saturate(dot(n, olhar)), _PotenciaRim) * _ForcaRim;
                cor += _CorRim.rgb * rim * (0.35h + 0.65h * aceso);

                cor = MixFog(cor, i.ambienteFog.a);
                return half4(cor, 1.0h);
            }
            ENDHLSL
        }

        Pass
        {
            // Casco invertido: as costas da malha, infladas na direcao da normal em espaco de clip (espessura constante
            // em pixel). Malha com aresta dura (normal partida) abre fresta no contorno: aceito no prototipo.
            Name "Contorno"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front
            ZWrite On

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex vertContorno
            #pragma fragment fragContorno
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"

            struct AttributesContorno
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct VaryingsContorno
            {
                float4 positionCS : SV_POSITION;
                half   fog        : TEXCOORD0;
            };

            VaryingsContorno vertContorno(AttributesContorno v)
            {
                VaryingsContorno o;
                float4 cs = TransformObjectToHClip(v.positionOS.xyz);
                float3 nWS = TransformObjectToWorldNormal(v.normalOS);
                float2 nCS = mul((float3x3)GetWorldToHClipMatrix(), nWS).xy;
                float comprimento = length(nCS);
                float2 direcao = comprimento > 1e-5 ? nCS / comprimento : float2(0, 0);
                direcao.x *= _ScreenParams.y / _ScreenParams.x;   // mesma espessura em pixel na horizontal e na vertical
                cs.xy += direcao * _EspessuraContorno * cs.w;
                o.positionCS = cs;
                o.fog = ComputeFogFactor(cs.z);
                return o;
            }

            half4 fragContorno(VaryingsContorno i) : SV_Target
            {
                return half4(MixFog(_CorContorno.rgb, i.fog), 1.0h);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex vertSombra
            #pragma fragment fragSombra
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            float3 _LightDirection;   // global do URP (ShadowUtils), fora do CBUFFER do material

            struct AttributesSombra
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            float4 vertSombra(AttributesSombra v) : SV_POSITION
            {
                float3 posWS = TransformObjectToWorld(v.positionOS.xyz);
                float3 nWS = TransformObjectToWorldNormal(v.normalOS);
                float4 cs = TransformWorldToHClip(ApplyShadowBias(posWS, nWS, _LightDirection));
                return ApplyShadowClamping(cs);
            }

            half4 fragSombra(float4 positionCS : SV_POSITION) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex vertProfundidade
            #pragma fragment fragProfundidade

            float4 vertProfundidade(float4 positionOS : POSITION) : SV_POSITION
            {
                return TransformObjectToHClip(positionOS.xyz);
            }

            half fragProfundidade(float4 positionCS : SV_POSITION) : SV_Target
            {
                return positionCS.z;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
