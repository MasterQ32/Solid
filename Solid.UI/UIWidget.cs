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
			InheritFromHierarchy = true,
		});

		public static readonly SolidProperty ForegroundProperty = SolidProperty.Register<UIWidget, Brush>(nameof(Foreground), new SolidPropertyMetadata()
		{
			DefaultValue = new SolidBrush(Color4.Black),
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
		/// Gets the associated user interface.
		/// </summary>
		public UserInterface UserInterface { get; internal set; }
	}
}
