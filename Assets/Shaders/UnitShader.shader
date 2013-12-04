Shader "Tactibru/UnitShader" {
	Properties {
		_MainTex("Base (RGBA)", 2D) = "black" {}
		//_HightlightColor("Highlight Color", Color) = (1, 0, 0, 1)
		_BaseColor("Base Color", Color) = (0, 1, 0, 1)
		//_ShadeColor("Shade Color", Color) = (0, 0, 1, 1)
		
		_TargetHighlightColor("Target Highlight Color", Color) = (1, 0, 0, 1)
		_TargetBaseColor("Target Base Color", Color) = (1, 0, 0, 1)
		_TargetShadeColor("Target Shade Color", Color) = (1, 0, 0, 1)
	}
	
	SubShader {
		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		
		Cull Off
		ZTest Less
		ColorMask RGBA
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		
		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			
			float4 _BaseColor;
			//float4 _HighlightColor = _BaseColor + float4(0.2f, 0.2f, 0.2f, 0.0f);
			//float4 _ShadeColor = _BaseColor - float4(0.2f, 0.2f, 0.2f, 0.0f);
			
			float4 _TargetHighlightColor;
			float4 _TargetBaseColor;
			float4 _TargetShadeColor;
			
			struct FragIn {
				float4 pos : SV_POSITION;
				float2 texCoord : TEXCOORD0;
			};
			
			bool float3_equal(float3 lhs, float3 rhs) {
				return ((lhs.r == rhs.r) &&
					(lhs.g == rhs.g) &&
					(lhs.b == rhs.b));
			}
				
			FragIn vert(appdata_base input) {
				FragIn output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.texCoord = input.texcoord.xy;
				return output;
			}
				
			half4 frag(FragIn input) : COLOR {
				half4 fragColor = tex2D(_MainTex, input.texCoord.xy);
				
				float alpha = fragColor.a;
				
				if(float3_equal(fragColor.rgb, _TargetBaseColor.rgb))
					fragColor.rgb = _BaseColor.rgb;
				else if(float3_equal(fragColor.rgb, _TargetShadeColor.rgb))
					fragColor.rgb = (_BaseColor.rgb - float3(0.25f, 0.25f, 0.25f));
				else if(float3_equal(fragColor.rgb, _TargetHighlightColor.rgb))
					fragColor.rgb = (_BaseColor.rgb + float3(0.25f, 0.25f, 0.25f));
				
				fragColor.a = alpha;
				
				return fragColor;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
