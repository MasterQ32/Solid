using OpenTK.Graphics;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Solid.UI
{
	public class BrushConverter : TypeConverter
	{
		private readonly ColorConverter colorConverter = new ColorConverter();

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				var text = (string)value;
				try
				{
					var color = (Color)colorConverter.ConvertFrom(context, culture, value);
					return new SolidBrush((Color4)color);
				}
				catch { }
				// TODO: Implement brush parsing

				// simple assumption: if brush is no color, the brush must be a texture brush
				return new TextureBrush(new Texture(text));

				throw new NotSupportedException("The given brush description is not supported.");
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}