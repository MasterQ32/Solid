using SharpFont;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Solid.UI
{
	public class FaceConverter : TypeConverter
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
			if(value is string)
			{
				var text = (string)value;
				return new Face(UserInterface.FontLibrary, text, 0);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}