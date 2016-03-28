namespace Solid.UI
{
	using OpenTK.Graphics;
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Globalization;

	public class Color4Converter : TypeConverter
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
				return (Color4)(Color)colorConverter.ConvertFrom(context, culture, value);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}