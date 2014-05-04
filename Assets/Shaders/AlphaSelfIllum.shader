Shader "AlphaSelfIllum" {

Properties {

    _Color ("Main Color", Color) = (1,1,1,1)

    _MainTex ("Color (RGB) Alpha (A)", 2D) = "white" {}

    _Illum ("Illumin (RBG)", 2D) = "green" {}

    _BumpMap ("Normalmap", 2D) = "bump" {}

    _EmissionLM ("Emission (Lightmapper)", Float) = 0

}

SubShader {

    Tags { "Queue"="Transparent" "RenderType"="Transparent" }

    LOD 300

 

CGPROGRAM

#pragma surface surf Lambert alpha

 

sampler2D _MainTex;

sampler2D _BumpMap;

sampler2D _Illum;

fixed4 _Color;

 

struct Input {

    float2 uv_MainTex;

    float2 uv_Illum;

    float2 uv_BumpMap;

};

 

void surf (Input IN, inout SurfaceOutput o) {

    fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);

    fixed4 c = tex * _Color;

    o.Albedo = c.rgb;

	o.Alpha = tex2D (_MainTex, IN.uv_MainTex).a;

    o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).g;

    o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

}

ENDCG

} 

FallBack "Self-Illumin/Diffuse"

}