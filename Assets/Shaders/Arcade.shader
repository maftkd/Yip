Shader "Custom/Arcade"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Dissolve ("Dissolve", Range(0,1)) = 0.5
		_DissolveB ("DissolveB", Range(0,1)) = 0.8
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _ColorB;
		fixed _Dissolve;
		fixed _DissolveB;

		fixed3 hsv2rgb(fixed3 c)
		{
			fixed4 K = fixed4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			fixed3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
		}

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed n = tex2D (_MainTex, IN.uv_MainTex).r;
			fixed dt = dot(IN.viewDir,IN.worldNormal);
			fixed3 rgb = hsv2rgb(fixed3(abs(dt)*0.5+0.5,1,0.5));
			n=smoothstep(_Dissolve,_DissolveB,n);
            o.Albedo = lerp(_Color,rgb,n);//c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic*(1-n);
            o.Smoothness = _Glossiness*(1-n);
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
