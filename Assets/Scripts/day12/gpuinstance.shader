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
            // ������ʵ���ı�������
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                //������ɫ���� InstancingID ����
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                //������ɫ���� InstancingID ����
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : SV_POSITION;
            };

            // �����ʵ����������
            UNITY_INSTANCING_BUFFER_START(props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_INSTANCING_BUFFER_END(props)
            v2f vert(appdata v) {
                v2f o;
                //װ�� InstancingID
                UNITY_SETUP_INSTANCE_ID(v);
                //���뵽�ṹ�д���ƬԪ��ɫ��
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                //װ�� InstancingID
                UNITY_SETUP_INSTANCE_ID(i);
                //��ȡ��ʵ���еĵ�ǰʵ����Color���Ա���ֵ
                return UNITY_ACCESS_INSTANCED_PROP(props, _Color);
            }
            ENDCG
        }
    }
}