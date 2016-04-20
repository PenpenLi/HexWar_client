Shader "Custom/InnerLight" {
	Properties {
		_Color ("Color (RGB)", Color) = (1,1,0.5,1)
		_RimPower("RimPower",Range(1,10)) = 5
		_MainTex ("Albedo (RGB)", 2D) = "black" {}
	}
	SubShader {

		LOD 200
		
		Pass {
		
			CGPROGRAM
			#pragma vertex vert  
            #pragma fragment frag  
              
            #include "UnityCG.cginc"  
            
			sampler2D _MainTex;
			float _RimPower;
			fixed4 _Color;

			struct appdata_t {  
	            float4 vertex : POSITION;  
	            half4 color : COLOR;
	            float4 texcoord : TEXCOORD0;  
	            float3 normal : NORMAL;
	        };  
	          
	        struct v2f {  
	            float4 vertex : POSITION;  
	            half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
	        };  
	          
	        v2f vert(appdata_t v) {  
	            v2f o;
	           	o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				o.worldPos = v.vertex.xy ;
				
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                float dotProduct = 1 - dot(v.normal, viewDir);
		//		o.color.rgb = smoothstep(1 - rimWidth, 1.0, dotProduct);
                o.color = _Color * pow(dotProduct,_RimPower);//  _RimColor;
//				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
//				v.rim = TransformViewToProjection(norm.xy).x;

	            return o;  
	        }  

			float4 frag(v2f IN) : COLOR {  

				// Sample the texture
				half4 col = tex2D(_MainTex, IN.texcoord) ;

				col.rgb += IN.color;
				return col;
			}  
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
