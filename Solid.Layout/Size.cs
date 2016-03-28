namespace Solid.Layout
{
	public struct Size
	{
		public float Width, Height;

		public Size(float width, float height)
		{
			this.Width = width;
			this.Height = height;
		}

		public static Size Empty { get; } = new Size(0, 0);

		public override string ToString()
		{
			return $"{this.Width} × {this.Height}";
		}
	}
}