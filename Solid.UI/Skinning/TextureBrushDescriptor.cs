using System.Drawing;

namespace Solid.UI.Skinning
{
	public sealed class TextureBrushDescriptor : BrushDescriptor
	{
		public static readonly SolidProperty SourceProperty = SolidProperty.Register<TextureBrushDescriptor, string>(nameof(Source));

		public static readonly SolidProperty RectProperty = SolidProperty.Register<TextureBrushDescriptor, Rectangle>(nameof(Rect));

		public override Brush CreateBrush()
		{
			if ((Rect.Width > 0) && (Rect.Height > 0))
			{
				using (var bmp = new Bitmap(this.Source))
				{
					using (var target = new Bitmap((int)this.Rect.Width, (int)this.Rect.Height))
					{
						using (var g = Graphics.FromImage(target))
						{
							g.DrawImage(
								bmp,
								new RectangleF(0, 0, target.Width, target.Height),
								new RectangleF(this.Rect.X, this.Rect.Y, this.Rect.Width, this.Rect.Height),
								GraphicsUnit.Pixel);
						}
						return new TextureBrush(new Texture(target));
					}
				}
			}
			else {
				return new TextureBrush(new Texture(this.Source));
			}
		}

		public string Source
		{
			get { return Get<string>(SourceProperty); }
			set { Set(SourceProperty, value); }
		}

		public Rectangle Rect
		{
			get { return Get<Rectangle>(RectProperty); }
			set { Set(RectProperty, value); }
		}
	}
}