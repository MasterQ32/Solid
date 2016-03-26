using OpenTK.Graphics.OpenGL4;

namespace Solid.UI
{
	public abstract class Brush
	{
		private int shaderProgram;
		private int screenSizeLocation;
		private int rectangleLocation;

		internal int ScreenSizeLocation => this.screenSizeLocation;
		internal int RectangleLocation => this.rectangleLocation;

		protected Brush(string fragmentShaderSource)
		{
			this.shaderProgram = GL.CreateProgram();

			var vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertexShaderSource);
			GL.CompileShader(vertexShader);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragmentShaderSource);
			GL.CompileShader(fragmentShader);

			GL.AttachShader(this.shaderProgram, vertexShader);
			GL.AttachShader(this.shaderProgram, fragmentShader);

			GL.LinkProgram(this.shaderProgram);


			GL.DetachShader(this.shaderProgram, vertexShader);
			GL.DetachShader(this.shaderProgram, fragmentShader);

			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);

			this.rectangleLocation = GL.GetUniformLocation(this.shaderProgram, "uRectangle");
			this.screenSizeLocation = GL.GetUniformLocation(this.shaderProgram, "uScreenSize");

			this.OnCompiled();
		}

		protected virtual void OnCompiled()
		{

		}

		public virtual void Setup()
		{

		}


		public int ShaderProgram => this.shaderProgram;

		static string vertexShaderSource =
@"#version 330 core

layout(location = 0) in vec2 vPosition;

uniform vec4 uRectangle;
uniform vec2 uScreenSize;

out vec2 uv;

void main()
{
	float x = 2.0f * (uRectangle.x + vPosition.x * uRectangle.z) / uScreenSize.x - 1.0f;
	float y = 1.0f - 2.0f * (uRectangle.y + vPosition.y * uRectangle.w) / uScreenSize.y;
	gl_Position = vec4(x, y, 0.0f, 1.0f);
	uv = vPosition;
}
";
	}
}