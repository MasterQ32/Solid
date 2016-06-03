namespace Solid.UI.Converters
{
	using Skinning;
	using System;
	using System.ComponentModel;
	using System.Globalization;

	public class ColorConverter : TypeConverter
	{
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

				var splits = text.Split(';');
				if(splits.Length == 3)
				{
					return new Color(
						(1.0f / 255.0f) * float.Parse(splits[0], culture),
						(1.0f / 255.0f) * float.Parse(splits[1], culture),
						(1.0f / 255.0f) * float.Parse(splits[2], culture));
				}

				// TODO: Implement color converter
				return new Color(1.0f, 0.0f, 0.0f);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}