using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Solid.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI
{
    public class UIWidget : Widget
    {
		public static readonly SolidProperty BackgroundProperty = SolidProperty.Register<UIWidget, Color>(nameof(Background), new SolidPropertyMetadata()
		{
			DefaultValue = Color.Transparent,
			InheritFromHierarchy = true,
		});

		public static readonly SolidProperty ForegroundProperty = SolidProperty.Register<UIWidget, Color>(nameof(Foreground), new SolidPropertyMetadata()
		{
			DefaultValue = Color.Black,
			InheritFromHierarchy = true,
		});
		
		/// <summary>
		/// Draws the widget.
		/// </summary>
		public void Draw()
		{
			if (this.UserInterface == null)
				throw new InvalidOperationException("Cannot draw a user control without a associated user interface.");
			this.OnDraw();
		}

		/// <summary>
		/// Implements the widget specific draw routines.
		/// </summary>
		protected virtual void OnDraw()
		{
			// Render the UI element here...
			// TODO: Improve the widget rendering...
			GL.ClearColor(Color4.Pink);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			var g = this.UserInterface;
			
			g.FillRectangle(this.GetClientRectangle(), this.Background);
		}

		/// <summary>
		/// Gets a rectangle that defines where the widget is located.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetClientRectangle() => new Rectangle(
			(int)this.Position.X, (int)this.Position.Y,
			(int)this.Size.Width, (int)this.Size.Height);

		/// <summary>
		/// Gets or sets the background color of this widget.
		/// </summary>
		public Color Background
		{
			get { return Get<Color>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets or sets the foreground color of this widget.
		/// </summary>
		public Color Foreground
		{
			get { return Get<Color>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		/// <summary>
		/// Gets the associated user interface.
		/// </summary>
		public UserInterface UserInterface { get; internal set; }
	}
}
