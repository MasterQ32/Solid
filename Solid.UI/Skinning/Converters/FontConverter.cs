using System;
using System.ComponentModel;
using System.Globalization;

namespace Solid.UI.Skinning.Converters
{
	public sealed class FontConverter : TypeConverter
	{
		private readonly IGraphicsObjectFactory objectFactory;

		public FontConverter(IGraphicsObjectFactory objectFactory)
		{
			this.objectFactory = objectFactory;
		}

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
				return this.objectFactory.CreateFont((string)value);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}