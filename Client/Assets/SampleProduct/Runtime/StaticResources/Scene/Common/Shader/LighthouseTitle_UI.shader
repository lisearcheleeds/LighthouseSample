Shader "Custom/LighthouseTitle_UI"
{
    Properties
    {
        // ---------------------------------------------------------------
        // ビーム
        // ---------------------------------------------------------------
        _BeamSpeed      ("Beam Speed",      Range(0.1, 2.0))    = 0.5
        _BeamWidth      ("Beam Width",      Range(0.80, 0.999)) = 0.93   // cos(half-angle)
        _BeamSoftness   ("Beam Softness",   Range(0.01, 0.20))  = 0.09
        _BeamDecay      ("Beam Decay",      Range(0.3, 5.0))    = 1.1
        _BeamColor      ("Beam Color",      Color) = (0.85, 0.90, 0.95, 1.0)
        _HorizonColor   ("Horizon Color",   Color) = (0.85, 0.90, 0.95, 1.0)

        // ---------------------------------------------------------------
        // グロー・強度
        // ---------------------------------------------------------------
        _GlowIntensity      ("Lamp Glow",       Range(0.0, 5.0))  = 2.5
        _HorizonIntensity   ("Horizon Glow",    Range(0.0, 2.0))  = 0.7

        // ---------------------------------------------------------------
        // 波・水面反射
        // ---------------------------------------------------------------
        _WaveSpeed      ("Wave Speed",      Range(0.0, 3.0))    = 1.2
        _WaveStrength   ("Wave Strength",   Range(0.0, 1.0))    = 0.6
        _ReflIntensity  ("Reflection",      Range(0.0, 1.0))    = 0.35

        // ---------------------------------------------------------------
        // 色（PlexusEffect_UI と同パレット）
        // ---------------------------------------------------------------
        _SkyColorTop        ("Sky Top",         Color) = (0.12, 0.17, 0.21, 1.0)
        _SkyColorHorizon    ("Sky Horizon",     Color) = (0.22, 0.28, 0.33, 1.0)
        _SeaColorHorizon    ("Sea Horizon",     Color) = (0.18, 0.23, 0.28, 1.0)
        _SeaColorBottom     ("Sea Bottom",      Color) = (0.07, 0.10, 0.13, 1.0)
        _TowerColor         ("Tower Color",     Color) = (0.06, 0.09, 0.12, 1.0)

        // ---------------------------------------------------------------
        // レイアウト
        // ---------------------------------------------------------------
        _HorizonY       ("Horizon Y",       Range(0.1, 0.7))    = 0.25
        _LighthouseY    ("Lighthouse Y",    Range(0.3, 1.0))    = 0.60

        // ---------------------------------------------------------------
        // RawImage / uGUI Mask 対応
        // ---------------------------------------------------------------
        [HideInInspector] _MainTex          ("MainTex", 2D) = "white" {}
        [HideInInspector] _StencilComp      ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil          ("Stencil ID",         Float) = 0
        [HideInInspector] _StencilOp        ("Stencil Operation",  Float) = 0
        [HideInInspector] _StencilReadMask  ("Stencil Read Mask",  Float) = 255
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _ColorMask        ("Color Mask",         Float) = 15
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue"          = "Transparent"
        }

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

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ---------------------------------------------------------------
            // CBUFFER（SRP Batcher 対応）
            // ---------------------------------------------------------------
            CBUFFER_START(UnityPerMaterial)
                float  _BeamSpeed;
                float  _BeamWidth;
                float  _BeamSoftness;
                float  _BeamDecay;
                float4 _BeamColor;
                float  _GlowIntensity;
                float  _HorizonIntensity;
                float  _WaveSpeed;
                float  _WaveStrength;
                float  _ReflIntensity;
                float4 _SkyColorTop;
                float4 _SkyColorHorizon;
                float4 _SeaColorHorizon;
                float4 _SeaColorBottom;
                float4 _TowerColor;
                float4 _HorizonColor;
                float  _HorizonY;
                float  _LighthouseY;
            CBUFFER_END

            float4 _ClipRect;   // RectMask2D（ドローコール単位、CBUFFER外）

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
                float2 worldPos    : TEXCOORD1;
            };

            // ---------------------------------------------------------------
            // ビーム強度（aspect補正済み2D座標系で計算）
            //   uv      : フラグメントのUV
            //   lampUV  : ランプのUV位置
            //   beamDir : ビーム方向（aspect補正済み空間で正規化済み）
            //   aspect  : アスペクト比
            // ---------------------------------------------------------------
            float BeamIntensity(float2 uv, float2 lampUV, float2 beamDir, float aspect)
            {
                float2 toFrag = float2((uv.x - lampUV.x) * aspect, uv.y - lampUV.y);
                float  dist   = length(toFrag);

                // ビームの「前方」のみ（後方を照らさない）
                float  fwd    = step(0.0, dot(toFrag, beamDir));

                float2 fragDir = toFrag / (dist + 1e-6);
                float  cosA    = dot(fragDir, beamDir);

                float core = smoothstep(_BeamWidth - _BeamSoftness,       _BeamWidth,               cosA);
                float halo = smoothstep(_BeamWidth - _BeamSoftness * 3.0, _BeamWidth - _BeamSoftness, cosA) * 0.22;

                return (core + halo) * exp(-dist * _BeamDecay) * fwd;
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
                float  aspect = _ScreenParams.x / max(_ScreenParams.y, 1.0);

                float horizonY    = _HorizonY;
                float lighthouseY = _LighthouseY;

                // -----------------------------------------------------------
                // 灯台ジオメトリ（_LighthouseY から自動導出、水平線と独立）
                // 全ての幅はスクリーン高さ単位（UV.x への変換 = halfH / aspect）
                // -----------------------------------------------------------
                float lampY      = lighthouseY + 0.13;            // ランプのY位置
                float towerBaseY = max(0.01, lighthouseY - 0.42); // 塔の基底

                float topHalfH  = 0.030;   // 塔頂部の半幅（高さ単位）
                float botHalfH  = 0.115;   // 塔基底の半幅（高さ単位）

                // -----------------------------------------------------------
                // 塔シルエット（台形）
                // -----------------------------------------------------------
                float towerT = saturate((uv.y - towerBaseY) / max(lampY - towerBaseY, 1e-4));
                float halfW  = lerp(botHalfH, topHalfH, towerT) / aspect;

                // エッジを1ピクセル幅でソフト化してジャギーを抑制
                float edgeDist = abs(uv.x - 0.5) - halfW;
                float inTower  = saturate(-edgeDist * _ScreenParams.y * 0.6);
                inTower *= step(towerBaseY, uv.y) * step(uv.y, lampY);

                // 台座（塔基底より少し広く・少し高く）
                float pedHalfW = botHalfH * 1.25 / aspect;
                float pedTop   = towerBaseY + 0.010;
                float pedBot   = towerBaseY - 0.018;
                float inPed    = step(abs(uv.x - 0.5), pedHalfW)
                               * step(pedBot, uv.y) * step(uv.y, pedTop);

                float towerMask = saturate(inTower + inPed);

                // -----------------------------------------------------------
                // 背景グラデーション（空・海）
                // -----------------------------------------------------------
                float skyT  = saturate((uv.y - horizonY) / max(1.0 - horizonY, 1e-4));
                float3 skyCol = lerp(_SkyColorHorizon.rgb, _SkyColorTop.rgb, skyT * skyT);

                float seaT  = saturate((horizonY - uv.y) / max(horizonY, 1e-4));
                float3 seaCol = lerp(_SeaColorHorizon.rgb, _SeaColorBottom.rgb, seaT);

                float  aboveH = step(horizonY, uv.y);
                float3 bgCol  = lerp(seaCol, skyCol, aboveH);

                // -----------------------------------------------------------
                // ビーム方向（sin アニメーション・aspect補正済み空間）
                // swing: -1→左, 0→中央下向き, 1→右
                // -----------------------------------------------------------
                float  swing   = sin(time * _BeamSpeed);
                float2 beamDir = normalize(float2(swing, -0.08));

                float2 lampUV  = float2(0.5, lampY);

                // -----------------------------------------------------------
                // ビーム（空エリア・ランプ高さ以下・塔の外側）
                // -----------------------------------------------------------
                float beam = BeamIntensity(uv, lampUV, beamDir, aspect);
                beam *= aboveH;                     // 空のみ
                beam *= step(uv.y, lampY + 0.01);   // ランプより上は照らさない
                beam *= (1.0 - towerMask);           // 塔内部は描画しない

                // -----------------------------------------------------------
                // ランプグロー（塔頂部の発光）
                // -----------------------------------------------------------
                float2 toLamp   = float2((uv.x - 0.5) * aspect, uv.y - lampY);
                float  lampDist = length(toLamp);
                float  lampCore = exp(-lampDist * lampDist * 150.0);       // 鋭い中心発光
                float  lampHalo = exp(-lampDist * 10.0) * 0.35;            // 広がるハロー
                float  lampGlow = (lampCore + lampHalo) * _GlowIntensity;

                // -----------------------------------------------------------
                // 水平線（微細な波打ちつき輝くライン）
                // -----------------------------------------------------------
                float waveOff = sin(uv.x * 14.0 + time * _WaveSpeed) * 0.003 * _WaveStrength;
                float waveY   = horizonY + waveOff;
                float hDist   = abs(uv.y - waveY);
                float hLine   = exp(-hDist * hDist * 12000.0) * _HorizonIntensity;
                float hGlow   = exp(-hDist * 38.0) * _HorizonIntensity * 0.22;

                // -----------------------------------------------------------
                // 水面反射（海エリアのみ・波による UV 歪みつき）
                // -----------------------------------------------------------
                float  belowH   = 1.0 - aboveH;
                float  depth    = max(0.0, horizonY - uv.y);

                // 水面の揺れによる横方向歪み（水平線近くほど小さく）
                float  distortX = sin(uv.x * 22.0 + time * _WaveSpeed * 1.7)
                                  * 0.006 * _WaveStrength * saturate(depth * 10.0);
                // 反射UV：鏡像 + 歪み（縦方向を少し圧縮して奥行き感を出す）
                float2 reflUV   = float2(uv.x + distortX, horizonY + depth * 0.65);

                // 水面に映るランプ位置（水平線を軸に反転）
                float2 reflLampUV = float2(0.5, 2.0 * horizonY - lampY);
                float2 reflDir    = normalize(float2(beamDir.x, -beamDir.y));

                float  beamRefl = BeamIntensity(reflUV, reflLampUV, reflDir, aspect);
                beamRefl *= belowH * exp(-depth * 8.0) * _ReflIntensity;

                // -----------------------------------------------------------
                // 合成
                // -----------------------------------------------------------
                float3 col = bgCol;

                col += _BeamColor.rgb * beam;                        // メインビーム
                col += _BeamColor.rgb * beamRefl * 0.55;             // 水面反射
                col += _HorizonColor.rgb * (hLine + hGlow);          // 水平線
                col += _BeamColor.rgb * lampGlow;                    // ランプハロー

                // 塔シルエット（暗色で上書き）
                col = lerp(col, _TowerColor.rgb, towerMask);

                // ランプの光が塔シルエットから滲み出る（ランプ室の発光を表現）
                col += _BeamColor.rgb * lampCore * _GlowIntensity * 0.45 * towerMask;

                // UI 頂点カラー乗算
                col *= IN.color.rgb;
                float alpha = IN.color.a;

                // RectMask2D クリッピング
                #ifdef UNITY_UI_CLIP_RECT
                    float2 m = saturate((_ClipRect.zw - _ClipRect.xy
                        - abs(IN.worldPos * 2.0 - _ClipRect.zw - _ClipRect.xy)) * 4096.0);
                    alpha *= m.x * m.y;
                #endif

                // AlphaClip（Mask 対応）
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
