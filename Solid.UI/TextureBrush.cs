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
		
		private int textureLocation;

		public TextureBrush() :
			base(fragmentShaderSource)
		{
		}

		public TextureBrush(Texture texture) :
			this()
		{
			if (texture == null)
				throw new ArgumentNullException(nameof(texture));
			this.Texture = texture;
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
			this.Texture.Bind();
			GL.Uniform1(this.textureLocation, 0);
		}

		public Texture Texture
		{
			get; set;
		}
	}
}