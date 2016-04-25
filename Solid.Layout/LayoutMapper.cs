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

			RegisterConverter<Thickness, ThicknessConverter>();
			RegisterConverter<MarkupDocument, TemplateConverter>();
		}

		protected override IMarkupDocument<Widget> CreateDocument() => new LayoutDocument();

		protected override bool AddChildNode(Widget parent, Widget child)
		{
			parent.Children.Add(child);
			return true;
		}

		public LayoutDocument Instantiate(MarkupDocument document) => (LayoutDocument)this.Map(document);
	}
}