Shader "Custom/SceneShift"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color1("Color1", Color) = (1, 1, 1, 1)
        _Color2("Color2", Color) = (0.5, 0.75, 1.0, 1.0)
    }
    SubShader
    {
        // ... 其他子着色器代码 ...

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color1;
            fixed4 _Color2;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 iResolution = _ScreenParams.xy;
                float iTime = _Time.y;

                // 计算屏幕宽高比
                float aspect = iResolution.y / iResolution.x;
                // 定义一个变量保存计算结果
                float value;
                // 将屏幕坐标转换为 [0, 1] 的范围
                float2 uv = i.uv;
                // 平移坐标，使中心在屏幕中心
                uv -= float2(0.5, 0.5 * aspect);
                // 旋转坐标
                float rot = radians(45.0); // radians(45.0 * sin(iTime));
                float2x2 m = float2x2(cos(rot), -sin(rot), sin(rot), cos(rot));
                uv = mul(m, uv);
                // 平移坐标，使中心回到屏幕中心
                uv += float2(0.5, 0.5 * aspect);
                // 纠正坐标，使宽高比正确
                uv.y += 0.5 * (1.0 - aspect);
                // 计算位置
                float2 pos = 10.0 * uv;
                // 计算重复部分
                float2 rep = frac(pos);
                // 计算距离
                float dist = 2.0 * min(min(rep.x, 1.0 - rep.x), min(rep.y, 1.0 - rep.y));
                // 计算和中心点的距离
                float squareDist = length((floor(pos) + float2(0.5, 0.5)) - float2(5.0, 5.0));

                // 计算边缘值
                float edge = sin(iTime - squareDist * 0.5) * 0.5 + 0.5;
                // 以下是更多计算边缘值的代码，根据您的需要选择
                // edge = (iTime - squareDist * 0.5) * 0.5;
                // edge = 2.0 * frac(edge * 0.5);

                value = frac(dist * 2.0);
                // 使用 lerp 函数进行插值
                value = lerp(value, 1.0 - value, step(1.0, edge));

                // 缩放边缘值
                edge = pow(abs(1.0 - edge), 2.0);
                // 平滑化边缘值
                value = smoothstep(edge - 0.05, edge, 0.95 * value);
                // 添加距离信息
                value += squareDist * .1;

                // 根据value计算颜色
                float4 fragColor = lerp(_Color1, _Color2, value);
                // 计算透明度
                fragColor.a = 0.25 * clamp(value, 0.0, 1.0);

                return fragColor;
            }
            ENDHLSL
        }
    }
}
