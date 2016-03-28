﻿using OpenTK.Graphics;
using SharpFont;
using Solid.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI.Skinning
{
	public sealed class SkinMapper : SolidMapper<SolidObject>
	{
		public SkinMapper()
		{
			this.RegisterType<Skin>();
			this.RegisterType<Style>();

			this.RegisterType<TextureBrushDescriptor>("Texture");
			this.RegisterType<SolidBrushDescriptor>("Color");
			this.RegisterType<TextureBoxBrushDescriptor>("TextureBox");

			this.RegisterConverter<Color4, Color4Converter>();
			this.RegisterConverter<Rectangle, RectangleConverter>();
			this.RegisterConverter<Face, FaceConverter>();
		}

		protected override IMarkupDocument<SolidObject> CreateDocument() => new Document();

		public Skin Instantiate(MarkupDocument document)
		{
			var doc = (Document)this.Map(document);
			return doc.Skin;
		}
		
		private class Document : IMarkupDocument<SolidObject>
		{
			public Skin Skin{ get; private set; }

			public void NotifyCreateNode(SolidObject node) { }

			public void SetNodeName(SolidObject node, string name) { }

			public void SetRoot(SolidObject root)
			{
				this.Skin = (Skin)root;
			}
		}
	}
}
