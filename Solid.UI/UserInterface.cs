using Solid.Layout;
using Solid.Markup;
using System;
using Solid.UI.Skinning;
using System.IO;
using System.Collections.Generic;

namespace Solid.UI
{
	public class UserInterface : LayoutDocument
	{
		private Layout.Size screenSize;
		private InputSource input;

		private UIWidget currentMouseWidget;

		private FontTextureBrush fontBrush;

		public UserInterface() :
			base(new UIMapper())
		{
		
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

				// GL.Scissor((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
				drawable?.Draw(WidgetDrawMode.PreChildren);
			}
			foreach (var child in widget.Children)
				Draw(child);
			if (drawable != null)
			{
				var rect = drawable.GetClientRectangle();
				rect.Y = (int)(screenSize.Height - rect.Y - rect.Height);

				// GL.Scissor((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
				drawable?.Draw(WidgetDrawMode.PostChildren);
			}
		}

		public void Draw()
		{
			this.Draw(this.Root);
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

		/*
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
		*/

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