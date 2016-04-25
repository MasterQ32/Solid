using static System.Math;

namespace Solid.Layout
{
	public class DockPanel : Panel
	{
		public static readonly SolidProperty DockProperty = SolidProperty.Register<Widget, DockStyle>("DockPanel.Dock", DockStyle.Left);

		public override Size SizeHint
		{
			get
			{
				Size childSize;

				// Trivial
				if (this.Children.Count == 0)
					return base.SizeHint;

				Size size = this.DeclaredSize;
				Size minSize = new Size(0, 0);

				bool modifyX = (size.Width <= 0);
				bool modifyY = (size.Height <= 0);

				for (int i = 0; i < this.Children.Count - 1; i++)
				{
					var child = this.Children[i];
					childSize = child.WidgetSize;
					DockStyle style = DockProperty.GetValue<DockStyle>(child);
					switch (style)
					{
						case DockStyle.Left:
						case DockStyle.Right:
						{
							if (modifyX) size.Width += childSize.Width;
							minSize.Height = Max(minSize.Height, childSize.Height);
							break;
						}
						case DockStyle.Top:
						case DockStyle.Bottom:
						{
							minSize.Width = Max(minSize.Width, childSize.Width);
							if (modifyY) size.Height += childSize.Height;
							break;
						}
					}
				}
				childSize = this.Children[this.Children.Count - 1].WidgetSize;
				if (modifyX) size.Width += childSize.Width;
				if (modifyY) size.Height += childSize.Height;
				minSize.Width = Max(minSize.Width, childSize.Width);
				minSize.Height = Max(minSize.Height, childSize.Height);

				if (modifyX) size.Width = Max(size.Width, minSize.Width);
				if (modifyY) size.Height = Max(size.Height, minSize.Height);

				if (modifyX) size.Width += this.Padding.Left + this.Padding.Right;
				if (modifyY) size.Height += this.Padding.Top + this.Padding.Bottom;

				return size;
			}
		}

		protected override void OnLayout()
		{
			// Trivial
			if (this.Children.Count == 0)
				return;

			Point position = this.ClientPosition;
			Size size = this.ClientSize;

			for (int i = 0; i < this.Children.Count - 1; i++)
			{
				var child = this.Children[i];
				Size childSize = child.WidgetSize;
				DockStyle style = DockProperty.GetValue<DockStyle>(child);
				switch (style)
				{
					case DockStyle.Left:
					{
						child.ApplyAlignment(position, new Size(childSize.Width, size.Height));
						position.X += childSize.Width;
						size.Width -= childSize.Width;
						break;
					}
					case DockStyle.Right:
					{
						child.ApplyAlignment(new Point(position.X + size.Width - childSize.Width, position.Y), new Size(childSize.Width, size.Height));
						size.Width -= childSize.Width;
						break;
					}
					case DockStyle.Top:
					{
						child.ApplyAlignment(position, new Size(size.Width, childSize.Height));
						position.Y += childSize.Height;
						size.Height -= childSize.Height;
						break;
					}
					case DockStyle.Bottom:
					{
						child.ApplyAlignment(new Point(position.X, position.Y + size.Height - childSize.Height), new Size(size.Width, childSize.Height));
						size.Height -= childSize.Height;
						break;
					}
				}
				child.SetupLayout();
			}
			var last = this.Children[this.Children.Count - 1];
			last.ApplyAlignment(position, size);
			last.SetupLayout();
		}
	}

	public enum DockStyle
	{
		Left,
		Right,
		Top,
		Bottom,
	}
}