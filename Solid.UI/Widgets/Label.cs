namespace Solid.UI.Widgets
{
	using System;
	using Solid.Layout;
	using Solid.UI.Skinning;

	public class Label : UIWidget
	{
		public static SolidProperty TextProperty = SolidProperty.Register<Label, string>(nameof(Text), "");

		public static SolidProperty FontProperty = SolidProperty.Register<Label, IFont>(nameof(Font), new SolidPropertyMetadata()
		{
			InheritFromHierarchy = true,
		});

		public static SolidProperty FontColorProperty = SolidProperty.Register<Label, Color>(nameof(FontColor), new SolidPropertyMetadata()
		{
			DefaultValue = new Color(0, 0, 0),
			InheritFromHierarchy = true,
		});

		private static int TextPadding = 2;
		private static int TextPadding2 => 2 * TextPadding;

		public Label()
		{
			this.IsTouchable = false;
			this.VerticalAlignment = VerticalAlignment.Center;
			this.HorizontalAlignment = HorizontalAlignment.Center;

			ExtendDefaultGen(FontProperty, (style) => style.Font);
			ExtendDefaultGen(FontColorProperty, (style) => style.FontColor);
		}

		protected override void OnDrawPreChildren(IGraphics graphics)
		{
			base.OnDrawPreChildren(graphics);

			var rect = this.GetClientRectangle();

			rect.X = (int)Math.Round(rect.X, MidpointRounding.AwayFromZero) + TextPadding;
			rect.Y = (int)Math.Round(rect.Y, MidpointRounding.AwayFromZero) + TextPadding;
			rect.Width -= TextPadding2;
			rect.Height -= TextPadding2;
			if (this.Font == null)
				return;
			graphics.DrawString(
				this.Font,
				this.FontColor,
				rect,
				this.Text);
		}

		public override Size SizeHint
		{
			get
			{
				var size = this.Font.Measure(this.Text);
				size.Width += TextPadding2;
				size.Height += TextPadding2;
				return size;
			}
		}

		public string Text
		{
			get { return Get<string>(TextProperty); }
			set { Set(TextProperty, value); }
		}

		public IFont Font
		{
			get { return Get<IFont>(FontProperty); }
			set { Set(FontProperty, value); }
		}

		public Color FontColor
		{
			get { return Get<Color>(FontColorProperty); }
			set { Set(FontColorProperty, value); }
		}
	}
}