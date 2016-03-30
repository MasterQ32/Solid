using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid
{
	/// <summary>
	/// A bindable property.
	/// </summary>
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

		/// <summary>
		/// Gets a named property of a given type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Binds a solid property to a binding source property. This binding is only active when a binding source is given, otherwise it won't to shit.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property">The property to bind.</param>
		/// <param name="obj">The object for which the property is bound.</param>
		/// <param name="targetPropertyName">The property of the binding source.</param>
		/// <remarks>If name is null, any previous bindings will be undone.</remarks>
		/// <remarks>There can only be a single binding per property and object.</remarks>
		public static void Bind<T>(SolidProperty property, T obj, string targetPropertyName) where T : SolidObject
		{
			if (property == null)
				throw new ArgumentNullException(nameof(property));
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));
			obj.SetPropertyBinding(property, targetPropertyName);
		}

		/// <summary>
		/// Gets a list of all properties for the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Registers a property for a given type.
		/// </summary>
		/// <param name="objectType">The type of the class the property is defined on.</param>
		/// <param name="name">The name of the property.</param>
		/// <param name="propertyType">The type of the property value.</param>
		/// <param name="metaData">Some metadata that allows specification of the properties behaviour.</param>
		/// <returns>Registered property</returns>
		/// <exception cref="System.InvalidOperationException">Is thrown when a property with the given name is already registered.</exception>
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

		/// <summary>
		/// Registers a property for a given type.
		/// </summary>
		/// <typeparam name="TObject">The type of the class the property is defined on.</typeparam>
		/// <typeparam name="TProperty">The type of the property value.</typeparam>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="defaultValue">The default value the property has.</param>
		/// <returns>Registered property</returns>
		/// <exception cref="System.InvalidOperationException">Is thrown when a property with the given name is already registered.</exception>
		public static SolidProperty Register<TObject, TProperty>(string propertyName, TProperty defaultValue = default(TProperty))
			where TObject : SolidObject
			=> Register(typeof(TObject), propertyName, typeof(TProperty), new SolidPropertyMetadata()
			{
				DefaultValue = defaultValue,
			});


		/// <summary>
		/// Registers a property for a given type.
		/// </summary>
		/// <typeparam name="TObject">The type of the class the property is defined on.</typeparam>
		/// <typeparam name="TProperty">The type of the property value.</typeparam>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="metaData">Some metadata that allows specification of the properties behaviour.</param>
		/// <returns>Registered property</returns>
		/// <exception cref="System.InvalidOperationException">Is thrown when a property with the given name is already registered.</exception>
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

		/// <summary>
		/// Gets the default value of the property.
		/// </summary>
		public object DefaultValue => this.Metadata?.DefaultValue;

		/// <summary>
		/// Gets if the property is exported to script languages.
		/// </summary>
		public bool IsExported => this.Metadata?.IsExported ?? true;

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the type the property is bound to.
		/// </summary>
		public Type ObjectType { get; private set; }

		/// <summary>
		/// Gets the type of the property value.
		/// </summary>
		public Type PropertyType { get; private set; }

		/// <summary>
		/// Gets the meta data for the property.
		/// </summary>
		public SolidPropertyMetadata Metadata { get; private set; }

		/// <summary>
		/// Sets the value on the target object.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="value"></param>
		public void SetValue(SolidObject target, object value) => target.Set(this, value);

		/// <summary>
		/// Gets the value from the target object.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public object GetValue(SolidObject target) => target.Get(this);

		/// <summary>
		/// Gets the value from the target object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target"></param>
		/// <returns></returns>
		public T GetValue<T>(SolidObject target) => (T)target.Get(this);

		public override string ToString() => $"{this.PropertyType.Name} {this.Name}";
	}
}