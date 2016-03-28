namespace Solid.UI
{
	using Solid.Layout;

	public class Image : UIWidget
	{
		public static readonly SolidProperty SourceProperty = SolidProperty.Register<Image, Texture>(nameof(Source));

		public override Size SizeHint =>  this.Source?.Size ?? base.SizeHint;

		private TextureBrush brush = new TextureBrush();

		protected override void OnDrawPreChildren()
		{
			base.OnDrawPreChildren();

			var rect = GetClientRectangle();
			if(this.Source != null)
			{
				this.brush.Texture = this.Source;
				this.UserInterface.RenderBrush(this.brush, rect);
			}
		}

		public Texture Source
		{
			get { return Get<Texture>(SourceProperty); }
			set { Set(SourceProperty, value); }
		}
	}
}
