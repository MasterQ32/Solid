using static System.Math;

namespace Solid.Layout
{
	public class StackLayout : Widget
	{
		public static readonly SolidProperty DirectionProperty = SolidProperty.Register<StackLayout, StackDirection>(nameof(Direction), StackDirection.Horizontal);

		public StackDirection Direction
		{
			get { return Get<StackDirection>(DirectionProperty); }
			set { Set(DirectionProperty, value); }
		}

		protected override void OnLayout()
		{
			if (this.Direction == StackDirection.Vertical)
			{
				float offset = 0;
				Size size = new Size(this.Size.Width, 0);
				foreach (Widget child in this.Children)
				{
					size.Height = child.WidgetSize.Height;

					child.ApplyAlignment(new Point(0, offset), size);

					offset += size.Height;
					child.SetupLayout();
				}
			}
			else
			{
				float offset = 0;
				Size size = new Size(0, this.Size.Height);
				foreach (Widget child in this.Children)
				{
					size.Width = child.WidgetSize.Width;

					child.ApplyAlignment(new Point(offset, 0), size);

					offset += size.Width;
					child.SetupLayout();
				}
			}
		}

		public override Size SizeHint
		{
			get
			{
				Size size = this.DeclaredSize;
				bool modifyX = (size.Width == 0);
				bool modifyY = (size.Height == 0);
				if (this.Direction == StackDirection.Vertical)
				{
					foreach (Widget child in this.Children)
					{
						Size s = child.WidgetSize;
						if (modifyX) size.Width = Max(size.Width, s.Width);
						if (modifyY) size.Height += s.Height;
					}
				}
				else
				{
					foreach (Widget child in this.Children)
					{
						Size s = child.WidgetSize;
						if (modifyX) size.Width += s.Width;
						if (modifyY) size.Height = Max(size.Height, s.Height);
					}
				}
				return size;
			}
		}
	}

	public enum StackDirection
	{
		Horizontal,
		Vertical
	}
}