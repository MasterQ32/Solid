using Solid.Markup;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace Solid.UI.Skinning
{
	public sealed class Skin : SolidObject, INamedNodeContainer
	{
		private readonly Dictionary<string, Style> styles = new Dictionary<string, Style>();

		void INamedNodeContainer.SetChildNodeName(object child, string name) => styles.Add(name, (Style)child);

		public Style this[string name]
		{
			get { return this.styles.ContainsKey(name) ? this.styles[name] : null; }
			set { this.styles[name] = value; }
		}

		public Style Button => this["Button"];
		public Style Panel => this["Panel"];
		public Style Label => this["Label"];
		public Style Window => this["Window"];

		public static Skin Load(string fileName)
		{
			var document = Parser.Load(fileName);

			var skinMapper = new SkinMapper();

			return skinMapper.Instantiate(document);
		}

		public static Skin Load(Stream stream, Encoding encoding)
		{
			var document = Parser.Parse(stream, encoding);

			var skinMapper = new SkinMapper();

			return skinMapper.Instantiate(document);
		}
	}
}
