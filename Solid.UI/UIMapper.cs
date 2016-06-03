using Solid.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solid.Markup;
using Solid.UI.Skinning;
using Solid.UI.Converters;

namespace Solid.UI
{
	public class UIMapper : LayoutMapper
	{
		protected override IMarkupDocument<Widget> CreateDocument() => new Form();

		public static void RegisterConverters<T>(MarkupMapper<T> mapper)
			where T : class
		{
			mapper.RegisterConverter<Color, ColorConverter>();
			// mapper.RegisterConverter<Brush, BrushConverter>();
			// mapper.RegisterConverter<Skin, SkinConverter>();
			mapper.RegisterConverter<Rectangle, RectangleConverter>();
			//mapper.RegisterConverter<Face, FaceConverter>();
			//mapper.RegisterConverter<Texture, TextureConverter>();
		}

		public UIMapper()
		{
			RegisterConverters(this);
			
			this.RegisterType<Button>();
			this.RegisterType<Label>();
			// this.RegisterType<Image>();
		}

		protected override Widget CreateNode(string nodeClass)
		{
			switch(nodeClass)
			{
				// Override widget with UIWidget
				case "Widget": return new UIWidget();
				case "Panel": return new Panel();
				default: return base.CreateNode(nodeClass);
			}
		}

		public new Form Instantiate(MarkupDocument document) => (Form)this.Map(document);
	}
}
