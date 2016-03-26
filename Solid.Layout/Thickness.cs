namespace Solid.Layout
{
	public struct Thickness
	{
		public float Left, Right, Top, Bottom;

		public Thickness(float all) : 
			this(all, all)
		{

		}

		public Thickness(float horizontal, float vertical) : 
			this(horizontal, horizontal, vertical, vertical)
		{
			
		}

		public Thickness(float left, float right, float top, float bottom)
		{
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
		}
	}
}