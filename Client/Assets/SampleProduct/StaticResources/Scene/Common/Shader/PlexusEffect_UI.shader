Shader "Custom/PlexusEffect_UI"
{
    Properties
    {
        // ノード数は8固定（パフォーマンス最適化）
        _FocusRatio     ("Focus Ratio",        Range(0.0, 1.0))    = 1.0
        _FocusCenter    ("Focus Center (XY: -1 to 1)", Vector)     = (0, 0, 0, 0)
        _NodeRadius     ("Node Radius",        Range(0.001, 0.05)) = 0.012
        _GlowRadius     ("Glow Radius",        Range(0.01, 0.15))  = 0.06
        _EdgeThreshold  ("Edge Threshold",     Range(0.05, 0.6))   = 0.32
        _EdgeWidth      ("Edge Width",         Range(0.0005, 0.01))= 0.0022
        _EdgeFade       ("Edge Fade",          Range(0.0, 1.0))    = 0.6
        _MoveSpeed      ("Move Speed",         Range(0.0, 1.0))    = 0.12
        _MoveScale      ("Move Scale",         Range(0.0, 0.15))   = 0.06
        _NodeColor      ("Node Color",         Color)              = (1.0, 1.0, 1.0, 1.0)
        _EdgeColor      ("Edge Color",         Color)              = (0.85, 0.90, 0.95, 1.0)
        _BgColorA       ("Background Color A (Center)", Color)     = (0.30, 0.36, 0.40, 1.0)
        _BgColorB       ("Background Color B (Edge)",   Color)     = (0.15, 0.20, 0.24, 1.0)
        _GlowIntensity  ("Glow Intensity",     Range(0.0, 3.0))    = 1.8
        _NodeIntensity  ("Node Intensity",     Range(0.0, 10.0))   = 6.0

        // RawImage が内部で参照するテクスチャスロット（空でよい）
        [HideInInspector] _MainTex ("MainTex", 2D) = "white" {}

        // Mask / RectMask2D 対応（uGUI が自動設定する）
        [HideInInspector] _StencilComp     ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil         ("Stencil ID",         Float) = 0
        [HideInInspector] _StencilOp       ("Stencil Operation",  Float) = 0
        [HideInInspector] _StencilReadMask ("Stencil Read Mask",  Float) = 255
        [HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
        [HideInInspector] _ColorMask       ("Color Mask",         Float) = 15
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue"          = "Transparent"
        }

        // Mask コンポーネント対応
        Stencil
        {
            Ref       [_Stencil]
            Comp      [_StencilComp]
            Pass      [_StencilOp]
            ReadMask  [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Pass
        {
            Blend     SrcAlpha OneMinusSrcAlpha
            ZWrite    Off
            ZTest     Always
            Cull      Off
            ColorMask [_ColorMask]

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   3.5

            // RectMask2D / AlphaClip 対応（使わない場合はバリアントを生成しない）
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ノード数定数（変更する場合はここだけ直す）
            #define NODE_COUNT 8

            // ---------------------------------------------------------------
            // CBUFFER（SRP Batcher 対応：マテリアルプロパティを全てまとめる）
            // ---------------------------------------------------------------
            CBUFFER_START(UnityPerMaterial)
                float  _FocusRatio;
                float4 _FocusCenter;
                float  _NodeRadius;
                float  _GlowRadius;
                float  _EdgeThreshold;
                float  _EdgeWidth;
                float  _EdgeFade;
                float  _MoveSpeed;
                float  _MoveScale;
                float4 _NodeColor;
                float4 _EdgeColor;
                float4 _BgColorA;
                float4 _BgColorB;
                float  _GlowIntensity;
                float  _NodeIntensity;
            CBUFFER_END

            // RectMask2D がドローコール単位でセットするグローバルuniform（CBUFFER外）
            float4 _ClipRect;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // ---------------------------------------------------------------
            // Vertex I/O
            // ---------------------------------------------------------------
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
                float2 worldPos    : TEXCOORD1;  // RectMask2D クリップ判定用
            };

            // ---------------------------------------------------------------
            // Hash（sin を使わない整数ベース実装 → モバイル/WebGPU で精度安定）
            // ---------------------------------------------------------------
            float hash11(float n)
            {
                n = frac(n * 0.1031);
                n *= n + 33.33;
                return frac(n * (n + n));
            }

            float hash21(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * float3(0.1031, 0.1030, 0.0973));
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            // ---------------------------------------------------------------
            // Value Noise (1D)
            // ---------------------------------------------------------------
            float vnoise1(float t)
            {
                float i = floor(t);
                float f = frac(t);
                float u = f * f * (3.0 - 2.0 * f);
                return lerp(hash11(i), hash11(i + 1.0), u);
            }

            // ---------------------------------------------------------------
            // ノード位置（focusUV からの相対座標を返す、範囲は概ね [-0.5, 0.5]）
            // ---------------------------------------------------------------
            float2 NodePosition(int idx, float time)
            {
                float fi = float(idx);
                // hash を 0.5 中心にシフトして相対座標化
                float sx = hash21(float2(fi * 3.7, 1.1)) - 0.5;
                float sy = hash21(float2(fi * 2.3, 9.7)) - 0.5;
                float ox = (vnoise1(time * _MoveSpeed + fi * 5.13)         - 0.5) * 2.0;
                float oy = (vnoise1(time * _MoveSpeed + fi * 7.91 + 100.0) - 0.5) * 2.0;
                return float2(sx + ox * _MoveScale, sy + oy * _MoveScale);
            }

            // ---------------------------------------------------------------
            // 点 → 線分 SDF（アスペクト比補正付き）
            // ---------------------------------------------------------------
            float DistToSegment(float2 p, float2 a, float2 b, float aspect)
            {
                float2 pa = (p - a) * float2(aspect, 1.0);
                float2 ba = (b - a) * float2(aspect, 1.0);
                float  h  = saturate(dot(pa, ba) / (dot(ba, ba) + 1e-6));
                return length(pa - ba * h);
            }

            // ---------------------------------------------------------------
            // Vertex Shader
            // ---------------------------------------------------------------
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv          = IN.uv;
                OUT.color       = IN.color;
                OUT.worldPos    = mul(unity_ObjectToWorld, IN.positionOS).xy;
                return OUT;
            }

            // ---------------------------------------------------------------
            // Fragment Shader
            // ---------------------------------------------------------------
            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv    = IN.uv;
                float  time  = _Time.y;

                // アスペクト比はレンダーターゲットから自動取得（手動設定不要）
                float aspect = _ScreenParams.x / max(_ScreenParams.y, 1.0);

                // フォーカス点：UV空間 [0,1] × [0,1]
                // _FocusCenter.xy が (0,0) → 画面中央 (0.5, 0.5)
                // _FocusCenter.xy が (1,1) → 画面の角 (1.0, 1.0)
                float2 focusUV = float2(0.5, 0.5) + _FocusCenter.xy * 0.5;

                // ----- 背景グラデーション（フォーカス点を中心に放射）-----
                float2 center = (uv - focusUV) * float2(aspect, 1.0);
                float  radial = length(center) * 1.6;
                float3 bgColor = lerp(_BgColorA.rgb, _BgColorB.rgb, saturate(radial));

                // ----- ノード位置キャッシュ（NODE_COUNT = 8 固定、O(N) = 8回）-----
                // nodePos = focusUV + 相対位置 × FocusRatio
                //   FocusRatio=1: focusUV を中心に通常通り広がる
                //   FocusRatio=0: 全ノードが focusUV に収束
                //   FocusCenter 変更時: focusUV がピボットごと移動するため常に全ノードに影響する
                float2 nodePos[NODE_COUNT];
                UNITY_LOOP
                for (int i = 0; i < NODE_COUNT; i++)
                {
                    nodePos[i] = focusUV + NodePosition(i, time) * _FocusRatio;
                }

                // ----- エッジ描画（NODE_COUNT=8 → 28ペア固定、ブレーク不要）-----
                float edgeAccum = 0.0;
                UNITY_LOOP
                for (int ei = 0; ei < NODE_COUNT - 1; ei++)
                {
                    UNITY_LOOP
                    for (int ej = ei + 1; ej < NODE_COUNT; ej++)
                    {
                        float2 diff      = (nodePos[ej] - nodePos[ei]) * float2(aspect, 1.0);
                        float  distNodes = length(diff);

                        if (distNodes < _EdgeThreshold)
                        {
                            float edgeFade  = pow(1.0 - saturate(distNodes / _EdgeThreshold), 1.5);
                            float d         = DistToSegment(uv, nodePos[ei], nodePos[ej], aspect);
                            float lineAlpha = 1.0 - smoothstep(_EdgeWidth * 0.5, _EdgeWidth * 1.5, d);
                            edgeAccum += lineAlpha * edgeFade * _EdgeFade;
                        }
                    }
                }
                edgeAccum = saturate(edgeAccum);

                // ----- ノード描画 -----
                float nodeAccum = 0.0;
                float glowAccum = 0.0;
                UNITY_LOOP
                for (int ni = 0; ni < NODE_COUNT; ni++)
                {
                    float2 delta = (uv - nodePos[ni]) * float2(aspect, 1.0);
                    float  d     = length(delta);
                    nodeAccum += 1.0 - smoothstep(_NodeRadius * 0.6, _NodeRadius, d);
                    glowAccum += exp(-d * d / (_GlowRadius * _GlowRadius) * 8.0);
                }
                nodeAccum = saturate(nodeAccum);
                glowAccum = saturate(glowAccum);

                // ----- 合成 -----
                float3 col = bgColor;
                col += _EdgeColor.rgb * edgeAccum * 0.5;
                col += _NodeColor.rgb * glowAccum * _GlowIntensity;
                col += _NodeColor.rgb * nodeAccum * _NodeIntensity;
                col *= IN.color.rgb;

                float alpha = IN.color.a;

                // RectMask2D クリッピング
                #ifdef UNITY_UI_CLIP_RECT
                    float2 m = saturate((_ClipRect.zw - _ClipRect.xy
                        - abs(IN.worldPos.xy * 2.0 - _ClipRect.zw - _ClipRect.xy)) * 4096.0);
                    alpha *= m.x * m.y;
                #endif

                // Mask AlphaClip
                #ifdef UNITY_UI_ALPHACLIP
                    clip(alpha - 0.001);
                #endif

                return float4(col, alpha);
            }

            ENDHLSL
        }
    }

    FallBack Off
}
