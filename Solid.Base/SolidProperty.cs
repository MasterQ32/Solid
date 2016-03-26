using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid
{
	public sealed class SolidProperty
	{
		private static readonly Dictionary<Type, Dictionary<string, SolidProperty>> registry = new Dictionary<Type, Dictionary<string, SolidProperty>>();

		private static Dictionary<string, SolidProperty> GetRegistryForType(Type type)
		{
			lock (registry)
			{
				if (registry.ContainsKey(type))
					return registry[type];
				var properties = new Dictionary<string, SolidProperty>();
				registry[type] = properties;
				return properties;
			}
		}

		public static SolidProperty GetProperty(Type type, string name)
		{
			var registry = GetRegistryForType(type);
			lock(registry)
			{
				if (registry.ContainsKey(name))
					return registry[name];
			}
			if (type.BaseType != null)
				return GetProperty(type.BaseType, name);
			else
				return null;
		}

		public static IDictionary<string, SolidProperty> GetProperties(Type type)
		{
			var registry = GetRegistryForType(type);
			IDictionary<string, SolidProperty> properties;
			lock (registry)
			{
				properties = registry.ToDictionary(k => k.Key, v => v.Value);
			}
			if(type.BaseType != null)
			{
				// recursivly find all properties....
				foreach(var property in GetProperties(type.BaseType))
				{
					if (properties.ContainsKey(property.Key) == false)
						properties[property.Key] = property.Value;
				}
			}
			return properties;
		}

		public static SolidProperty Register(
			Type objectType,
			string name,
			Type propertyType,
			SolidPropertyMetadata metaData = null)
		{
			var property = new SolidProperty(name, objectType, propertyType, metaData ?? new SolidPropertyMetadata());

			var registry = GetRegistryForType(objectType);
			lock(registry)
			{
				if (registry.ContainsKey(name))
					throw new InvalidOperationException($"The property {name} is already registered.");
				registry.Add(name, property);
			}
			return property;
		}

		public static SolidProperty Register<TObject, TProperty>(string propertyName, TProperty defaultValue = default(TProperty))
			where TObject : SolidObject
			=> Register(typeof(TObject), propertyName, typeof(TProperty), new SolidPropertyMetadata()
			{
				DefaultValue = defaultValue,
			});


		public static SolidProperty Register<TObject, TProperty>(string propertyName, SolidPropertyMetadata metaData)
			where TObject : SolidObject
			=> Register(typeof(TObject), propertyName, typeof(TProperty), metaData);

		private SolidProperty(
			string name, 
			Type targetType,
			Type propertyType,
			SolidPropertyMetadata meta)
		{
			this.Name = name;
			this.ObjectType = targetType;
			this.PropertyType = propertyType;
			this.Metadata = meta;
		}

		public object DefaultValue => this.Metadata?.DefaultValue;

		public bool IsExported => this.Metadata?.IsExported ?? true;

		public string Name { get; private set; }

		public Type ObjectType { get; private set; }

		public Type PropertyType { get; private set; }

		internal SolidPropertyMetadata Metadata { get; private set; }

		public void SetValue(SolidObject target, object value) => target.Set(this, value);

		public object GetValue(SolidObject target) => target.Get(this);

		public T GetValue<T>(SolidObject target) => (T)target.Get(this);
	}
}