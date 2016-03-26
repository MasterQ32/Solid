using Solid.Markup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Layout
{
	public class Layout : IMarkupDocument<Widget>
	{
		public Layout()
		{

		}

		public Widget Root { get; set; }

		public void Update(Size size)
		{
			var root = this.Root;

			if (root == null)
				throw new InvalidOperationException("Cannot update a layout with no root object.");

			root.ApplyAlignment(new Point(0, 0), size);
			root.SetupLayout();
			root.AfterLayout();
		}

		public Layout FromString(string source) => this.Load(new StringReader(source));

		public Layout Load(TextReader source)
		{
			var document = Parser.Parse(source);
			var mapper = new LayoutMapper();
			return mapper.Instantiate(document);
		}

		void IMarkupDocument<Widget>.SetRoot(Widget root)
		{
			this.Root = root;
		}

		void IMarkupDocument<Widget>.SetNodeName(Widget node, string name)
		{
			// discard
		}
	}
}
