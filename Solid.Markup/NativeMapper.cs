using System;

namespace Solid.Markup
{
	public sealed class NativeMapper : MarkupMapper<object>
	{
		private readonly TypeMapper types;

		public NativeMapper(TypeMapper types)
		{
			this.types = types;
		}

		public object Instantiate(MarkupDocument document)
		{
			return ((StubDocument)this.Map(document)).Root;
		}

		protected override IMarkupDocument<object> CreateDocument() => new StubDocument();

		protected override object CreateNode(string nodeClass)
		{
			var type = types.Get(nodeClass);
			if (type == null)
				throw new InvalidOperationException($"Could not map {nodeClass} to a type.");
			return Activator.CreateInstance(type);
		}
	}
}