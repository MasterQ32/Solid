using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Solid.UI
{
	public sealed class SolidBrush : Brush
	{
		private int colorLocation;

		static string fragmentShaderSource =
@"#version 330 core

out vec4 color;

uniform vec4 uColor;

void main() {
	color = uColor;
}";

		public SolidBrush(Color4 color) : 
			base(fragmentShaderSource)
		{
			this.Color = color;
		}

		protected override void OnCompiled()
		{
			this.colorLocation = GL.GetUniformLocation(this.ShaderProgram, "uColor");
			base.OnCompiled();
		}

		public override void Setup()
		{
			GL.Uniform4(this.colorLocation, this.Color);
		}

		public Color4 Color { get; set; }
	}
}