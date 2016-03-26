using System;

namespace Solid.Markup
{
	internal class StubDocument : IMarkupDocument<object>
	{
		public object Root { get; set; }

		public void NotifyCreateNode(object node)
		{
			// discard
		}

		public void SetNodeName(object node, string name)
		{
			// discard
		}

		public void SetRoot(object root)
		{
			this.Root = root;
		}
	}
}