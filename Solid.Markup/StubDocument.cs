using System;

namespace Solid.Markup
{
	internal class StubDocument : IMarkupDocument<object>
	{
		public object Root { get; set; }

		public void SetNodeName(object node, string name)
		{
			// Discard
		}

		public void SetRoot(object root)
		{
			this.Root = root;
		}
	}
}