using System;

namespace Solid.UI.Skinning
{
	public sealed class TextureBoxBrush : LogicBrush
	{
		public override void Draw(UserInterface g, Rectangle target)
		{
			var x0 = target.X;
			var x1 = target.X + this.BorderSize;
			var x2 = target.X + target.Width - this.BorderSize;
			var x3 = target.X + target.Width;

			var y0 = target.Y;
			var y1 = target.Y + this.BorderSize;
			var y2 = target.Y + target.Height - this.BorderSize;
			var y3 = target.Y + target.Height;

			var s = this.BorderSize;
			var sx = target.Width - 2 * this.BorderSize;
			var sy = target.Height - 2 * this.BorderSize;

			g.RenderBrush(this.TopLeft, new Rectangle(x0, y0, s, s));
			g.RenderBrush(this.TopMiddle, new Rectangle(x1, y0, sx, s));
			g.RenderBrush(this.TopRight, new Rectangle(x2, y0, s, s));
			g.RenderBrush(this.MiddleLeft, new Rectangle(x0, y1, s, sy));
			g.RenderBrush(this.Center, new Rectangle(x1, y1, sx, sy));
			g.RenderBrush(this.MiddleRight, new Rectangle(x2, y1, s, sy));
			g.RenderBrush(this.BottomLeft, new Rectangle(x0, y2, s, s));
			g.RenderBrush(this.BottomCenter, new Rectangle(x1, y2, sx, s));
			g.RenderBrush(this.BottomRight, new Rectangle(x2, y2, s, s));
		}

		public float BorderSize { get; set; } = 16.0f;

		public Brush TopLeft { get; set; }

		public Brush TopMiddle { get; set; }

		public Brush TopRight { get; set; }

		public Brush MiddleLeft { get; set; }

		public Brush Center { get; set; }

		public Brush MiddleRight { get; set; }

		public Brush BottomLeft { get; set; }

		public Brush BottomCenter { get; set; }
		
		public Brush BottomRight { get; set; }
	}
}