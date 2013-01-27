Shader "Custom/PulseEcho" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0) // Echo color
        _Position ("Position", Vector) = (0.0, 0.0, 0.0) // Position of pulse center in world coordinates
        _Radius ("Radius", float) = 1.0
        _MaxRadius ("Max Radius", float) = 5.0
        _Fade ("Fade", float) = 0.0
    }
    SubShader {
        Tags { "RenderType" = "Opaque" "Queue" = "Overlay" }
        LOD 200
        Lighting Off Cull Off ZWrite Off Fog { Mode Off } 
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf NoLighting

        sampler2D _MainTex;
        float4 _MainColor;
        float3 _Position;
        float  _Radius;
        float  _MaxRadius;
        float  _Fade;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half4 LightingNoLighting(SurfaceOutput s, half3 lightDir, half atten) {
            half4 c;

            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        void surf (Input IN, inout SurfaceOutput o) {
            // Compute the distance from the current pixel's world coordinate to the center of the pulse.
            float dist = distance(IN.worldPos, _Position);
            // Compute the fade using the distance from the center of the pulse.
            float fade = (dist - _Radius) * -1 + _Fade;
            // Normalize the fade value.
            float c2 = fade / _MaxRadius;
            float c1 = 1.0 - c2;

            // Blend the echo color if the pixel is inside the sphere and there's still a fade value.
            if (dist < _Radius && fade > 0.0) {
                o.Albedo = _MainColor.rgb * c1 + tex2D(_MainTex, IN.uv_MainTex).rgb * c2;
            } else {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            }
        }
        ENDCG
    } 
    FallBack "Diffuse"
}
