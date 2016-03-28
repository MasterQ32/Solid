using Solid.Layout;
using Solid.Markup;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Graphics;
using Solid.UI.Skinning;
using OpenTK.Input;

namespace Solid.UI
{
	public class UserInterface : LayoutDocument
	{
		private int vertexArray;
		private int vertexBuffer;

		private Layout.Size screenSize;
		private InputSource input;

		private UIWidget currentMouseWidget;

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
			if (drawable != null)
			{
				var rect = drawable.GetClientRectangle();
				rect.Y = (int)(screenSize.Height - rect.Y - rect.Height);

				GL.Scissor((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
				drawable?.Draw(WidgetDrawMode.PreChildren);
			}
			foreach (var child in widget.Children)
				Draw(child);
			if (drawable != null)
			{
				var rect = drawable.GetClientRectangle();
				rect.Y = (int)(screenSize.Height - rect.Y - rect.Height);

				GL.Scissor((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
				drawable?.Draw(WidgetDrawMode.PostChildren);
			}
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

		public void RenderBrush(Brush brush, Rectangle area)
		{
			if (brush == null)
			{
				return;
			}
			else if (brush is RenderBrush)
			{
				var rbrush = (RenderBrush)brush;
				GL.UseProgram(rbrush.ShaderProgram);

				rbrush.Setup();

				GL.Uniform2(rbrush.ScreenSizeLocation, this.screenSize.Width, this.screenSize.Height);
				GL.Uniform4(rbrush.RectangleLocation, area.X, area.Y, area.Width, area.Height);

				this.DrawRawQuad();
			}
			else if (brush is LogicBrush)
			{
				var lbrush = (LogicBrush)brush;
				lbrush.Draw(this, area);
			}
			else
			{
				throw new InvalidOperationException("The given brush type is not supported.");
			}
		}

		public void RenderStyleBrush(string brushKey, StyleKey key, Rectangle area)
		{
			var style = this.Skin[brushKey];
			if (style != null)
				this.RenderBrush(style.GetBrush(key), area);
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

		private void BindInput()
		{
			if (this.input == null)
				return;

			this.input.MouseDown += Input_MouseDown;
			this.input.MouseMove += Input_MouseMove;
			this.input.MouseUp += Input_MouseUp;
			this.input.MouseWheel += Input_MouseWheel;

			this.input.KeyDown += Input_KeyDown;
			this.input.KeyPress += Input_KeyPress;
			this.input.KeyUp += Input_KeyUp;
		}

		private void UnbindInput()
		{
			if (this.input == null)
				return;
		}

		public Skin Skin { get; set; }

		public InputSource Input
		{
			get { return this.input; }
			set
			{
				this.UnbindInput();
				this.input = value;
				this.BindInput();
			}
		}

		#region Input Handling

		private UIWidget GetWidgetFromPosition(Point pt) => this.GetWidgetFromPosition(this.Root, pt);

		private UIWidget GetWidgetFromPosition(Widget widget, Point pt)
		{
			var rect = new Rectangle(widget.Position, widget.Size);
			if (rect.Contains(pt) == false)
				return null;
			foreach(var child in widget.Children)
			{
				var childWidget = GetWidgetFromPosition(child, pt);
				if (childWidget != null)
					return childWidget;
			}
			if (widget is UIWidget) {
				var uiWidget = (UIWidget)widget;
				if (uiWidget.IsTouchable == false)
					return null;
				return uiWidget;
			}
			else {
				return null;
			}
		}
		
		private void Input_MouseWheel(object sender, OpenTK.Input.MouseWheelEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);
			if (widget != null)
			{
				var cpos = widget.PointToClient(ipos);
				widget.OnMouseWheel(new MouseWheelEventArgs((int)cpos.X, (int)cpos.Y, e.Value, e.Delta));
			}
		}

		private void Input_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);
			if (widget != null)
			{
				var cpos = widget.PointToClient(ipos);
				widget.OnMouseUp(new MouseButtonEventArgs((int)cpos.X, (int)cpos.Y, e.Button, e.IsPressed));
			}
		}

		private void Input_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);

			if(widget != this.currentMouseWidget)
			{
				if (this.currentMouseWidget != null)
				{
					var localPos = this.currentMouseWidget.PointToClient(ipos);
					this.currentMouseWidget.OnMouseLeave(new MouseEventArgs((int)localPos.X, (int)localPos.Y));
				}
				this.currentMouseWidget = widget;
				if (this.currentMouseWidget != null)
				{
					var localPos = this.currentMouseWidget.PointToClient(ipos);
					this.currentMouseWidget.OnMouseEnter(new MouseEventArgs((int)localPos.X, (int)localPos.Y));
				}
			}
			if (widget == null)
				return;
			var cpos = widget.PointToClient(ipos);
			widget.OnMouseMove(new MouseMoveEventArgs((int)cpos.X, (int)cpos.Y, e.XDelta, e.YDelta));
		}

		private void Input_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);
			if (widget != null)
			{
				var cpos = widget.PointToClient(ipos);
				widget.OnMouseDown(new MouseButtonEventArgs((int)cpos.X, (int)cpos.Y, e.Button, e.IsPressed));
			}
		}
		
		private void Input_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{

		}

		private void Input_KeyPress(object sender, OpenTK.KeyPressEventArgs e)
		{

		}

		private void Input_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{

		}

		#endregion

		public static UserInterface Load(string fileName)
		{
			var document = Parser.Load(fileName);
			var mapper = new UIMapper();
			return mapper.Instantiate(document);
		}
	}
}