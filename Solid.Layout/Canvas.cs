namespace Solid.Layout
{
	using static System.Math;

	public class Canvas : Panel
	{
		public static readonly SolidProperty LeftProperty = SolidProperty.Register<Widget, float>("Canvas.Left");
		public static readonly SolidProperty TopProperty = SolidProperty.Register<Widget, float>("Canvas.Top");

		public override Size SizeHint
		{
			get
			{
				var minSize = new Size();
				foreach (var child in this.Children)
				{
					minSize.Width = Max(minSize.Width, LeftProperty.GetValue<float>(child) + child.WidgetSize.Width);
					minSize.Height = Max(minSize.Height, TopProperty.GetValue<float>(child) + child.WidgetSize.Height);
				}
				minSize.Width += this.Padding.Left + this.Padding.Right;
				minSize.Height += this.Padding.Top + this.Padding.Bottom;
				return minSize;
			}
		}

		protected override void OnLayout()
		{
			foreach (var child in this.Children)
			{
				var position = new Point(
					LeftProperty.GetValue<float>(child) + this.Padding.Left,
					TopProperty.GetValue<float>(child) + this.Padding.Top);
				var size = child.WidgetSize;

				child.ApplyAlignment(position, size);
				child.SetupLayout();
			}
		}
	}
}