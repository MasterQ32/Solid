using Solid.Layout;
using Solid.Markup;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System;
using OpenTK.Graphics;

namespace Solid.UI
{ 
	public class UserInterface : LayoutDocument
	{
		private int vertexArray;
		private int vertexBuffer;
		private int blitTextureShader;

		private int rectangleLocation;
		private int colorLocation;

		private Layout.Size screenSize;

		protected override void OnNodeCreation(Widget node)
		{
			var uiElement = node as UIWidget;
			if (uiElement == null)
				return;
			uiElement.UserInterface = this;
		}

		protected override void OnPostLayout(Layout.Size size)
		{
			this.screenSize = size;
		}

		private void Draw(Widget widget)
		{
			var drawable = widget as UIWidget;
			{
				var rect = drawable.GetClientRectangle();
				rect.Y = (int)(screenSize.Height - rect.Y - rect.Height);

				GL.Scissor(rect.Left, rect.Top, rect.Width, rect.Height);
				drawable?.Draw();
			}
			foreach (var child in widget.Children)
				Draw(child);
		}

		public void Draw()
		{
			this.BeginDraw();
			this.Draw(this.Root);
			this.EndDraw();
		}

		public void BeginDraw()
		{
			GL.BindVertexArray(this.vertexArray);
			GL.UseProgram(this.blitTextureShader);
			GL.Enable(EnableCap.ScissorTest);
			GL.Enable(EnableCap.Blend);
			GL.Disable(EnableCap.DepthTest);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.Uniform2(this.screenSizeLocation, this.screenSize.Width, this.screenSize.Height);
		}

		public void EndDraw()
		{
			GL.Disable(EnableCap.ScissorTest);
			GL.Disable(EnableCap.Blend);
			GL.BindVertexArray(0);
			GL.UseProgram(0);
		}

		public void FillRectangle(Rectangle area, Color color)
		{
			GL.Uniform4(this.rectangleLocation, (float)area.X, (float)area.Y, (float)area.Width, (float)area.Height);
			GL.Uniform4(this.colorLocation, (Color4)color);
			this.DrawRawQuad();
		}

		private void DrawRawQuad()
		{
			GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
		}

		public void InitializeOpenGL()
		{
			this.vertexArray = GL.GenVertexArray();
			this.vertexBuffer = GL.GenBuffer();

			GL.BindVertexArray(this.vertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer,
				sizeof(float) * 12,
				new[]
				{
					0.0f, 0.0f,
					1.0f, 0.0f,
					1.0f, 1.0f,

					0.0f, 0.0f,
					1.0f, 1.0f,
					0.0f, 1.0f,
				},
				BufferUsageHint.StaticDraw);

			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(
				0,
				2,
				VertexAttribPointerType.Float,
				false,
				2 * sizeof(float),
				0);

			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			this.blitTextureShader = GL.CreateProgram();

			{
				var vertexShader = GL.CreateShader(ShaderType.VertexShader);
				GL.ShaderSource(vertexShader, vertexShaderSource);
				GL.CompileShader(vertexShader);

				var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
				GL.ShaderSource(fragmentShader, fragmentShaderSource);
				GL.CompileShader(fragmentShader);

				GL.AttachShader(this.blitTextureShader, vertexShader);
				GL.AttachShader(this.blitTextureShader, fragmentShader);

				GL.LinkProgram(this.blitTextureShader);


				GL.DetachShader(this.blitTextureShader, vertexShader);
				GL.DetachShader(this.blitTextureShader, fragmentShader);

				GL.DeleteShader(vertexShader);
				GL.DeleteShader(fragmentShader);

				this.rectangleLocation = GL.GetUniformLocation(this.blitTextureShader, "uRectangle");
				this.screenSizeLocation = GL.GetUniformLocation(this.blitTextureShader, "uScreenSize");
				this.colorLocation = GL.GetUniformLocation(this.blitTextureShader, "uColor");
			}
		}

		static string vertexShaderSource =
@"#version 330 core

layout(location = 0) in vec2 vPosition;

uniform vec4 uRectangle;
uniform vec2 uScreenSize;

void main()
{
	float x = 2.0f * (uRectangle.x + vPosition.x * uRectangle.z) / uScreenSize.x - 1.0f;
	float y = 1.0f - 2.0f * (uRectangle.y + vPosition.y * uRectangle.w) / uScreenSize.y;
	gl_Position = vec4(x, y, 0.0f, 1.0f);
}
";

		static string fragmentShaderSource =
@"#version 330 core

out vec4 color;

uniform vec4 uColor;

void main() {
	color = uColor;
}";
		private int screenSizeLocation;

		public static UserInterface Load(string fileName)
		{
			var document = Parser.Load(fileName);
			var mapper = new UIMapper();
			return mapper.Instantiate(document);
		}
	}
}