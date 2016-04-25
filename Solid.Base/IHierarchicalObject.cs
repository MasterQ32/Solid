using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid
{
	public interface IHierarchicalObject
	{
		event EventHandler<ParentChangedEventArgs> ParentChanged;

		IHierarchicalObject Parent { get; }

		IEnumerable<IHierarchicalObject> Children { get; }
	}

	public sealed class ParentChangedEventArgs : EventArgs
	{
		public ParentChangedEventArgs(IHierarchicalObject oldParent, IHierarchicalObject newParent)
		{
			this.NewParent = newParent;
			this.OldParent = oldParent;
		}

		public IHierarchicalObject NewParent { get; private set; }

		public IHierarchicalObject OldParent { get; private set; }
	}
}
