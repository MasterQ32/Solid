using System;
using Solid.Markup;

namespace Solid.Layout
{
	public class LayoutMapper : SolidMapper<Widget>
	{
		public LayoutMapper()
		{
		}

		protected override IMarkupDocument<Widget> CreateDocument()
		{
			return new Layout();
		}

		protected override Widget CreateNode(string nodeClass)
		{
			switch(nodeClass)
			{
				case "StackPanel": return new StackPanel();
				case "DockPanel": return new DockPanel();
				case "Canvas": return new Canvas();
				case "Table": return new Table();
				case "Widget": return new Widget();
				default: return base.CreateNode(nodeClass);
			}
		}

		protected override void AddChildNode(Widget parent, Widget child) => parent.Children.Add(child);

		public Layout Instantiate(MarkupDocument document) => (Layout)this.Map(document);
	}
}