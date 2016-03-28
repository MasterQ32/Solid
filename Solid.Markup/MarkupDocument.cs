using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Solid.Markup
{
	public sealed class MarkupDocument
	{
		public MarkupNode Root
		{
			get;
			set;
		}

		public T Map<T>(TypeMapper types)
			where T : class, new()
		{
			var root = Map(types);
			if (root is T)
				return (T)root;
			else
				throw new InvalidOperationException($"The root object is not of the type {typeof(T).Name}.");
		}

		public object Map(TypeMapper types)
		{
			var mapper = new NativeMapper(types);
			return mapper.Instantiate(this);
		}
		
	}
}