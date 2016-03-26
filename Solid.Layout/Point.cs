namespace Solid.Layout
{
	public struct Point
	{
		public float X, Y;

		public Point(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public static Point Add(Point lhs, Point rhs) => new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);

		public static Point operator +(Point lhs, Point rhs) => Add(lhs, rhs);

		public override string ToString()
		{
			return $"{this.X}; {this.Y}";
        }
	}
}