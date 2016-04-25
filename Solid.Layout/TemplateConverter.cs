using Solid.Markup;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Solid.Layout
{
	public  class TemplateConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if(sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if(value is string)
			{
				var fileName = (string)value;
				return Parser.Load(fileName);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}