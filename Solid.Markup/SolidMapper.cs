﻿using System;
using System.Collections.Generic;

namespace Solid.Markup
{
	public abstract class SolidMapper<T> : MarkupMapper<T>
		where T : SolidObject
	{
		private readonly Dictionary<string, Func<T>> factories = new Dictionary<string, Func<T>>();

		public SolidMapper()
		{
			
		}

		public void RegisterType<TNode>()
			where TNode : T, new()
			=> this.RegisterType<TNode>(typeof(TNode).Name);

		public void RegisterType<TNode>(string name)
			where TNode : T, new ()
		{
			if (factories.ContainsKey(name))
				throw new InvalidOperationException("The given type name is already registered.");
			factories.Add(name, () => new TNode());
		}

		public void RegisterType(Type type) => RegisterType(type, type.Name);

		public void RegisterType(Type type, string name)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (name == null) throw new ArgumentNullException(nameof(name));
			if (type.IsSubclassOf(typeof(T)) == false)
				throw new ArgumentException($"{nameof(type)} must derive from {typeof(T).Name}.", nameof(type));
			var ctor = type.GetConstructor(Type.EmptyTypes);
			if (ctor == null)
				throw new InvalidOperationException("The given type must feature a public default constructor.");
			factories.Add(name, () => (T)ctor.Invoke(null));
        }

		protected override T CreateNode(string nodeClass)
		{
			if (factories.ContainsKey(nodeClass))
				return factories[nodeClass]();
			else
				throw new NotSupportedException($"{nodeClass} is not a supported type.");
		}

		protected override void SetProperty(T obj, MarkupProperty markupProperty)
		{
			var property = SolidProperty.GetProperty(obj.GetType(), markupProperty.Name);
			if(property == null)
			{
				base.SetProperty(obj, markupProperty);
				return;
			}

			if (markupProperty.Type == MarkupPropertyType.Binding)
			{
				SolidProperty.Bind(property, obj, markupProperty.Value);
				return;
			}

			if (property.Metadata.IsExported == false)
				throw new NotSupportedException("Setting a non-exported property is not supported.");
			
			property.SetValue(obj, ConvertPropertyType(property.PropertyType, markupProperty));
		}
	}
}