namespace Solid.Markup
{
	public interface IMarkupDocument<T>
		where T : class
	{
		void SetRoot(T root);

		void SetNodeName(T node, string name);
	}
}