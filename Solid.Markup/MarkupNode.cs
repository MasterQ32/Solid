using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solid.Markup
{
    public class MarkupNode
    {
		private readonly List<MarkupNode> children = new List<MarkupNode>();

		public IList<MarkupNode> Children => this.children;

		public string Type { get; set; }

		public string ID { get; set; }

		public IDictionary<string, MarkupProperty> Attributes { get; } = new Dictionary<string, MarkupProperty>();
	}
}
