using System;
using System.ComponentModel;
using System.Globalization;

namespace Solid.UI.Skinning.Converters
{
	internal class BrushConverter : TypeConverter
	{
		private IGraphicsObjectFactory objectFactory;

		public BrushConverter(IGraphicsObjectFactory objectFactory)
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
				return this.objectFactory.CreateBrush((string)value);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}