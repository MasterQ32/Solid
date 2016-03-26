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
				case "StackLayout": return new StackLayout();
				case "DockLayout": return new DockLayout();
				case "Widget": return new Widget();
				default: return base.CreateNode(nodeClass);
			}
		}

		protected override void AddChildNode(Widget parent, Widget child) => parent.Children.Add(child);

		public Layout Instantiate(MarkupDocument document) => (Layout)this.Map(document);
	}
}