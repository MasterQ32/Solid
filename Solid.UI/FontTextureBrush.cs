using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Solid.UI
{
	internal sealed class FontTextureBrush : RenderBrush
	{
		static string fragmentShaderSource =
@"#version 330 core

out vec4 color;
in vec2 uv;

uniform sampler2D uTexture;
uniform vec4 uColor;

void main() {
	color = vec4(1, 1, 1, texture2D(uTexture, uv).r) * uColor;
;
}";

		private Texture texture;
		private int textureLocation;
		private int colorLocation;

		public FontTextureBrush() :
			base(fragmentShaderSource)
		{
			this.texture = new Texture();
		}

		protected override void OnCompiled()
		{
			this.textureLocation = GL.GetUniformLocation(this.ShaderProgram, "uTexture");
			this.colorLocation = GL.GetUniformLocation(this.ShaderProgram, "uColor");
			base.OnCompiled();
		}

		public override void Setup()
		{
			base.Setup();
			GL.ActiveTexture(TextureUnit.Texture0);
			this.texture.Bind();
			GL.Uniform1(this.textureLocation, 0);
			GL.Uniform4(this.colorLocation, this.Color);
		}

		public Color4 Color { get; set; } = Color4.Black;

		public Texture Texture => this.texture;
	}
}