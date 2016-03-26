using System;

namespace Solid.Markup
{
	public sealed class NativeMapper : MarkupMapper<object>
	{
		private readonly TypeMapper types;

		public event EventHandler<NodeEventArgs> NamedNodeEmitted;

		public NativeMapper(TypeMapper types)
		{
			this.types = types;
		}

		public object Instantiate(MarkupDocument document)
		{
			var doc = (StubDocument)this.Map(document);
			doc.NamedNodeEmitted -= Doc_NamedNodeEmitted;
			return doc.Root;
		}

		protected override IMarkupDocument<object> CreateDocument()
		{
			var doc = new StubDocument();
			doc.NamedNodeEmitted += Doc_NamedNodeEmitted;
			return doc;
		}

		private void Doc_NamedNodeEmitted(object sender, NodeEventArgs e)
		{
			this.NamedNodeEmitted?.Invoke(this, e);
		}

		protected override object CreateNode(string nodeClass)
		{
			var type = types.Get(nodeClass);
			if (type == null)
				throw new InvalidOperationException($"Could not map {nodeClass} to a type.");
			return Activator.CreateInstance(type);
		}
	}
}