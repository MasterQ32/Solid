using System.Collections.Generic;

namespace Solid.Markup
{
	public sealed class MarkupDocument
	{
		private readonly Dictionary<string, MarkupNode> namedNodes = new Dictionary<string, MarkupNode>();

		public MarkupNode this[string id]
		{
			get
			{
				if (this.namedNodes.ContainsKey(id))
					return this.namedNodes[id];
				else
					return null;
			}
			set
			{
				if (value == null)
					this.namedNodes.Remove(id);
				else
					this.namedNodes[id] = value;
			}
		}

		public MarkupNode Root
		{
			get;
			set;
		}
	}
}