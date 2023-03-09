Shader "Custom/hightLight"
{
	Properties{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_OutlineCol("OutlineCol", Color) = (1,0,0,1)
		_OutlineFactor("OutlineFactor", Range(0,1)) = 0.1
		_MainTex("Base 2D", 2D) = "white"{}
	}
 
	SubShader
	{
		Pass
		{
			Cull Front

			CGPROGRAM
			#include "UnityCG.cginc"  
			fixed4 _OutlineCol;
			float _OutlineFactor;

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(vnormal.xy);
				o.pos.xy += offset * _OutlineFactor;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _OutlineCol;
			}

			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
