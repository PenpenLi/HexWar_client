Shader "Custom/HeroSurf" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_OutlineColor("Outline Color",Color) = (0,0,0,1)
		_PartIndex("Part Index", Float) = 0.0
		_WeaponIndex("Weapon Index", Float) = 0.0

		_AlphaXXX("Alpha",Float) = 1
		_ZWrite("ZWrite",Float) = 1
		
		_SrcAlpha("SrcAlpha",Float) = 1
		_TarAlpha("TarAlpha",Float) = 0
		_RimPower("RimPower",Range(1,10)) = 5
		_InnerColor("InnerColor", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		
		Blend [_SrcAlpha] [_TarAlpha]  
		ZWrite [_ZWrite]
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert noforwardadd vertex:vert finalcolor:mycolor

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		int _PartIndex;
		int _WeaponIndex;
		half4 _OutlineColor;
		float _AlphaXXX;
		fixed4 _Color;
		float _RimPower;
		half4 _InnerColor;

		struct Input {
		
			float2 uv_MainTex;
			float isOutline;
			half4 color;
		};

		
		void vert (inout appdata_full v, out Input o) {
		
            UNITY_INITIALIZE_OUTPUT(Input,o);
            
            if(v.texcoord1.x > 0 && v.texcoord1.x != _PartIndex){
			
				v.vertex.x = 0;
				v.vertex.y = 0;
				v.vertex.z = 0;
				
			}else if(v.texcoord1.x < 0 && v.texcoord1.x != _WeaponIndex){
			
				v.vertex.x = 0;
				v.vertex.y = 0;
				v.vertex.z = 0;
				
			}else{
			
				o.isOutline = v.texcoord1.y;
				
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float dotProduct = 1 - dot(v.normal, viewDir);
                o.color = _InnerColor * pow(dotProduct,_RimPower);//  _RimColor;
			}
        }

		void surf (Input IN, inout SurfaceOutput o) {

			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
		}
		
		void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
	    {
	    	color = _OutlineColor * IN.isOutline + (1 - IN.isOutline) * color * _Color;
	        
	        color.a = _AlphaXXX;
	        
	        color.rgb += IN.color;
	    }
		
		ENDCG
	} 
	FallBack "Diffuse"
}
