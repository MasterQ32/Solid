using Solid.Layout;
using Solid.Markup;
using Solid.UI.Skinning;
using System;
using System.Collections.Generic;
using System.IO;

namespace Solid.UI
{
	public sealed class Form : LayoutDocument
	{
		private static readonly Dictionary<string, Type> customTypes = new Dictionary<string, Type>();

		public Form() :
			base(new UIMapper())
		{

		}

		public object ViewModel
		{
			get { return this.Root.BindingSource; }
			set { this.Root.BindingSource = value; }
		}
		
		internal UserInterface UI { get; set; }

		public Skin Skin => this.UI.Skin;

		protected override void OnNodeCreation(Widget node)
		{
			UIWidget.FormProperty.SetValue(node, this);
			base.OnNodeCreation(node);
		}

		#region Custom Widgets

		public static void RegisterCustomWidget<T>(string name)
					where T : UIWidget
		{
			lock (customTypes)
				customTypes.Add(name, typeof(T));
		}

		public static void RegisterCustomWidget<T>()
			where T : UIWidget
			=> RegisterCustomWidget<T>(typeof(T).Name);

		public static Form Load(string fileName)
		{
			var document = Parser.Load(fileName);
			return Create(document);
		}

		public static Form Load(Stream stream, System.Text.Encoding encoding)
		{
			var document = Parser.Parse(stream, encoding);
			return Create(document);
		}

		private static Form Create(MarkupDocument document)
		{
			var mapper = new UIMapper();
			lock (customTypes)
			{
				foreach (var ctype in customTypes)
					mapper.RegisterType(ctype.Value, ctype.Key);
			}
			return mapper.Instantiate(document);
		}

		#endregion

	}
}