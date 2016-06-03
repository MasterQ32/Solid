using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Solid.Layout;
using Solid.UI.Skinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI
{
	public class UIWidget : Widget
	{
		public static readonly SolidProperty ForegroundProperty = SolidProperty.Register<UIWidget, string>(nameof(Foreground));

		public static readonly SolidProperty BackgroundProperty = SolidProperty.Register<UIWidget, string>(nameof(Background));

		public static readonly SolidProperty IsTouchableProperty = SolidProperty.Register<UIWidget, bool>(nameof(IsTouchable), new SolidPropertyMetadata()
		{
			DefaultValue = true,
			InheritFromHierarchy = true,
		});

		public static readonly SolidProperty ClickCommandProperty = SolidProperty.Register<UIWidget, Command>(nameof(ClickCommand));

		private static void ExtendDefaultGen(SolidProperty property, Func<Style,object> getValue)
		{
			// Overrides the property defaults of some properties so the style can define the default value.
			var prevGen = property.Metadata.DefaultGenerator;
			property.Metadata.DefaultGenerator = (obj, prop) =>
			{
				if (obj is UIWidget)
				{
					var widget = (UIWidget)obj;
					var style = widget.UserInterface?.Skin?[widget.GetType().Name];
					if(style != null)
						return getValue(style);
				}
				return prevGen(obj, prop);
			};
		}

		static UIWidget()
		{
			ExtendDefaultGen(MarginProperty, (style) => style.Margin);
			ExtendDefaultGen(PaddingProperty, (style) => style.Padding);
			ExtendDefaultGen(HorizontalAlignmentProperty, (style) => style.HorizontalAlignment);
			ExtendDefaultGen(VerticalAlignmentProperty, (style) => style.VerticalAlignment);
			ExtendDefaultGen(DeclaredSizeProperty, (style) => style.Size);
		}

		public event EventHandler<KeyboardKeyEventArgs> KeyDown;
		public event EventHandler<KeyPressEventArgs> KeyPress;
		public event EventHandler<KeyboardKeyEventArgs> KeyUp;

		public event EventHandler<MouseEventArgs> MouseEnter;
		public event EventHandler<MouseEventArgs> MouseLeave;
		public event EventHandler<MouseButtonEventArgs> MouseDown;
		public event EventHandler<MouseMoveEventArgs> MouseMove;
		public event EventHandler<MouseButtonEventArgs> MouseUp;
		public event EventHandler<MouseWheelEventArgs> MouseWheel;

		private bool isHovered = false;

		private bool isPressed = false;

		#region Input Handling

		protected internal void OnKeyDown(KeyboardKeyEventArgs e)
		{
			this.KeyDown?.Invoke(this, e);
		}

		protected internal void OnKeyPress(KeyPressEventArgs e)
		{
			this.KeyPress?.Invoke(this, e);
		}

		protected internal void OnKeyUp(KeyboardKeyEventArgs e)
		{
			this.KeyUp?.Invoke(this, e);
		}
		
		protected internal void OnMouseEnter(MouseEventArgs e)
		{
			this.isHovered = true;
			this.MouseEnter?.Invoke(this, e);
		}

		protected internal void OnMouseLeave(MouseEventArgs e)
		{
			this.isHovered = false;
			this.isPressed = false;
			this.MouseLeave?.Invoke(this, e);
		}

		protected internal void OnMouseDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
				this.isPressed = true;
			this.MouseDown?.Invoke(this, e);
		}

		protected internal void OnMouseMove(MouseMoveEventArgs e)
		{
			this.MouseMove?.Invoke(this, e);
		}

		protected internal void OnMouseUp(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				this.isPressed = false;
				this.ClickCommand?.Execute();
			}
			this.MouseUp?.Invoke(this, e);
		}

		protected internal void OnMouseWheel(MouseWheelEventArgs e)
		{
			this.MouseWheel?.Invoke(this, e);
		}

		#endregion

		public void UpdateControlState()
		{

		}

		protected virtual void OnUpdateControlState()
		{

		}

		/// <summary>
		/// Draws the widget.
		/// </summary>
		public void Draw(WidgetDrawMode mode)
		{
			if (this.UserInterface == null)
				throw new InvalidOperationException("Cannot draw a user control without a associated user interface.");
			if (mode.HasFlag(WidgetDrawMode.PreChildren))
				this.OnDrawPreChildren();
			if (mode.HasFlag(WidgetDrawMode.PostChildren))
				this.OnDrawPostChildren();
		}

		protected virtual StyleKey GetCurrentStyleKey()
		{
			if (this.isPressed)
				return StyleKey.Active;
			if (this.isHovered)
				return StyleKey.Hovered;
			return StyleKey.Default;
		}

		/// <summary>
		/// Implements the widget specific draw routines.
		/// </summary>
		protected virtual void OnDrawPreChildren()
		{
			var g = this.UserInterface;

			var key = this.GetType().Name;
			if (string.IsNullOrWhiteSpace(this.Background) == false)
				key = this.Background;

			g.RenderStyleBrush(key, this.GetCurrentStyleKey(), this.GetClientRectangle());
		}

		protected virtual void OnDrawPostChildren()
		{
			var g = this.UserInterface;

			if (string.IsNullOrWhiteSpace(this.Foreground) == false)
				g.RenderStyleBrush(this.Foreground, this.GetCurrentStyleKey(), this.GetClientRectangle());
		}

		/// <summary>
		/// Gets a rectangle that defines where the widget is located.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetClientRectangle() => new Rectangle(
			this.Position.X, this.Position.Y,
			this.Size.Width, this.Size.Height);

		/// <summary>
		/// Converts a point in interface coordinates to local coordinates.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public Point PointToClient(Point pt) => (pt - this.Position);

		/// <summary>
		/// Converts a point from local coordinates to interface coordinates.
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public Point PointToInterface(Point pt) => (pt + this.Position);

		/// <summary>
		/// Gets the associated user interface.
		/// </summary>
		public UserInterface UserInterface { get; internal set; }

		/// <summary>
		/// Gets if the mouse hovers the control.
		/// </summary>
		public bool IsHovered => this.isHovered;

		/// <summary>
		/// Gets or sets the background skin.
		/// </summary>
		public string Background
		{
			get { return Get<string>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the foreground skin.
		/// </summary>
		public string Foreground
		{
			get { return Get<string>(ForegroundProperty); }
			set { Set(ForegroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the the element receives UI events.
		/// </summary>
		public bool IsTouchable
		{
			get { return Get<bool>(IsTouchableProperty); }
			set { Set(IsTouchableProperty, value); }
		}

		/// <summary>
		/// Gets or sets the command that is executed on click.
		/// </summary>
		public Command ClickCommand
		{
			get { return Get<Command>(ClickCommandProperty); }
			set { Set(ClickCommandProperty, value); }
		}
	}
}
