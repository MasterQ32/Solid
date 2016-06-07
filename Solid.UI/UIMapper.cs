namespace Solid.UI
{
	using Solid.Layout;
	using Solid.Markup;
	using Solid.UI.Skinning;
	using Solid.UI.Converters;
	using Solid.UI.Widgets;
	using Skinning.Converters;
	public class UIMapper : LayoutMapper
	{
		private readonly IGraphicsObjectFactory factory;

		protected override IMarkupDocument<Widget> CreateDocument() => new Form(this.factory);

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

		public UIMapper(IGraphicsObjectFactory factory)
		{
			this.factory = factory;

			RegisterConverters(this);

			this.RegisterConverter<IPicture>(new PictureConverter(factory));

			this.RegisterType<Button>();
			this.RegisterType<Label>();
			this.RegisterType<Image>();
		}

		protected override Widget CreateNode(string nodeClass)
		{
			switch(nodeClass)
			{
				// Override widget with UIWidget
				case "Widget": return new UIWidget();
				case "Panel": return new Widgets.Panel();
				default: return base.CreateNode(nodeClass);
			}
		}

		public new Form Instantiate(MarkupDocument document) => (Form)this.Map(document);
	}
}
