using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI
{
	public class RectangleConverter : TypeConverter
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
				var parts = text.Split(';').Select(x => x.Trim()).Select(s => float.Parse(s, culture)).ToArray();
				return new Rectangle(parts[0], parts[1], parts[2], parts[3]);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
