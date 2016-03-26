using System;
using System.Collections;
using System.Collections.Generic;

namespace Solid.Markup
{
	public class TypeMapper : IDictionary<string, Type>
	{
		private readonly Dictionary<string, Type> types = new Dictionary<string, Type>();

		private void CheckType(Type type)
		{
			if (type.GetConstructor(Type.EmptyTypes) == null)
				throw new InvalidOperationException("The added type requires a default constructor");
		}

		public Type this[string key]
		{
			get
			{
				return types[key];
			}

			set
			{
				CheckType(value);
				types[key] = value;
			}
		}

		public string DefaultNamespace { get; set; } = null;

		public int Count => types.Count;

		public bool IsReadOnly => ((IDictionary<string, Type>)types).IsReadOnly;

		public ICollection<string> Keys => types.Keys;

		public ICollection<Type> Values => types.Values;

		public void Add(KeyValuePair<string, Type> item)
		{
			CheckType(item.Value);
			((IDictionary<string, Type>)types).Add(item);
		}

		public void Add(string key, Type value)
		{
			CheckType(value);
			types.Add(key, value);
		}

		public Type Get(string type)
		{
			if (this.ContainsKey(type))
				return this[type];
			if((type.Contains(".") == false) && (this.DefaultNamespace != null))
			{
				var t = Type.GetType(this.DefaultNamespace + "." + type, false);
				if (t != null)
					return t;
			}
			return Type.GetType(type, false);
		}

		public void Clear() => types.Clear();

		public bool Contains(KeyValuePair<string, Type> item) => ((IDictionary<string, Type>)types).Contains(item);

		public bool ContainsKey(string key) => types.ContainsKey(key);

		public void CopyTo(KeyValuePair<string, Type>[] array, int arrayIndex) => ((IDictionary<string, Type>)types).CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<string, Type>> GetEnumerator() => types.GetEnumerator();

		public bool Remove(KeyValuePair<string, Type> item) => ((IDictionary<string, Type>)types).Remove(item);

		public bool Remove(string key) => types.Remove(key);

		public bool TryGetValue(string key, out Type value) => types.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}