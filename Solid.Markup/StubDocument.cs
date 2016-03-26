using System;

namespace Solid.Markup
{
	internal class StubDocument : IMarkupDocument<object>
	{
		public event EventHandler<NodeEventArgs> NamedNodeEmitted;

		public object Root { get; set; }

		public void NotifyCreateNode(object node)
		{
			// discard
		}

		public void SetNodeName(object node, string name)
		{
			this.NamedNodeEmitted?.Invoke(this, new NodeEventArgs()
			{
				Name = name,
				Node = node,
			});
		}

		public void SetRoot(object root)
		{
			this.Root = root;
		}
	}
}