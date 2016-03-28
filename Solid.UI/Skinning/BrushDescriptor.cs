namespace Solid.UI.Skinning
{
	using Markup;
	using OpenTK.Graphics;
	using System;

	public abstract class BrushDescriptor : SolidObject
	{
		public abstract Brush CreateBrush();
	}
}