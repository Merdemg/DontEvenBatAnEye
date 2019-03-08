// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "MaskPenta" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_MaskTex("Base (RGB) Trans (A)", 2D) = "white" {}

}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha:fade

sampler2D _MainTex;
sampler2D _MaskTex;

fixed4 _Color;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 ca = tex2D(_MaskTex, IN.uv_MainTex);
    o.Albedo = c.rgb;
    o.Alpha = ca.a;
}
ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}
