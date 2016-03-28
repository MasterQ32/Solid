using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Markup
{
	/// <summary>
	/// A mapper that will map a MarkupDocument to a custom type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class MarkupMapper<T>
		where T : class
	{
		private readonly Dictionary<Type, TypeConverter> converters = new Dictionary<Type, TypeConverter>();

		private bool swallowsNamedNodeInDocument;

		/// <summary>
		/// Gets or sets if a named node will not be emitted to IMarkupDocument&lt;T&gt; when the node was registered in its INamedNodeContainer&lt;T&gt;.
		/// </summary>
		protected bool SwallowsNamedNodeInDocument
		{
			get { return this.swallowsNamedNodeInDocument; }
			set { this.swallowsNamedNodeInDocument = value; }
		}

		protected MarkupMapper()
		{
			this.RegisterConverter<decimal, DecimalConverter>();
			this.RegisterConverter<CultureInfo, CultureInfoConverter>();
			this.RegisterConverter<DateTime, DateTimeConverter>();
			this.RegisterConverter<TimeSpan, TimeSpanConverter>();
			this.RegisterConverter<Guid, GuidConverter>();
			this.RegisterConverter<bool, BooleanConverter>();
		}

		public void RegisterConverter<TProperty, TConverter>()
			where TConverter : TypeConverter, new()
			=> RegisterConverter(typeof(TProperty), new TConverter());

		public void RegisterConverter<TProperty>(TypeConverter converter) => RegisterConverter(typeof(TProperty), converter);

		public void RegisterConverter(Type propertyType, TypeConverter converter)
		{
			if (converters.ContainsKey(propertyType))
				throw new InvalidOperationException("A converter for this type is already registered.");
			this.converters.Add(propertyType, converter);
		}

		protected abstract T CreateNode(string nodeClass);

		protected abstract IMarkupDocument<T> CreateDocument();

		protected virtual void AddChildNode(T parent, T child)
		{
			var type = parent.GetType();
			var addMethods = type
				.GetMethods()
				.Where(m => (m.Name == "Add"))
				.Where(m => (m.GetParameters().Length == 1))
				.ToDictionary(m => m.GetParameters()[0].ParameterType);

			var childType = child.GetType();

			while (childType != null)
			{
				if (addMethods.ContainsKey(childType))
				{
					addMethods[childType].Invoke(parent, new object[] { child });
					break;
				}
				childType = childType.BaseType;
			}
			if (childType == null)
				throw new InvalidOperationException($"Could not find any Add method that can add a {child.GetType().Name}.");
		}

		protected virtual void SetProperty(T obj, MarkupProperty markupProperty)
		{
			var type = obj.GetType();
			var property = type.GetProperty(markupProperty.Name);
			if (property == null)
				throw new InvalidOperationException($"{type.Name} does not have a property {markupProperty.Name}.");

			SetPropertyValue(obj, property, markupProperty);
		}

		protected IMarkupDocument<T> Map(MarkupDocument document)
		{
			var doc = CreateDocument();

			doc.SetRoot(Map(doc, document.Root, null));

			return doc;
		}

		private T Map(IMarkupDocument<T> doc, MarkupNode node, T parent)
		{
			var obj = this.CreateNode(node.Type);
			var type = obj.GetType();

			var addMethods = type
				.GetMethods()
				.Where(m => (m.Name == "Add"))
				.Where(m => (m.GetParameters().Length == 1))
				.ToDictionary(m => m.GetParameters()[0].ParameterType);

			foreach (var attrib in node.Attributes)
			{
				this.SetProperty(obj, attrib.Value);
			}

			foreach (var child in node.Children)
			{
				var childInstance = Map(doc, child, obj);

				this.AddChildNode(obj, childInstance);

				if ((child.ID != null) && (obj is INamedNodeContainer<T>))
				{
					((INamedNodeContainer<T>)obj).SetChildNodeName(childInstance, child.ID);
				}
			}

			if ((node.ID != null) && (!(parent is INamedNodeContainer<T>) || !this.swallowsNamedNodeInDocument))
				doc.SetNodeName(obj, node.ID);

			doc.NotifyCreateNode(obj);

			return obj;
		}

		private void SetPropertyValue(object obj, PropertyInfo target, MarkupProperty source)
		{
			target.SetValue(obj, ConvertPropertyType(target.PropertyType, source));
		}

		protected object ConvertPropertyType(Type targetType, MarkupProperty source)
		{
			if (converters.ContainsKey(targetType))
			{
				var converter = converters[targetType];
				return converter.ConvertFromInvariantString(source.Value);
			}
			else if (targetType.IsEnum)
			{
				if (source.Type != MarkupPropertyType.Enumeration)
					throw new InvalidOperationException($"Expected enumeration value for {source.Name}.");
				return Enum.Parse(targetType, source.Value);
			}
			else if (targetType == typeof(string))
			{
				if (source.Type != MarkupPropertyType.String)
					throw new InvalidOperationException($"Expected string value for {source.Name}.");
				return source.Value;
			}
			else if (targetType.IsPrimitive)
			{
				if (source.Type != MarkupPropertyType.Number)
					throw new InvalidOperationException($"Expected number value for {source.Name}.");
				return Convert.ChangeType(source.Value, targetType, CultureInfo.InvariantCulture);
			}
			else
			{
				throw new NotSupportedException($"{targetType.Name} is not a supported type.");
			}
		}
	}
}
