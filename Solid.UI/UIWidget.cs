using Solid.Layout;
using Solid.UI.Input;
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
		public static readonly SolidProperty FormProperty = SolidProperty.Register<Widget, Form>("Form", new SolidPropertyMetadata()
		{

		});

		public static readonly SolidProperty StyleProperty = SolidProperty.Register<UIWidget, string>(nameof(Style), new SolidPropertyMetadata()
		{
			DefaultGenerator = (obj, prop) =>
			{
				var widget = (UIWidget)obj;
				var form = FormProperty.GetValue<Form>(obj);
				var styleName = widget.GetType().Name;
				return form.Skin[styleName];
			},
		});
		
		public static readonly SolidProperty IsTouchableProperty = SolidProperty.Register<UIWidget, bool>(nameof(IsTouchable), new SolidPropertyMetadata()
		{
			DefaultValue = true,
			InheritFromHierarchy = true,
		});

		public static readonly SolidProperty ClickCommandProperty = SolidProperty.Register<UIWidget, Command>(nameof(ClickCommand));

		protected static void ExtendDefaultGen(SolidProperty property, Func<Style, object> getValue)
		{
			// Overrides the property defaults of some properties so the style can define the default value.
			var prevGen = property.Metadata.DefaultGenerator;
			property.Metadata.DefaultGenerator = (obj, prop) =>
			{
				if (obj is UIWidget)
				{
					var widget = (UIWidget)obj;
					var style = widget.Style;
					if (style != null)
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

		public event EventHandler<TextInputEventArgs> TextInput;

		public event EventHandler<KeyboardEventArgs> KeyDown;
		public event EventHandler<KeyboardEventArgs> KeyUp;

		public event EventHandler<MouseEventArgs> MouseEnter;
		public event EventHandler<MouseEventArgs> MouseLeave;
		public event EventHandler<MouseEventArgs> MouseDown;
		public event EventHandler<MouseEventArgs> MouseMove;
		public event EventHandler<MouseEventArgs> MouseUp;
		public event EventHandler<MouseEventArgs> MouseWheel;

		private bool isHovered = false;

		private bool isPressed = false;

		#region Input Handling

		protected internal void OnKeyDown(KeyboardEventArgs e)
		{
			this.KeyDown?.Invoke(this, e);
		}

		protected internal void OnTextInput(TextInputEventArgs e)
		{
			this.TextInput?.Invoke(this, e);
		}

		protected internal void OnKeyUp(KeyboardEventArgs e)
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

		protected internal void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButton.Left)
				this.isPressed = true;
			this.MouseDown?.Invoke(this, e);
		}

		protected internal void OnMouseMove(MouseEventArgs e)
		{
			this.MouseMove?.Invoke(this, e);
		}

		protected internal void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				this.isPressed = false;
				this.ClickCommand?.Execute();
			}
			this.MouseUp?.Invoke(this, e);
		}

		protected internal void OnMouseWheel(MouseEventArgs e)
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
		public void Draw(WidgetDrawMode mode, IGraphics graphics)
		{
			if (mode.HasFlag(WidgetDrawMode.PreChildren))
				this.OnDrawPreChildren(graphics);
			if (mode.HasFlag(WidgetDrawMode.PostChildren))
				this.OnDrawPostChildren(graphics);
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
		protected virtual void OnDrawPreChildren(IGraphics graphics)
		{
			RenderStyleBrush(graphics, false);
		}

		private void RenderStyleBrush(IGraphics graphics, bool foreground)
		{
			var styleKey = this.GetCurrentStyleKey();
			var style = this.Style;
			if (style == null)
				return;
			var brush = style.GetBrush(styleKey, foreground);
			if (brush == null)
				return;
			graphics.DrawBrush(
				brush,
				this.GetClientRectangle());
		}

		protected virtual void OnDrawPostChildren(IGraphics graphics)
		{
			RenderStyleBrush(graphics, true);
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
		/// Gets if the mouse hovers the control.
		/// </summary>
		public bool IsHovered => this.isHovered;

		/// <summary>
		/// Gets or sets the style of the widget.
		/// </summary>
		public Style Style
		{
			get { return Get<Style>(StyleProperty); }
			set { Set(StyleProperty, value); }
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
