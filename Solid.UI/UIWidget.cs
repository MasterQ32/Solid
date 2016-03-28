using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
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

		/// <summary>
		/// Implements the widget specific draw routines.
		/// </summary>
		protected virtual void OnDrawPreChildren()
		{
			var g = this.UserInterface;

			var key = this.GetType().Name;
			if (string.IsNullOrWhiteSpace(this.Background) == false)
				key = this.Background;

			g.RenderStyleBrush(key, StyleKey.Default, this.GetClientRectangle());
		}

		protected virtual void OnDrawPostChildren()
		{
			var g = this.UserInterface;

			if (string.IsNullOrWhiteSpace(this.Foreground) == false)
				g.RenderStyleBrush(this.Foreground, StyleKey.Default, this.GetClientRectangle());
		}

		/// <summary>
		/// Gets a rectangle that defines where the widget is located.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetClientRectangle() => new Rectangle(
			this.Position.X, this.Position.Y,
			this.Size.Width, this.Size.Height);

		/// <summary>
		/// Gets the associated user interface.
		/// </summary>
		public UserInterface UserInterface { get; internal set; }

		public string Background
		{
			get { return Get<string>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		public string Foreground
		{
			get { return Get<string>(ForegroundProperty); }
			set { Set(ForegroundProperty, value); }
		}
	}
}
