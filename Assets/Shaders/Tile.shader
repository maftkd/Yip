Shader "Custom/Tile"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ColorB ("ColorB", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _GridColor ("Grid Color", Color) = (1,1,1,1)
		_GridSize ("Grid size", Range(0,1)) = 0.1
		_Noise ("Noise", Range(0,1)) = 0.5
		_GridNoise ("Grid Noise", Range(0,1)) = 0.5
		_GridNoiseScale ("Grid Noise Scale", Float) = 5
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
			float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _ColorB;
        fixed4 _GridColor;
		fixed _GridSize;
		fixed _Noise;
		fixed _GridNoise;
		fixed _GridNoiseScale;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //fixed4 n = tex2D (_MainTex, IN.uv_MainTex).r;
            fixed4 n = tex2D (_MainTex, IN.worldPos.xz*0.08).r;
            fixed4 n2 = tex2D (_MainTex, IN.uv_MainTex*_GridNoiseScale).r;
			fixed gridN=(n2-0.5)*2;
			fixed gridX=step(frac(IN.worldPos.x+_GridNoise*gridN),_GridSize);
			fixed gridZ=step(frac(IN.worldPos.z+_GridNoise*gridN),_GridSize);
			fixed grid = saturate(gridX+gridZ);
            // Albedo comes from a texture tinted by color
			fixed glossN=lerp(n,1,1-_Noise);
			fixed4 c = lerp(_Color,_GridColor,grid);
            o.Albedo = lerp(c.rgb,_ColorB,glossN);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic*glossN;
            o.Smoothness = _Glossiness*glossN*(1-grid);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
