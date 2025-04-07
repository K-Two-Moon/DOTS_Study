Shader "Unlit/gpuinstance" {
    Properties {
        _Color ("color", COLOR) = (1, 1, 1, 1)
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            // 开启多实例的变量编译
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                //顶点着色器的 InstancingID 定义
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                //顶点着色器的 InstancingID 定义
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : SV_POSITION;
            };

            // 定义多实例变量数组
            UNITY_INSTANCING_BUFFER_START(props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_INSTANCING_BUFFER_END(props)
            v2f vert(appdata v) {
                v2f o;
                //装配 InstancingID
                UNITY_SETUP_INSTANCE_ID(v);
                //输入到结构中传给片元着色器
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                //装配 InstancingID
                UNITY_SETUP_INSTANCE_ID(i);
                //提取多实例中的当前实例的Color属性变量值
                return UNITY_ACCESS_INSTANCED_PROP(props, _Color);
            }
            ENDCG
        }
    }
}