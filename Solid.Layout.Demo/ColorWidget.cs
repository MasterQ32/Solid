using System.Drawing;

namespace Solid.Layout.Demo
{
	class ColorWidget : Widget
	{
		public static readonly SolidProperty ColorProperty = SolidProperty.Register<ColorWidget, Color>(nameof(Color));

		public void Paint(Graphics g)
		{
			var rect = this.WidgetBounds;
			g.FillRectangle(
				new SolidBrush(this.Color),
				rect);
			g.DrawRectangle(
				Pens.Black,
				rect);
		}

		public Rectangle WidgetBounds => new Rectangle(
			(int)this.Position.X, (int)this.Position.Y,
			(int)this.Size.Width, (int)this.Size.Height);

		public Color Color
		{
			get { return Get<Color>(ColorProperty); }
			set { Set(ColorProperty, value); }
		}
	}
}