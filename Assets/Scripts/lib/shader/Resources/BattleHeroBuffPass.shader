Shader "Custom/BattleHeroBuffPass" {
	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
	}
	
	SubShader {
	
		Tags {
			"Queue"="Transparent+3"
			"RenderType"="Transparent"
		}
		
		Pass{
		
			Blend SrcAlpha OneMinusSrcAlpha  
            ZWrite Off 
            
		
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
			
			float4 positions[50];
			float4 fix[50];
			float index;
			//float4x4 myMatrix[40];
			float4 scaleFix[50];
			
			v2f vert(appdata v){
			
				v2f o;
				
				index = v.tangent.x;
				
				float4 vt;

				vt.x = v.uv.x;// + fix[index].x;
				vt.y = v.uv.y;// + fix[index].y;
				
				o.uv = vt.xy;
				
				//坐标
				float4 targetPos;
				
				vt.x = v.vertex.x;
				vt.y = v.vertex.y;
				vt.z = v.vertex.z;
				vt.w = v.vertex.w;

				vt.x = vt.x + scaleFix[index].w;
				
				vt = vt * fix[index].w;
				
				
				targetPos.x = positions[index].x;
				targetPos.y = positions[index].y;
				targetPos.z = positions[index].z;
				targetPos.w = 1;
				
				//vt = mul(myMatrix[index], vt);

//				float4x4 tempMatrix;
//
//				tempMatrix[0][0] = scaleFix[index].x; tempMatrix[0][1] = 0; tempMatrix[0][2] = 0; tempMatrix[0][3] = 0;
//				tempMatrix[1][0] = 0; tempMatrix[1][1] = scaleFix[index].y; tempMatrix[1][2] = 0; tempMatrix[1][3] = 0;
//				tempMatrix[2][0] = 0; tempMatrix[2][1] = 0; tempMatrix[2][2] = scaleFix[index].z; tempMatrix[2][3] = 0;
//				tempMatrix[3][0] = 0; tempMatrix[3][1] = 0; tempMatrix[3][2] = 0; tempMatrix[3][3] = 1;

				vt = scaleFix[index] * vt;
               	
              	o.pos = mul(UNITY_MATRIX_P, (mul(UNITY_MATRIX_MV , targetPos) + float4(vt.x, vt.y, vt.z, 0.0)));
				o.color = fix[index].z;
				
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
