Shader "V/Outlines/SpriteOutline"
{
    Properties
    {
        _Color ("Tint", Color) = (0, 0, 0, 1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

        // When Rendering Opaque Material, 
        Blend SrcAlpha OneMinusSrcAlpha
        zWrite off
        Cull off    // render back

        Pass
        {
            CGPROGRAM

            #include "UnityCG.cginc"
            
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST; // Tilling Offest
            float4 _MainTex_TexelSize;  // Pixel Size of texture

            fixed4 _Color;
            fixed4 _OutlineColor;

            float _OutlineWidth;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                
                fixed4 color : COLOR;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex); 
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                col *= i.color;

                #define Div_Sqrt2 0.70710678118 // deal with diagonals

                /* Directions we want to sample it(also can use sin, cos) */
                float2 directions[8] = {float2(1, 0), float2(0, 1), float2(-1, 0), float2(0, -1), 
                    float2(Div_Sqrt2, Div_Sqrt2), float2(-Div_Sqrt2, Div_Sqrt2),
                    float2(-Div_Sqrt2, -Div_Sqrt2), float2(Div_Sqrt2, -Div_Sqrt2)}; 

                /* _OutlineDistance 倍數的 texture Pixel */
                float sampleDistance = _MainTex_TexelSize.xy * _OutlineWidth;

                float maxAlpha = 0;
                for(uint index = 0; index < 8; index++)
                {
                    float2 sampleUV = i.uv + directions[index] * sampleDistance; // cauculate sample point
                    maxAlpha = max(maxAlpha, tex2D(_MainTex, sampleUV).a);  // Maximum alpha 
                }

                /* Apply Border */ 
                col.rgb =   lerp(_OutlineColor, col.rgb, col.a);
                col.a = max(col.a, maxAlpha);

                return col;
            }
            ENDCG
        }
    }
}