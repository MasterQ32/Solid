namespace Solid.UI.Skinning
{
	using OpenTK.Graphics;
	using System;

	public sealed class SolidBrushDescriptor : BrushDescriptor
	{
		public static readonly SolidProperty ColorProperty = SolidProperty.Register<SolidBrushDescriptor, Color4>(nameof(Color));

		public override Brush CreateBrush()
		{
			return new SolidBrush(this.Color);
		}

		public Color4 Color
		{
			get { return Get<Color4>(ColorProperty); }
			set { Set(ColorProperty, value); }
		}
	}
}