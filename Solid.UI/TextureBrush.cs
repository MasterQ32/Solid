namespace Solid.UI
{
	using OpenTK.Graphics.OpenGL4;
	using System;

	public sealed class TextureBrush : RenderBrush
	{
		static string fragmentShaderSource =
@"#version 330 core

out vec4 color;
in vec2 uv;

uniform sampler2D uTexture;

void main() {
	color = texture2D(uTexture, uv);
}";

		private Texture texture;
		private int textureLocation;
		
		public TextureBrush(Texture texture) :
			base(fragmentShaderSource)
		{
			if (texture == null)
				throw new ArgumentNullException(nameof(texture));
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