Shader "Pang/UnlitBall"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelsInUnit("Pixel In Unit", float) = 16
        _Color("Color", Color) = (1,1,1,1)
        _BorderColor("Border Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
                
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PixelsInUnit;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BorderColor)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            half3 ObjectScale() {
                return half3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22));
            };

            float sampleUv(float2 pixelPosition)
            {
                return 1.0f - step(0.5f, length(pixelPosition));
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                half3 scale = ObjectScale();
                
                float pixelSize = 1.0f / _PixelsInUnit / scale.x;
                // offseting to the center of the pixel
                float2 uv = (i.uv - 0.5f) + (pixelSize / 2);
                // finding the pixel
                float2 pixelPosition = ((uv / pixelSize) - frac(uv / pixelSize));
                // back to 0.5f -0.5f stf range
                pixelPosition *= pixelSize;
                // stepping sdf
                float inside = sampleUv(pixelPosition);

                // checking if a pixel on a border 
                
                float sampleUp = sampleUv(pixelPosition + float2(0, - pixelSize));
                float sampleDown = sampleUv(pixelPosition + float2(0, pixelSize));
                float sampleLeft = sampleUv(pixelPosition + float2(-pixelSize, 0));
                float sampleRight = sampleUv(pixelPosition + float2(pixelSize, 0));

                float notBorder = step(0.9f, (sampleUp + sampleDown + sampleLeft + sampleRight) / 4);

                fixed4 borderColor = UNITY_ACCESS_INSTANCED_PROP(Props, _BorderColor);
                fixed4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                fixed4 border = borderColor * (1.0f - notBorder);
                fixed4 ball = color * notBorder;
                fixed4 col = (ball + border) * inside;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
