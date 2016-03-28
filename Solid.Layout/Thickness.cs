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

		/// <summary>
		/// Gets the total padding in vertical direction (Top + Bottom)
		/// </summary>
		public float Vertical => this.Top + this.Bottom;

		/// <summary>
		/// Gets the total padding in horizontal direction (Left + Right)
		/// </summary>
		public float Horizontal => this.Left + this.Right;
	}
}