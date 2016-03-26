using Solid.Layout;
using Solid.Markup;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Graphics;

namespace Solid.UI
{ 
	public class UserInterface : LayoutDocument
	{
		private int vertexArray;
		private int vertexBuffer;

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

				GL.Scissor((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
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
			GL.Enable(EnableCap.ScissorTest);
			GL.Enable(EnableCap.Blend);
			GL.Disable(EnableCap.DepthTest);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		public void EndDraw()
		{
			GL.Disable(EnableCap.ScissorTest);
			GL.Disable(EnableCap.Blend);
			GL.BindVertexArray(0);
			GL.UseProgram(0);
		}

		public void FillRectangle(Rectangle area, Brush brush)
		{
			GL.UseProgram(brush.ShaderProgram);

			brush.Setup();

			GL.Uniform2(brush.ScreenSizeLocation, this.screenSize.Width, this.screenSize.Height);
			GL.Uniform4(brush.RectangleLocation, area.X, area.Y, area.Width, area.Height);

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

			
		}
		
		public static UserInterface Load(string fileName)
		{
			var document = Parser.Load(fileName);
			var mapper = new UIMapper();
			return mapper.Instantiate(document);
		}
	}
}