Shader "Custom/BattleHeroDamagePass" {
	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
	}
	
	SubShader {
	
		Tags {
			"Queue"="Transparent+30"
			"RenderType"="Transparent"
		}
		
		Pass{
		
			Blend SrcAlpha OneMinusSrcAlpha  
            ZWrite Off 
			ZTest Off
            
		
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata{
			
				float4 vertex:POSITION;
				float4 tangent:TANGENT;
				float2 uv:TEXCOORD;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
				float2 uv:TEXCOORD;
				float3 color : COLOR0;
			};


			sampler2D _MainTex;
			
			float4 positions[10];
			float4 fix[80];
			float4 state[80];
//			float index;
//			float matrixIndex;
			//float4x4 myMatrix[10];
			float4 scaleFix[10];

			
			v2f vert(appdata v){
			
				v2f o;
				
				float4 vt;
				float index = v.tangent.x;
				

				vt.x = v.uv.x + fix[index].y;
				vt.y = v.uv.y + fix[index].z;
				
				o.uv = vt.xy;
				
				//坐标
				float4 targetPos;
				
				float tmpIndex = state[index].y;
				
				targetPos = positions[tmpIndex];

//				targetPos.w = 1;
				
				vt = v.vertex * state[index].z;
				vt.x = vt.x + fix[index].x;
				
				//vt = mul(myMatrix[state[index].y], vt);

//				float4x4 tempMatrix;
//
//				tempMatrix[0][0] = scaleFix[state[index].y].x; tempMatrix[0][1] = 0; tempMatrix[0][2] = 0; tempMatrix[0][3] = 0;
//				tempMatrix[1][0] = 0; tempMatrix[1][1] = scaleFix[state[index].y].y; tempMatrix[1][2] = 0; tempMatrix[1][3] = 0;
//				tempMatrix[2][0] = 0; tempMatrix[2][1] = 0; tempMatrix[2][2] = scaleFix[state[index].y].z; tempMatrix[2][3] = 0;
//				tempMatrix[3][0] = 0; tempMatrix[3][1] = 0; tempMatrix[3][2] = 0; tempMatrix[3][3] = 1;

				vt = scaleFix[tmpIndex] * vt;
               	
               	o.pos = mul(UNITY_MATRIX_P, (mul(UNITY_MATRIX_MV , targetPos) + vt));
				o.color = state[index].x;
               	
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				half4 h = tex2D(_MainTex,o.uv);
			
				h.w = h.w * o.color.x;
				return h;
//				return half4( o.color, 1 );
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
