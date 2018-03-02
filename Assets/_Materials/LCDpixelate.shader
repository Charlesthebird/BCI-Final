Shader "CustomEffects/CRTDiffuse" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MaskTex("Mask texture", 2D) = "white" {}
		_maskBlend("Mask blending", Float) = 0.5
		_Strength("Strength", Float) = 0.5
		_maskSize("Mask Size", Float) = 1
		_DisplacementTex("LCD (RGB)", 2D) = "white" {}
	}
	SubShader{
	Pass{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform sampler2D _DisplacementTex;
		uniform sampler2D _MaskTex;

		fixed _Strength;
		fixed _maskBlend;
		fixed _maskSize;

		float4 frag(v2f_img i) : COLOR{
			half2 n = tex2D(_DisplacementTex, i.uv);
			half2 d = n * 2 - 1;
			i.uv += d * _Strength;
			i.uv = saturate(i.uv);

			float4 c = tex2D(_MainTex, i.uv);
			return c;
		}
			ENDCG
	}
	}
}