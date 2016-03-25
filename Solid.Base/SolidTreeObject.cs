using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solid
{
	public class SolidTreeObject<T> : SolidTreeBase
		where T : SolidTreeBase
	{
		private readonly new ChildCollection<T> children;

		public SolidTreeObject() :
			base()
		{
			this.children = new ChildCollection<T>(this);
			base.children = (ICollection<SolidTreeBase>)this.children;
		}

		public T Parent
		{
			get { return (T)this.parent; }
			set
			{
				this.parent?.children.Remove(this);
				// this.parent = value; <- this is implicitly by the following line.
				value?.children.Add(this);
			}
		}

		public ICollection<T> Children => this.children;
	}

	public class SolidTreeBase : SolidObject
	{
		internal SolidTreeBase parent;
		internal ICollection<SolidTreeBase> children;

		internal SolidTreeBase()
		{

		}

		internal class ChildCollection<T> : ICollection<T>
			where T : SolidTreeBase
		{
			private readonly HashSet<T> children = new HashSet<T>();
			private SolidTreeBase container;

			public ChildCollection(SolidTreeBase container)
			{
				this.container = container;
			}

			public void Add(T item)
			{
				if (item.parent == this.container)
					return;
				if (item.parent != null)
				{
					item.parent.children.Remove(item);
				}
				item.parent = this.container;
				children.Add(item);
			}

			public void Clear()
			{
				foreach (var child in this.children)
					child.parent = null;
				children.Clear();
			}

			public bool Remove(T item)
			{
				if (item == null)
					throw new ArgumentNullException(nameof(item));
				if (item.parent != this.container)
					throw new InvalidOperationException("The given item is not a child of the collection.");
				item.parent = null;
				return children.Remove(item);
			}

			public int Count => children.Count;

			public bool IsReadOnly => false;

			public bool Contains(T item) => children.Contains(item);

			public void CopyTo(T[] array, int arrayIndex) => children.CopyTo(array, arrayIndex);

			public IEnumerator<T> GetEnumerator() => children.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
		}
	}
}
