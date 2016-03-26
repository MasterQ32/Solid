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

		public override string ToString()
		{
			return $"{this.Width} × {this.Height}";
		}
	}
}