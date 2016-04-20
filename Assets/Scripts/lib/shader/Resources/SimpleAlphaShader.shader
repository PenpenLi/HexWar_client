Shader "Custom/SimpleAlphaShader" {

	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
		_Alpha("Alpha",Float) = 1
	}
	
	SubShader {
	
		Tags {
			"Queue"="Transparent+10"
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
				float2 uv:TEXCOORD;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
				float2 uv:TEXCOORD;
			};

			sampler2D _MainTex;
			float _Alpha;
			
			v2f vert(appdata v){
			
				v2f o;
			
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				
				o.uv = v.uv;
				
				return o;
			}
			
			half4 frag(v2f o):COLOR{
				half4 h = tex2D(_MainTex,o.uv);
			
				h.w = h.w * _Alpha;
				return h;
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
