using Solid.Layout;
using Solid.Markup;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Graphics;
using Solid.UI.Skinning;
using OpenTK.Input;
using SharpFont;
using System.IO;
using System.Collections.Generic;

namespace Solid.UI
{
	public class UserInterface : LayoutDocument
	{
		public static readonly Library FontLibrary = new Library();

		private int vertexArray;
		private int vertexBuffer;

		private Layout.Size screenSize;
		private InputSource input;

		private UIWidget currentMouseWidget;

		private FontTextureBrush fontBrush;

		public UserInterface() :
			base(new UIMapper())
		{
			this.Skin = new Skin();
			this.Skin["Panel"] = new Style()
			{
				Padding = new Thickness(8.0f),
				Default = new TextureBoxBrush()
				{
					BorderSize = 1,

					TopLeft = new SolidBrush(Color4.Black),
					TopMiddle = new SolidBrush(Color4.Black),
					TopRight = new SolidBrush(Color4.Black),
					MiddleLeft = new SolidBrush(Color4.Black),
					MiddleRight = new SolidBrush(Color4.Black),
					BottomLeft = new SolidBrush(Color4.Black),
					BottomCenter = new SolidBrush(Color4.Black),
					BottomRight = new SolidBrush(Color4.Black),

					Center = new SolidBrush(Color4.DimGray),
				},
			};
			this.Skin["Button"] = new Style()
			{
				// Width = 128.0f,
				Height = 32.0f,
				Padding = new Thickness(4.0f),
				Margin = new Thickness(2.0f),
				Default = new SolidBrush(new Color4(0.7f, 0.7f, 0.7f, 1.0f)),
				Hovered = new SolidBrush(new Color4(0.8f, 0.8f, 0.8f, 1.0f)),
				Active = new SolidBrush(new Color4(0.6f, 0.6f, 0.6f, 1.0f)),
				Disabled = new SolidBrush(new Color4(0.3f, 0.3f, 0.3f, 1.0f)),
			};
			this.Skin["Label"] = new Style()
			{
				// Font = new Face(FontLibrary, @"C:\Windows\Fonts\arial.ttf"),
				FontSize = 24,
				FontColor = Color4.Black,
			};
		}

		public object ViewModel
		{
			get { return this.Root.BindingSource; }
			set { this.Root.BindingSource = value; }
		}

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
			GL.Disable(EnableCap.CullFace);

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		public void EndDraw()
		{
			GL.Disable(EnableCap.ScissorTest);
			GL.Disable(EnableCap.Blend);
			GL.BindVertexArray(0);
			GL.UseProgram(0);
		}

		public Rectangle RenderString(
			string text,
			Rectangle area,
			string styleName,
			bool onlyMeasureFont = false)
		{
			var style = this.Skin[styleName];

			Point cursor = new Point(0.0f, 0.0f);
			Rectangle resultingSize = new Rectangle(area.Position, new Size(0, style.FontSize));

			style.Font.SetPixelSizes(0, (uint)style.FontSize);

			for (int i = 0; i < text.Length; i++)
			{
				var c = text[i];

				switch (c)
				{
					case '\r': break;
					case '\n':
					{
						cursor.X = 0.0f;
						cursor.Y += style.FontSize;
						resultingSize.Height += style.FontSize;
						break;
					}
					default:
					{

						if (onlyMeasureFont)
							style.Font.LoadChar(c, LoadFlags.Default, LoadTarget.Normal);
						else
							style.Font.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);

						var glyph = style.Font.Glyph;
						var bitmap = glyph.Bitmap;

						if (onlyMeasureFont == false)
						{
							GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
							this.fontBrush.Color = style.FontColor;
							this.fontBrush.Texture.Load(bitmap.Width, bitmap.Rows, (PixelInternalFormat)PixelFormat.Red, PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);
							this.RenderBrush(
								this.fontBrush,
								new Rectangle(
									area.X + cursor.X + glyph.BitmapLeft,
									area.Y + cursor.Y + style.FontSize - glyph.BitmapTop,
									bitmap.Width,
									bitmap.Rows));
						}

						resultingSize.Width = Math.Max(resultingSize.Width, cursor.X + glyph.Metrics.Width.ToSingle());

						cursor.X += glyph.Advance.X.ToSingle();
						cursor.Y += glyph.Advance.Y.ToSingle();
						break;
					}
				}
			}
			return resultingSize;
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
			var style = this.Skin?[brushKey];
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

			this.fontBrush = new FontTextureBrush();
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

		/// <summary>
		/// Gets the current resource loader.
		/// </summary>
		public static IResourceLoader ResourceLoader
		{
			get;
			private set;
		} = new DefaultResourceLoader();

		/// <summary>
		/// Sets the resource loader for the current program.
		/// </summary>
		/// <param name="loader"></param>
		public static void SetResourceLoader(IResourceLoader loader)
		{
			ResourceLoader = loader ?? new DefaultResourceLoader();
		}

		#region Input Handling

		private UIWidget GetWidgetFromPosition(Point pt) => this.GetWidgetFromPosition(this.Root, pt);

		private UIWidget GetWidgetFromPosition(Widget widget, Point pt)
		{
			var rect = new Rectangle(widget.Position, widget.Size);
			if (rect.Contains(pt) == false)
				return null;
			foreach (var child in widget.Children)
			{
				var childWidget = GetWidgetFromPosition(child, pt);
				if (childWidget != null)
					return childWidget;
			}
			if (widget is UIWidget)
			{
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

			if (widget != this.currentMouseWidget)
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

		private static readonly Dictionary<string, Type> customTypes = new Dictionary<string, Type>();

		public static void RegisterCustomWidget<T>(string name)
			where T : UIWidget
		{
			lock(customTypes)
				customTypes.Add(name, typeof(T));
		}

		public static void RegisterCustomWidget<T>()
			where T : UIWidget
			=> RegisterCustomWidget<T>(typeof(T).Name);

		public static UserInterface Load(string fileName)
		{
			var document = Parser.Load(fileName);
			return Create(document);
		}

		public static UserInterface Load(Stream stream, System.Text.Encoding encoding)
		{
			var document = Parser.Parse(stream, encoding);
			return Create(document);
		}

		private static UserInterface Create(MarkupDocument document)
		{
			var mapper = new UIMapper();
			lock (customTypes)
			{
				foreach (var ctype in customTypes)
					mapper.RegisterType(ctype.Value, ctype.Key);
			}
			return mapper.Instantiate(document);
		}
	}
}