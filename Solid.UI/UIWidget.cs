using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Solid.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI
{
	public class UIWidget : Widget
	{
		public static readonly SolidProperty BackgroundProperty = SolidProperty.Register<UIWidget, Brush>(nameof(Background), new SolidPropertyMetadata()
		{
			DefaultValue = null,
			InheritFromHierarchy = false,
		});

		public static readonly SolidProperty ForegroundProperty = SolidProperty.Register<UIWidget, Brush>(nameof(Foreground), new SolidPropertyMetadata()
		{
			DefaultValue = new SolidBrush(Color4.Black),
			InheritFromHierarchy = true,
		});

		public static readonly SolidProperty SkinProperty = SolidProperty.Register<UIWidget, Skin>(nameof(Skin), new SolidPropertyMetadata()
		{
			DefaultValue = null,
			InheritFromHierarchy = true,
		});

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
			if (this.Background != null)
			{
				g.FillRectangle(this.GetClientRectangle(), this.Background);
			}
		}

		protected virtual void OnDrawPostChildren()
		{
			var g = this.UserInterface;
			if (this.Background == null)
			{
				g.DrawBox(this.Skin, "Window", this.GetClientRectangle());
			}
		}

		/// <summary>
		/// Gets a rectangle that defines where the widget is located.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetClientRectangle() => new Rectangle(
			this.Position.X, this.Position.Y,
			this.Size.Width, this.Size.Height);

		/// <summary>
		/// Gets or sets the background color of this widget.
		/// </summary>
		public Brush Background
		{
			get { return Get<Brush>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the foreground color of this widget.
		/// </summary>
		public Brush Foreground
		{
			get { return Get<Brush>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the skin of this widget.
		/// </summary>
		public Skin Skin
		{
			get { return Get<Skin>(SkinProperty); }
			set { Set(SkinProperty, value); }
		}

		/// <summary>
		/// Gets the associated user interface.
		/// </summary>
		public UserInterface UserInterface { get; internal set; }
	}
}
