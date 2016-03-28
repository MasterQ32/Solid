namespace Solid.UI
{
	using OpenTK.Graphics;
	using SharpFont;
	using Layout;
	using System;
	public class Label : UIWidget
	{
		
		public static SolidProperty TextProperty = SolidProperty.Register<Label, string>(nameof(Text), "");

		public static SolidProperty FontProperty = SolidProperty.Register<Label, string>(nameof(Font), new SolidPropertyMetadata()
		{
			DefaultValue = "Font",
			InheritFromHierarchy = true,
		});

		public Label()
		{
			this.IsTouchable = false;
			this.VerticalAlignment = VerticalAlignment.Center;
			this.HorizontalAlignment = HorizontalAlignment.Center;
		}

		protected override void OnDrawPreChildren()
		{
			var g = this.UserInterface;

			base.OnDrawPreChildren();

			var rect = this.GetClientRectangle();

			rect.X = (int)Math.Round(rect.X, MidpointRounding.AwayFromZero);
			rect.Y = (int)Math.Round(rect.Y, MidpointRounding.AwayFromZero);

			g.RenderString(
				this.Text,
				rect,
				this.Font,
				false);
		}

		public override Size SizeHint
		{
			get
			{
				return this.UserInterface.RenderString(this.Text, this.GetClientRectangle(), this.Font, true).Size;
			}
		}

		public string Text
		{
			get { return Get<string>(TextProperty); }
			set { Set(TextProperty, value); }
		}

		public string Font
		{
			get { return Get<string>(FontProperty); }
			set { Set(TextProperty, value); }
		}
	}
}