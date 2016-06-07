namespace Solid.UI.Widgets
{
	using Solid.UI.Skinning;
	using Solid.Layout;

	public sealed class Image : UIWidget
	{
		public static readonly SolidProperty ImageProperty = SolidProperty.Register<Image, IPicture>(
			nameof(Picture));

		protected override void OnDrawPreChildren(IGraphics graphics)
		{
			base.OnDrawPreChildren(graphics);

			if (this.Picture != null)
			{
				graphics.DrawPicture(
					this.Picture,
					this.GetClientRectangle());
			}
		}

		public override Size SizeHint
		{
			get
			{
				var hint = base.SizeHint;
				if (hint.Width == 0) hint.Width = this.Picture?.Width ?? 0;
				if (hint.Height == 0) hint.Height = this.Picture?.Height ?? 0;
				return hint;
			}
		}

		public IPicture Picture
		{
			get { return Get<IPicture>(ImageProperty); }
			set { Set(ImageProperty, value); }
		}
	}
}
