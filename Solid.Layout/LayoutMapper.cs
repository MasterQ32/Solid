using System;
using Solid.Markup;

namespace Solid.Layout
{
	public class LayoutMapper : SolidMapper<Widget>
	{
		public LayoutMapper()
		{
			RegisterType<StackPanel>();
			RegisterType<DockPanel>();
			RegisterType<Canvas>();
			RegisterType<Table>();
			RegisterType<Widget>();
		}

		protected override IMarkupDocument<Widget> CreateDocument() => new LayoutDocument();
		
		protected override void AddChildNode(Widget parent, Widget child) => parent.Children.Add(child);

		public LayoutDocument Instantiate(MarkupDocument document) => (LayoutDocument)this.Map(document);
	}
}