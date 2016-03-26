using Solid.Markup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Layout
{
	public class LayoutDocument : IMarkupDocument<Widget>
	{
		public LayoutDocument()
		{

		}

		public Widget Root { get; set; }

		public void Update(Size size)
		{
			this.OnPreLayout(size);

			var root = this.Root;

			if (root == null)
				throw new InvalidOperationException("Cannot update a layout with no root object.");

			root.ApplyAlignment(new Point(0, 0), size);
			root.SetupLayout();
			root.AfterLayout();

			this.OnPostLayout(size);
		}

		public LayoutDocument FromString(string source) => this.Load(new StringReader(source));

		public LayoutDocument Load(TextReader source)
		{
			var document = Parser.Parse(source);
			var mapper = new LayoutMapper();
			return mapper.Instantiate(document);
		}

		void IMarkupDocument<Widget>.SetRoot(Widget root)
		{
			this.Root = root;
		}

		void IMarkupDocument<Widget>.SetNodeName(Widget node, string name) => this.SetNodeName(node, name);

		void IMarkupDocument<Widget>.NotifyCreateNode(Widget node) => this.OnNodeCreation(node);

		protected virtual void OnNodeCreation(Widget node)
		{
			// discard
		}

		protected virtual void OnPreLayout(Size size)
		{
			// discard
		}

		protected virtual void OnPostLayout(Size size)
		{
			// discard
		}

		protected virtual void SetNodeName(Widget node, string name)
		{
			// discard
		}
	}
}
