using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Solid.Layout
{
	public sealed class ThicknessConverter : TypeConverter
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
				var text = (string)value;
				var items = text.Split(';').Select(s => float.Parse(s, culture)).ToArray();
				switch(items.Length)
				{
					case 1: return new Thickness(items[0]);
					case 2: return new Thickness(items[0], items[1]);
					case 3: return new Thickness(items[0], items[2], items[1], items[3]);
					default: throw new NotSupportedException($"{items.Length} is not a valid number of items for a thickness."); 
				}
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}