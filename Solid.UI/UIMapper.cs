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

		public static void RegisterConverters<T>(MarkupMapper<T> mapper)
			where T : class
		{
			mapper.RegisterConverter<Color, ColorConverter>();
			mapper.RegisterConverter<Brush, BrushConverter>();
			mapper.RegisterConverter<Skin, SkinConverter>();
			mapper.RegisterConverter<Rectangle, RectangleConverter>();
			mapper.RegisterConverter<Face, FaceConverter>();
			mapper.RegisterConverter<Color4, Color4Converter>();
		}

		public UIMapper()
		{
			RegisterConverters(this);

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
