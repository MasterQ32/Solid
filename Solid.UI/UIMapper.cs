using Solid.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solid.Markup;
using OpenTK.Graphics;
using System.Drawing;
using Solid.UI.Skinning;
using SharpFont;

namespace Solid.UI
{
	public class UIMapper : LayoutMapper
	{
		protected override IMarkupDocument<Widget> CreateDocument() => new UserInterface();

		public UIMapper()
		{
			this.RegisterConverter<Color, ColorConverter>();
			this.RegisterConverter<Brush, BrushConverter>();
			this.RegisterConverter<Skin, SkinConverter>();
			this.RegisterConverter<Rectangle, RectangleConverter>();
			this.RegisterConverter<Face, FaceConverter>();

			this.RegisterType<Panel>();
			this.RegisterType<Button>();
			this.RegisterType<Label>();
		}

		protected override Widget CreateNode(string nodeClass)
		{
			switch(nodeClass)
			{
				// Override widget with UIWidget
				case "Widget": return new UIWidget();
				default: return base.CreateNode(nodeClass);
			}
		}

		public new UserInterface Instantiate(MarkupDocument document) => (UserInterface)this.Map(document);
	}
}
