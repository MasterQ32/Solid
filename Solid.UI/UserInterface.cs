using Solid.Layout;
using Solid.Markup;
using System;
using Solid.UI.Skinning;
using System.IO;
using System.Collections.Generic;
using Solid.UI.Input;

namespace Solid.UI
{
	public class UserInterface
	{
		private InputSource input;
		private Form form;

		private UIWidget currentMouseWidget;

		public UserInterface()
		{
		
		}

		public Form CurrentForm
		{
			get { return this.form; }
			set
			{
				if (value == this.form)
					return;
				if (value?.UI != null)
					throw new InvalidOperationException("This form is already bound to another user interface.");
				if (this.form != null) this.form.UI = null;
				this.form = value;
				if (this.form != null) this.form.UI = this;
			}
		}

		public void Update(IGraphics graphics)
		{
			this.CurrentForm?.Update(graphics.ScreenSize);
		}

		private void Draw(Widget widget, IGraphics graphics)
		{
			var drawable = widget as UIWidget;
			if (drawable != null)
			{
				var rect = drawable.GetClientRectangle();
				graphics.SetScissor(rect);
				drawable?.Draw(WidgetDrawMode.PreChildren, graphics);
			}
			foreach (var child in widget.Children)
				Draw(child, graphics);
			if (drawable != null)
			{
				var rect = drawable.GetClientRectangle();
				graphics.SetScissor(rect);
				drawable?.Draw(WidgetDrawMode.PostChildren, graphics);
			}
		}

		public void Draw(IGraphics graphics)
		{
			graphics.ResetScissor();
			this.Draw(this.CurrentForm.Root, graphics);
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
			this.input.TextInput += Input_TextInput;
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

		private UIWidget GetWidgetFromPosition(Point pt) => this.GetWidgetFromPosition(this.CurrentForm?.Root, pt);

		private UIWidget GetWidgetFromPosition(Widget widget, Point pt)
		{
			if (widget == null)
				return null;
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
		
		private void Input_MouseWheel(object sender, MouseEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);
			if (widget != null)
			{
				var cpos = widget.PointToClient(ipos);
				widget.OnMouseWheel(MouseEventArgs.CreateWheel((int)cpos.X, (int)cpos.Y, e.WheelDelta));
			}
		}

		private void Input_MouseUp(object sender, MouseEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);
			if (widget != null)
			{
				var cpos = widget.PointToClient(ipos);
				widget.OnMouseUp(MouseEventArgs.CreateButtonUp((int)cpos.X, (int)cpos.Y, e.Button));
			}
		}

		private void Input_MouseMove(object sender, MouseEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);

			if (widget != this.currentMouseWidget)
			{
				if (this.currentMouseWidget != null)
				{
					var localPos = this.currentMouseWidget.PointToClient(ipos);
					this.currentMouseWidget.OnMouseLeave(MouseEventArgs.CreateLeave((int)localPos.X, (int)localPos.Y));
				}
				this.currentMouseWidget = widget;
				if (this.currentMouseWidget != null)
				{
					var localPos = this.currentMouseWidget.PointToClient(ipos);
					this.currentMouseWidget.OnMouseEnter(MouseEventArgs.CreateEnter((int)localPos.X, (int)localPos.Y));
				}
			}
			if (widget == null)
				return;
			var cpos = widget.PointToClient(ipos);
			widget.OnMouseMove(MouseEventArgs.CreateMovement((int)cpos.X, (int)cpos.Y));
		}

		private void Input_MouseDown(object sender, MouseEventArgs e)
		{
			var ipos = new Point(e.X, e.Y);
			var widget = this.GetWidgetFromPosition(ipos);
			if (widget != null)
			{
				var cpos = widget.PointToClient(ipos);
				widget.OnMouseDown(MouseEventArgs.CreateButtonDown((int)cpos.X, (int)cpos.Y, e.Button));
			}
		}

		private void Input_KeyUp(object sender, KeyboardEventArgs e)
		{

		}

		private void Input_TextInput(object sender, TextInputEventArgs e)
		{

		}

		private void Input_KeyDown(object sender, KeyboardEventArgs e)
		{

		}

		#endregion
	}
}