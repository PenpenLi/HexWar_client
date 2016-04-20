Shader "Custom/UI/LightningMove_2"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
//		_StencilComp ("Stencil Comparison", Float) = 8
//		_Stencil ("Stencil ID", Float) = 0
//		_StencilOp ("Stencil Operation", Float) = 0
//		_StencilWriteMask ("Stencil Write Mask", Float) = 255
//		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15
		
		_LTex("L Texture", 2D) = ""{}
		
//		_UFix("UFix", Float) = 0
//		_VFix("VFix", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent+2" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
//		Stencil
//		{
//			Ref [_Stencil]
//			Comp [_StencilComp]
//			Pass [_StencilOp] 
//			ReadMask [_StencilReadMask]
//			WriteMask [_StencilWriteMask]
//		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				
				half2 oTexcoord : TEXCOORD2;
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
	
			sampler2D _MainTex;
			sampler2D _LTex;
			
			float _StrongFix;
			
			float _UFix = 0;
			float _VFix = 0;
			
			float _UOffset;
			float _VOffset;
			
			float _UScale;
			float _VScale;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
				#endif
				
				OUT.color = IN.color * _Color;
				
				OUT.oTexcoord.x = (IN.texcoord.x - _UOffset) * _UScale + _UFix;
				OUT.oTexcoord.y = (IN.texcoord.y - _VOffset) * _VScale + _VFix;
				
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

				half4 addColor = tex2D(_LTex, IN.oTexcoord) * _StrongFix;
				
				color.x = color.x + addColor.w;
				color.y = color.y + addColor.w;
				color.z = color.z + addColor.w;

				return color;
			}
		ENDCG
		}
	}
}
