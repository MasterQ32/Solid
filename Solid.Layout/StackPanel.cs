﻿using static System.Math;

namespace Solid.Layout
{
	public class StackPanel : Panel
	{
		public static readonly SolidProperty DirectionProperty = SolidProperty.Register<StackPanel, StackDirection>(nameof(Direction), StackDirection.Horizontal);

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
				Size size = new Size(this.ClientSize.Width, 0);
				foreach (Widget child in this.Children)
				{
					size.Height = child.WidgetSize.Height;

					child.ApplyAlignment(new Point(this.Padding.Left, this.Padding.Top + offset), size);

					offset += size.Height;
					child.SetupLayout();
				}
			}
			else
			{
				float offset = 0;
				Size size = new Size(0, this.ClientSize.Height);
				foreach (Widget child in this.Children)
				{
					size.Width = child.WidgetSize.Width;

					child.ApplyAlignment(new Point(this.Padding.Left + offset, this.Padding.Top), size);

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

				if (modifyX) size.Width += this.Padding.Left + this.Padding.Right;
				if (modifyY) size.Height += this.Padding.Top + this.Padding.Bottom;
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