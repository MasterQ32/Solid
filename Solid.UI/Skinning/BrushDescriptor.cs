namespace Solid.UI.Skinning
{
	using Markup;
	using OpenTK.Graphics;
	using System;

	public abstract class BrushDescriptor : SolidObject
	{
		public abstract Brush CreateBrush();
	}

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

	public sealed class TextureBoxBrushDescriptor : BrushDescriptor, INamedNodeContainer
	{
		public static readonly SolidProperty EdgeWidthProperty = SolidProperty.Register<TextureBoxBrushDescriptor, int>(nameof(EdgeWidthProperty), 16);

		private BrushDescriptor[,] childBrushes = new BrushDescriptor[3, 3];

		public override Brush CreateBrush()
		{
			return new TextureBoxBrush()
			{
				BorderSize = this.EdgeWidth,
				TopLeft = childBrushes[0,0]?.CreateBrush(),
				TopMiddle = childBrushes[1, 0]?.CreateBrush(),
				TopRight = childBrushes[2, 0]?.CreateBrush(),
				MiddleLeft= childBrushes[0, 1]?.CreateBrush(),
				Center = childBrushes[1, 1]?.CreateBrush(),
				MiddleRight = childBrushes[2, 1]?.CreateBrush(),
				BottomLeft= childBrushes[0, 2]?.CreateBrush(),
				BottomCenter = childBrushes[1, 2]?.CreateBrush(),
				BottomRight = childBrushes[2, 2]?.CreateBrush(),
			};
		}

		public void SetChildNodeName(object c, string name)
		{
			var brushDescriptor = (BrushDescriptor)c;
			switch (name)
			{
				case "TopLeft": childBrushes[0, 0] = brushDescriptor; break;
				case "TopMiddle": childBrushes[1, 0] = brushDescriptor; break;
				case "TopRight": childBrushes[2, 0] = brushDescriptor; break;
				case "MiddleLeft": childBrushes[0, 1] = brushDescriptor; break;
				case "Center": childBrushes[1, 1] = brushDescriptor; break;
				case "MiddleRight": childBrushes[2, 1] = brushDescriptor; break;
				case "BottomLeft": childBrushes[0, 2] = brushDescriptor; break;
				case "BottomCenter": childBrushes[1, 2] = brushDescriptor; break;
				case "BottomRight": childBrushes[2, 2] = brushDescriptor; break;
				default: throw new InvalidOperationException($"{name} is not a valid texture box position.");
			}
		}

		public int EdgeWidth
		{
			get { return Get<int>(EdgeWidthProperty); }
			set { Set(EdgeWidthProperty, value); }
		}
	}
}