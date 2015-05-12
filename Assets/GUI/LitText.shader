Shader "GUI/LitText" { 
Properties { 
   _MainTex ("Font Texture", 2D) = "white" {} 
   _Color ("Text Color", Color) = (1,1,1,1) 
   _Normal ("Text Normal",Vector) = (0,0,0,1)
} 

SubShader {
    Blend SrcAlpha OneMinusSrcAlpha
    Pass { 
        Color [_Color] 
        SetTexture [_MainTex] { 
            combine primary, texture * primary 
        } 
    } 
    pass {
         Tags { "LightMode" = "ForwardBase" } 

         CGPROGRAM

         #pragma vertex vert  
         #pragma fragment frag 

         uniform sampler2D _MainTex;  
         uniform float4 _Color; // define shader property for shaders
         uniform float4 _Normal;
         uniform float4 _LightColor0; 
             // color of light source (from "Lighting.cginc")
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
            float4 tex : TEXCOORD0;
         };

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;

            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 

            float3 normalDirection = normalize(float3(mul(_Normal, modelMatrixInverse)));
            float3 lightDirection = normalize(float3(_WorldSpaceLightPos0));

            float3 diffuseReflection = float3(_LightColor0) * float3(_Color)
               * max(0.0, dot(normalDirection, lightDirection));

            output.col = float4(diffuseReflection, 1.0);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            output.tex = input.texcoord;
            return output;
         }

         float4 frag(vertexOutput input) : COLOR
         {
            half4 color = tex2D(_MainTex, float2(input.tex));
            // use color.a to get alpha from text texture, rgb comes from vertex shader                        
            return float4(input.col.r,input.col.g,input.col.b,color.a);
         }

         ENDCG
    }
}
}
