using System;
using Solid.Layout;

namespace Solid.UI
{
	public struct Rectangle
	{
		public float X, Y, Width, Height;

		public Rectangle(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		public Rectangle(Point position, Size size) :
			this(position.X, position.Y, size.Width, size.Height)
		{

		}

		public Point Position
		{
			get { return new Point(this.X, this.Y); }
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		public Size Size
		{
			get { return new Size(this.Width, this.Height); }
			set
			{
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		public bool Contains(Point pt)
		{
			return
				(pt.X >= this.X) &&
				(pt.Y >= this.Y) &&
				(pt.X < (this.X + this.Width)) &&
				(pt.Y < (this.Y + this.Height));
		}

		public override string ToString() => $"{Position} -> {Size}";
	}
}