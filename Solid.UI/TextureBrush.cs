using OpenTK.Graphics.OpenGL4;

namespace Solid.UI
{
	public sealed class TextureBrush : Brush
	{
		private Texture texture;

		static string fragmentShaderSource =
@"#version 330 core

out vec4 color;
in vec2 uv;

uniform sampler2D uTexture;

void main() {
	color = texture2D(uTexture, uv);
}";
		private int textureLocation;

		public TextureBrush(Texture texture) : 
			base(fragmentShaderSource)
		{
			this.texture = texture;
		}

		protected override void OnCompiled()
		{
			this.textureLocation = GL.GetUniformLocation(this.ShaderProgram, "uTexture");
			base.OnCompiled();
		}

		public override void Setup()
		{
			base.Setup();
			GL.ActiveTexture(TextureUnit.Texture0);
			this.texture.Bind();
			GL.Uniform1(this.textureLocation, 0);
		}
	}
}