namespace Solid.UI
{
	using OpenTK;
	using OpenTK.Graphics.OpenGL4;
	using System;

	public sealed class TextureBrush : RenderBrush
	{
		static string fragmentShaderSource =
@"#version 330 core

out vec4 color;
in vec2 uv;

uniform vec4 uSector;
uniform sampler2D uTexture;

void main() {
	color = texture2D(
		uTexture, 
		uSector.xy + uSector.zw * uv);
}";
		
		private int textureLocation;
		private int sectorLocation;

		public TextureBrush() :
			base(fragmentShaderSource)
		{
			this.Sector = new Rectangle(0.0f, 0.0f, 1.0f, 1.0f);
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
			this.sectorLocation = GL.GetUniformLocation(this.ShaderProgram, "uSector");
			this.textureLocation = GL.GetUniformLocation(this.ShaderProgram, "uTexture");
			base.OnCompiled();
		}

		public override void Setup()
		{
			base.Setup();
			GL.ActiveTexture(TextureUnit.Texture0);
			this.Texture.Bind();
			GL.Uniform1(this.textureLocation, 0);

			var sector = this.Sector;
			GL.Uniform4(this.sectorLocation, new Vector4(sector.X, sector.Y, sector.Width, sector.Height));
		}

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture Texture
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the portion of the texture that is shown.
		/// </summary>
		public Rectangle Sector
		{
			get; set;
		}
	}
}